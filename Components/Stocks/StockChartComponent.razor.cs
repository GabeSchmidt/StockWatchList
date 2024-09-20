using CodingDemo.Interfaces.TwelveData;
using CodingDemo.Models.TwelveData;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace CodingDemo.Components.Stocks
{
    public partial class StockChartComponent
    {
        [Parameter] public string Symbol { get; set; }
        [Parameter] public string Company { get; set; }

        [Inject] ITwelveDataService twelveDataService { get; set; }

        TimeSeriesInterval interval = TimeSeriesInterval.OneDay;
        TimeSeries timeSeries = new TimeSeries();
        List<ChartSeries> series = new List<ChartSeries>();
       
        string[] XAxisLabels = { DateTime.Now.ToString("yyyy-MM-dd") };

        protected override async Task OnInitializedAsync()
        {
            var stringInterval = interval.GetEnumDescription();
            timeSeries = await twelveDataService.TimeSeries(Symbol, stringInterval);

            series = new List<ChartSeries>()
            {
                new ChartSeries()
                {
                    Name = Company,
                    Data = timeSeries?.values.Select(x => double.Parse(x.close)).ToArray()
                }
            };

            XAxisLabels = timeSeries?.values.Select(x => DateTime.Parse(x.datetime).ToString("yyyy-MM-dd")).ToArray();
        }
    }
}
