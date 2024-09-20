using CodingDemo.DAL;
using CodingDemo.Interfaces.DAL;
using CodingDemo.Interfaces.TwelveData;
using CodingDemo.Models.TwelveData;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace CodingDemo.Services.TwelveData
{
    public class TwelveDataService : ITwelveDataService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILoggingService _loggingService;

        public TwelveDataService(IHttpClientFactory httpClientFactory, ILoggingService loggingService)
        {
            _httpClientFactory = httpClientFactory;
            _loggingService = loggingService;
        }

        /// <summary>
        /// This API call returns an array of symbols available at Twelve Data API. 
        /// This list is updated daily.
        /// </summary>
        /// <returns></returns>
        public async Task<List<string>?> GetStockList()
        {
            try
            {
                var stocklist = await _loggingService.GetStocklist();
                if (stocklist != null && stocklist.Count > 0)
                {
                    var lastupdated = stocklist.Max(x => x.LastUpdated);
                    if (lastupdated.Date == DateTime.Now.Date)
                    {
                        return stocklist.Select(x => $"{x.StockSymbol} | {x.CompanyName}").ToList();
                    }
                    else
                    {
                        return await TruncateAndInsert();
                    }
                }
                else
                {
                    return await TruncateAndInsert();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private async Task<List<string>?> TruncateAndInsert()
        {
            try
            {
                await _loggingService.TruncateStocklist();
                var stocks = await GetStocks();
                if (stocks != null)
                {
                    var toInsert = stocks.Select(x => new Stockslist()
                    {
                        StockSymbol = x.symbol,
                        CompanyName = x.name,
                        LastUpdated = DateTime.Now.Date
                    })
                    .ToList();

                    await _loggingService.InsertStocklist(toInsert);

                    return stocks.Select(x => $"{x.symbol} | {x.name}").ToList();
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private async Task<List<Stock>?> GetStocks()
        {
            try
            {
                var httpClient = _httpClientFactory.CreateClient("TwelveData");

                var results = await httpClient.GetFromJsonAsync<Stocks>(
                    $"stocks?country=United%20States",
                    new JsonSerializerOptions(JsonSerializerDefaults.Web)
                );

                return results?.data;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// This API call returns meta and time series for the requested instrument. 
        /// Metaobject consists of general information about the requested symbol. 
        /// Time series is the array of objects ordered by time descending with Open, High, Low, Close prices. 
        /// Non-currency instruments also include volume information.
        /// </summary>
        /// <param name="symbol">Symbol ticker of the instrument</param>
        /// <param name="interval">Supports: 1min, 5min, 15min, 30min, 45min, 1h, 2h, 4h, 1day, 1week, 1month</param>
        /// <returns></returns>
        public async Task<TimeSeries?> TimeSeries(string symbol, string interval)
        {
            try
            {
                var httpClient = _httpClientFactory.CreateClient("TwelveData");

                var results = await httpClient.GetFromJsonAsync<TimeSeries>(
                    $"time_series?symbol={symbol}&interval={interval}",
                    new JsonSerializerOptions(JsonSerializerDefaults.Web)
                );

                return results;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// Quote endpoint is an efficient method to retrieve the latest quote of the selected instrument.
        /// Interval default 1day
        /// </summary>
        /// <param name="symbols">comma separated list of stock symbols</param>
        /// <returns></returns>
        public async Task<List<Quote>> Quote(string symbols)
        {
            try
            {
                List<Quote> quotes = new List<Quote>();
                var httpClient = _httpClientFactory.CreateClient("TwelveData");

                var response = await httpClient.GetAsync($"quote?symbol={symbols}&country=United%20States");
                var json = await response.Content.ReadAsStringAsync();

                JsonNode node = JsonNode.Parse(json);
                foreach (var symbol in symbols.Split(","))
                {
                    JsonNode currentSymbol = node[symbol];
                    var quote = currentSymbol.Deserialize<Quote>();
                    quotes.Add(quote);
                }

                return quotes;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        
    }
}
