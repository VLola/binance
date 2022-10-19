using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Project_03.Models
{
    public class SymbolModel: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
        private string _name { get; set; }
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged("Name");
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
        private decimal _averageFifteenMinutes { get; set; }
        public decimal AverageFifteenMinutes
        {
            get { return _averageFifteenMinutes; }
            set
            {
                _averageFifteenMinutes = value;
                OnPropertyChanged("AverageFifteenMinutes");
            }
        }
        private decimal _averageOneHour { get; set; }
        public decimal AverageOneHour
        {
            get { return _averageOneHour; }
            set
            {
                _averageOneHour = value;
                OnPropertyChanged("AverageOneHour");
            }
        }
        private decimal _averageFourHour { get; set; }
        public decimal AverageFourHour
        {
            get { return _averageFourHour; }
            set
            {
                _averageFourHour = value;
                OnPropertyChanged("AverageFourHour");
            }
        }
        private decimal _averageEightHour { get; set; }
        public decimal AverageEightHour
        {
            get { return _averageEightHour; }
            set
            {
                _averageEightHour = value;
                OnPropertyChanged("AverageEightHour");
            }
        }
        private decimal _averageTwelveHour { get; set; }
        public decimal AverageTwelveHour
        {
            get { return _averageTwelveHour; }
            set
            {
                _averageTwelveHour = value;
                OnPropertyChanged("AverageTwelveHour");
            }
        }
        private decimal _averageOneDay { get; set; }
        public decimal AverageOneDay
        {
            get { return _averageOneDay; }
            set
            {
                _averageOneDay = value;
                OnPropertyChanged("AverageOneDay");
            }
        }

        private bool _isLongTP { get; set; }
        public bool IsLongTP
        {
            get { return _isLongTP; }
            set
            {
                _isLongTP = value;
                OnPropertyChanged("IsLongTP");
            }
        }
        private bool _isShortTP { get; set; }
        public bool IsShortTP
        {
            get { return _isShortTP; }
            set
            {
                _isShortTP = value;
                OnPropertyChanged("IsShortTP");
            }
        }
        private decimal _takeProfit { get; set; } = 0.01m;
        public decimal TakeProfit
        {
            get { return _takeProfit; }
            set
            {
                _takeProfit = value;
                OnPropertyChanged("TakeProfit");
            }
        }
        private string _position { get; set; }
        public string Position
        {
            get { return _position; }
            set
            {
                _position = value;
                OnPropertyChanged("Position");
            }
        }
    }
}
