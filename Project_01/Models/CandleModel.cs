using System;
using System.ComponentModel;
using System.Diagnostics.Metrics;
using System.Runtime.CompilerServices;
using System.Windows.Controls;

namespace Project_01.Models
{
    public class CandleModel: INotifyPropertyChanged
    {
        public CandleModel(CandleModel candleModel, double openTime, decimal minOpenPrice, bool isWhite)
        {
            OpenTime = openTime;
            OpenPrice = candleModel.OpenPrice - minOpenPrice;
            CloseTime = candleModel.CloseTime;
            ClosePrice = candleModel.ClosePrice;
            HighPrice = candleModel.HighPrice;
            LowPrice = candleModel.LowPrice - minOpenPrice;
            IsPositive = candleModel.IsPositive;
            if(isWhite) Color = "White";
            else
            {
                if(IsPositive) Color = "LawnGreen";
                else Color = "OrangeRed";
            }
        }
        public CandleModel(decimal maxPrice, decimal minPrice, double openTime, DateTime closeTime, decimal openPrice, decimal closePrice, decimal lowPrice, decimal highPrice)
        {
            OpenTime = openTime;
            CloseTime = closeTime;
            decimal high = 0m;
            decimal low = 0m;
            decimal open = 0m;
            decimal close = 0m;
            decimal mul = 1m;
            if (minPrice > 50000m)
            {
                mul = 0.1m;
            }
            else if (minPrice > 10000m)
            {
                mul = 1m;
            }
            else if (minPrice > 1000m)
            {
                mul = 10m;
            }
            else if (minPrice > 100m)
            {
                mul = 100m;
            }
            else if (minPrice > 10m)
            {
                mul = 1000m;
            }
            else if (minPrice > 1m)
            {
                mul = 10000m;
            }
            else if (minPrice > 0.1m)
            {
                mul = 100000m;
            }
            else if (minPrice > 0.01m)
            {
                mul = 1000000m;
            }
            else if (minPrice > 0.001m)
            {
                mul = 10000000m;
            }
            else if (minPrice > 0.0001m)
            {
                mul = 100000000m;
            }
            else if (minPrice > 0.00001m)
            {
                mul = 1000000000m;
            }
            high = (highPrice * mul) - (minPrice * mul);
            low = (lowPrice * mul) - (minPrice * mul);
            open = (openPrice * mul) - (minPrice * mul);
            close = (closePrice * mul) - (minPrice * mul);


            if (highPrice < lowPrice)
            {
                LowPrice = (high);
                HighPrice = (low - high);
            }
            else
            {
                LowPrice = (low);
                HighPrice = (high - low);
            }

            if (openPrice <= closePrice)
            {
                IsPositive = true;
                OpenPrice = (open);
                ClosePrice = (close - open);
                Color = "LawnGreen";
            }
            else
            {
                IsPositive = false;
                OpenPrice = (close);
                ClosePrice = (open - close);
                Color = "OrangeRed";
            }
        }
        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
        private double _openTime { get; set; }
        public double OpenTime
        {
            get { return _openTime; }
            set
            {
                _openTime = value;
                OnPropertyChanged("OpenTime");
            }
        }
        private DateTime _closeTime { get; set; }
        public DateTime CloseTime
        {
            get { return _closeTime; }
            set
            {
                _closeTime = value;
                OnPropertyChanged("CloseTime");
            }
        }
        private decimal _openPrice { get; set; }
        public decimal OpenPrice
        {
            get { return _openPrice; }
            set
            {
                _openPrice = value;
                OnPropertyChanged("OpenPrice");
            }
        }
        private decimal _highPrice { get; set; }
        public decimal HighPrice
        {
            get { return _highPrice; }
            set
            {
                _highPrice = value;
                OnPropertyChanged("HighPrice");
            }
        }
        private decimal _lowPrice { get; set; }
        public decimal LowPrice
        {
            get { return _lowPrice; }
            set
            {
                _lowPrice = value;
                OnPropertyChanged("LowPrice");
            }
        }
        private decimal _closePrice { get; set; }
        public decimal ClosePrice
        {
            get { return _closePrice; }
            set
            {
                _closePrice = value;
                OnPropertyChanged("ClosePrice");
            }
        }
        private bool _isPositive { get; set; }
        public bool IsPositive
        {
            get { return _isPositive; }
            set
            {
                _isPositive = value;
                OnPropertyChanged("IsPositive");
            }
        }
        private string _color { get; set; }
        public string Color
        {
            get { return _color; }
            set
            {
                _color = value;
                OnPropertyChanged("Color");
            }
        }

    }
}
