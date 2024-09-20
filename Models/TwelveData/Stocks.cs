namespace CodingDemo.Models.TwelveData
{
    public class Stocks
    {
        public List<Stock> data { get; set; }
        public int count { get; set; }
        public string status { get; set; }
    }

    public class Stock
    {
        public string symbol { get; set; }
        public string name { get; set; }
        public string currency { get; set; }
        public string exchange { get; set; }
        public string mic_code { get; set; }
        public string country { get; set; }
        public string type { get; set; }
        public string figi_code { get; set; }
    }
}
