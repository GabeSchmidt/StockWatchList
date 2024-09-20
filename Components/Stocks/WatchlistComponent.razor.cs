using CodingDemo.DAL;
using CodingDemo.Interfaces.DAL;
using CodingDemo.Interfaces.TwelveData;
using CodingDemo.Models.TwelveData;
using CodingDemo.Services.TwelveData;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;

namespace CodingDemo.Components.Stocks
{
    public partial class WatchlistComponent
    {
        [Inject] AuthenticationStateProvider authenticationStateProvider { get; set; }
        [Inject] ITwelveDataService twelveDataService { get; set; }
        [Inject] ILoggingService loggingService { get; set; }
        [Inject] IDialogService dialogService { get; set; }

        IEnumerable<string>? allStocks { get; set; }
        IEnumerable<string>? userStocks { get; set; }

        List<Quote> watchlist = new List<Quote>();
        string UserName { get; set; }
        string stockSelected { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await GetUser();
            await GetStocksForAutoComplete();
            await GetWatchlist();
        }

        async Task GetUser()
        {
            var authState = await authenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;
            if (user.Identity.IsAuthenticated)
            {
                UserName = user.Identity.Name.Trim();
            }
        }

        async Task GetStocksForAutoComplete()
        {
            allStocks = await twelveDataService.GetStockList();
            StateHasChanged();
        }

        async Task GetWatchlist()
        {
            userStocks = await loggingService.GetWatchlist(UserName);
            var stocks = string.Join(",", userStocks);

            watchlist.Clear();
            watchlist = await twelveDataService.Quote(stocks);

            StateHasChanged();
        }

        async Task<IEnumerable<string>?> Search(string value, CancellationToken token)
        {
            if (string.IsNullOrEmpty(value))
            {
                return allStocks;
            }

            return allStocks?.Where(x => x.Contains(value, StringComparison.InvariantCultureIgnoreCase));
        }

        async Task StockSelected()
        {
            var split = stockSelected.Split("|", StringSplitOptions.TrimEntries);
            var symbol = split[0];

            await loggingService.AddToWatchlist(UserName, symbol);

            await GetWatchlist();
        }

        async Task ViewChart(string symbol, string company)
        {
            var options = new DialogOptions
            {
                CloseOnEscapeKey = true,
                CloseButton = true,
                NoHeader = true,
                Position = DialogPosition.TopCenter,
                MaxWidth = MaxWidth.Medium,
                FullWidth = true
            };

            var parms = new DialogParameters();
            parms.Add("Symbol", symbol);
            parms.Add("Company", company);

            await dialogService.ShowAsync<StockChartComponent>("", parms, options);
        }

        async Task DeleteFromWatchlist(Quote quote)
        {
            await loggingService.DeleteFromWatchlist(UserName, quote.symbol);
            watchlist.Remove(quote);

            StateHasChanged();
        }
    }
}
