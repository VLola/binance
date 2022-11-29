using System;
using System.Collections.Generic;

namespace Project_06.Models
{
    public class AlgorithmModel
    {
        public List<double> x = new();
        public List<double> y = new();
        public List<double> xIndicatorLong = new();
        public List<double> yIndicatorLong = new();
        public List<double> xIndicatorShort = new();
        public List<double> yIndicatorShort = new();
        public int Plus = 0;
        public int Minus = 0;
        public double PlusPercent = 0;
        public double MinusPercent = 0;
    }
}
