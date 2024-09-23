using CodingDemo.DAL;
using CodingDemo.Interfaces.DAL;
using CodingDemo.Interfaces.TwelveData;
using CodingDemo.Models.TwelveData;
using CodingDemo.Services.TwelveData;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;
using System.Timers;

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
        System.Threading.Timer timer { get; set; }
        bool disableRefreshbutton = true;

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
            disableRefreshbutton = true;

            userStocks = await loggingService.GetWatchlist(UserName);
            if (userStocks != null)
            {
                var stocks = string.Join(",", userStocks);

                watchlist.Clear();
                watchlist = await twelveDataService.Quote(stocks);

                await EnableRefreshButton();

                StateHasChanged();
            }
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

        async Task EnableRefreshButton()
        {
            if (timer != null)
                await timer.DisposeAsync();

            timer = new System.Threading.Timer(x =>
            {
                InvokeAsync(() =>
                {
                    disableRefreshbutton = false;
                    StateHasChanged();
                });
            }, null, TimeSpan.FromMinutes(1), TimeSpan.FromMinutes(1));
        }
    }
}
