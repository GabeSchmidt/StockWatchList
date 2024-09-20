using CodingDemo.DAL;
using CodingDemo.Interfaces.DAL;
using CodingDemo.Models.TwelveData;
using Microsoft.EntityFrameworkCore;

namespace CodingDemo.Services.DAL
{
    public class LoggingService : ILoggingService
    {
        readonly LoggingDbContext _loggingDbContext;

        public LoggingService(LoggingDbContext loggingDbContext)
        {
            _loggingDbContext = loggingDbContext;
        }

        public async Task<List<Stockslist>> GetStocklist()
        {
            try
            {
                return await _loggingDbContext.Stockslist
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task InsertStocklist(List<Stockslist> stocks)
        {
            try
            {
                await _loggingDbContext.Stockslist.AddRangeAsync(stocks);
                await _loggingDbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {

            }
        }

        public async Task TruncateStocklist()
        {
            try
            {
                await _loggingDbContext.Database.ExecuteSqlRawAsync("truncate table Stockslist");
            }
            catch (Exception ex)
            {

            }
        }

        public async Task<List<string>> GetWatchlist(string user)
        {
            try
            {
                return await _loggingDbContext.StocksWatchlist
                    .Where(x => x.UserName.ToLower().Trim() == user.ToLower().Trim())
                    .Select(x => x.StockSymbol.Trim())
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task AddToWatchlist(string user, string symbol)
        {
            try
            {
                await _loggingDbContext.StocksWatchlist.AddAsync(
                    new StocksWatchlist() { UserName = user, StockSymbol = symbol }
                );

                await _loggingDbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {

            }
        }

        public async Task DeleteFromWatchlist(string user, string symbol)
        {
            try
            {
                var result = await _loggingDbContext.StocksWatchlist
                    .Where(x => x.UserName.ToLower().Trim() == user.ToLower().Trim()
                             && x.StockSymbol.ToLower().Trim() == symbol.ToLower().Trim())
                    .FirstOrDefaultAsync();

                if (result != null)
                {
                    _loggingDbContext.StocksWatchlist.Remove(result);
                    await _loggingDbContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}
