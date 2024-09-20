namespace CodingDemo.Models.TwelveData
{
    public class Quote
    {
        public string symbol { get; set; }
        public string name { get; set; }
        public string exchange { get; set; }
        public string datetime { get; set; }
        public int timestamp { get; set; }
        public string open { get; set; }
        public string high { get; set; }
        public string low { get; set; }
        public string close { get; set; }
        public string previous_close { get; set; }
        public string change { get; set; }
        public string percent_change { get; set; }
        public bool is_market_open { get; set; }
        public Fifty_Two_Week fifty_two_week { get; set; }
    }

    public class Fifty_Two_Week
    {
        public string low { get; set; }
        public string high { get; set; }
        public string low_change { get; set; }
        public string high_change { get; set; }
        public string low_change_percent { get; set; }
        public string high_change_percent { get; set; }
        public string range { get; set; }
    }

}
