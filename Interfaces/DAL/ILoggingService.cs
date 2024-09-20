using CodingDemo.DAL;
using System.Threading.Tasks;

namespace CodingDemo.Interfaces.DAL
{
    public interface ILoggingService
    {
        Task<List<string>> GetWatchlist(string user);
        Task AddToWatchlist(string user, string symbol);
        Task<List<Stockslist>> GetStocklist();
        Task TruncateStocklist();
        Task InsertStocklist(List<Stockslist> stocks);
        Task DeleteFromWatchlist(string user, string symbol);
    }
}
