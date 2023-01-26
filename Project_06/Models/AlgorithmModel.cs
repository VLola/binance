using System;
using System.Collections.Generic;

namespace Project_06.Models
{
    public class AlgorithmModel
    {
        public List<Bet> BetModels { get; set; } = new();
        public List<double> x = new();
        public List<double> y = new();
        public List<double> xClose = new();
        public List<double> yClose = new();
        public List<PointModel> Points = new();
        public int Plus = 0;
        public int Minus = 0;
        public double PlusPercent = 0;
        public double MinusPercent = 0;
        public int Open = 0;
        public int Close = 0;
    }
}
