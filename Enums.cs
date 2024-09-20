using System.ComponentModel;

namespace CodingDemo
{
    public enum TimeSeriesInterval
    {
        [Description("1min")]
        OneMinute,

        [Description("5min")]
        FiveMinutes,

        [Description("15min")]
        FifteenMinutes,

        [Description("30min")]
        ThirtyMinutes,

        [Description("45min")]
        FourtyFiveMinutes,

        [Description("1h")]
        OneHour,

        [Description("2h")]
        TwoHours,

        [Description("4h")]
        FourHours,

        [Description("1day")]
        OneDay,

        [Description("1week")]
        OneWeek,

        [Description("1month")]
        OneMonth
    }
}
