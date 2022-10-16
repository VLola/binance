using System;

namespace Project_02.Models
{
    public class Order
    {
        public decimal OpenPrice { get; set; }
        public decimal ClosePrice { get; set; }
        public DateTime OpenTime { get; set; }
        public DateTime CloseTime { get; set; }
        public string PositionSide { get; set; }
        public decimal Profit { get; set; }
        public bool IsClose { get; set; }
    }
}
