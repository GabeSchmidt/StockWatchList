using CodingDemo.Models.TwelveData;

namespace CodingDemo.Interfaces.TwelveData
{
    public interface ITwelveDataService
    {
        Task<List<string>?> GetStockList();
        Task<TimeSeries?> TimeSeries(string symbol, string interval);
        Task<List<Quote>> Quote(string symbols);
    }
}
