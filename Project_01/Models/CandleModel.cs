using System;
using System.ComponentModel;
using System.Diagnostics.Metrics;
using System.Runtime.CompilerServices;
using System.Windows.Controls;

namespace Project_01.Models
{
    public class CandleModel: INotifyPropertyChanged
    {
        public CandleModel(decimal maxHighPrice, double openTime, DateTime closeTime, decimal openPrice, decimal closePrice, decimal lowPrice, decimal highPrice)
        {
            OpenTime = openTime;
            CloseTime = closeTime;
            decimal mul = 100m / maxHighPrice;
            decimal X = 200m;
            decimal minus = 97m;


            decimal high = ((highPrice * mul) - minus) * X;
            decimal low = ((lowPrice * mul) - minus) * X;
            decimal open = ((openPrice * mul) - minus) * X;
            decimal close = ((closePrice * mul) - minus) * X;


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
