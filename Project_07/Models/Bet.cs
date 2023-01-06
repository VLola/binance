using System;

namespace Project_07.Models
{
    public class Bet
    {
        public string Name { get; set; }
        public bool IsLong { get; set; }
        public bool IsPositive { get; set; }
        public decimal OpenPrice { get; set; }
        public decimal ClosePrice { get; set; }
        public DateTime OpenTime { get; set; }
        public DateTime CloseTime { get; set; }
        public DateTime KlineOpenTime { get; set; }
    }
}
