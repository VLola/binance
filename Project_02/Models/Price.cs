using System;
using System.Diagnostics;

namespace Project_02.Models
{
    public class Price
    {
        public decimal Value { get; set; }
        public DateTime Time { get; set; }
        public bool NotValid { get; set; }
    }
}
