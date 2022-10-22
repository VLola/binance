using System.Collections.Generic;

namespace Project_04.Models
{
    public class OrderModel
    {
        public List<double> OpenPrices { get; set; } = new();
        public List<double> OpenTimes { get; set; } = new();
    }
}
