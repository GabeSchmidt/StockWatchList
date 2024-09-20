using Microsoft.EntityFrameworkCore;

namespace CodingDemo.DAL
{
    public class LoggingDbContext : DbContext
    {
        public LoggingDbContext(DbContextOptions<LoggingDbContext> options) : base(options) { }

        public DbSet<StocksWatchlist> StocksWatchlist { get; set; }
        public DbSet<Stockslist> Stockslist { get; set; }
    }

    public class Stockslist
    {
        public int ID { get; set; }
        public string StockSymbol { get; set; }
        public string CompanyName { get; set; }
        public DateTime LastUpdated { get; set; }
    }

    public class StocksWatchlist
    {
        public int ID { get; set; }
        public string UserName { get; set; }
        public string StockSymbol { get; set; }
    }

}
