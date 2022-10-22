using ScottPlot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_04.Models
{
    public class ChartModel
    {
        public List<OHLC> OHLCs { get; set; } = new();
        public List<AverageModel> Averages { get; set; } = new();
        public OrderModel OpenOrders { get; set; } = new();
        public OrderModel CloseOrders { get; set; } = new();
        //public List<(double x, double y)> OpenOrders { get; set; } = new();
        //public List<(double x, double y)> CloseOrders { get; set; } = new();
    }
}
