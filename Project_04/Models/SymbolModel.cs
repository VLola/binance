using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Project_04.Models
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

        private bool _isOpenOrder { get; set; }
        public bool IsOpenOrder
        {
            get { return _isOpenOrder; }
            set
            {
                _isOpenOrder = value;
                OnPropertyChanged("IsOpenOrder");
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

        private decimal _averageTwoDay { get; set; }
        public decimal AverageTwoDay
        {
            get { return _averageTwoDay; }
            set
            {
                _averageTwoDay = value;
                OnPropertyChanged("AverageTwoDay");
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
        private decimal _takeProfit { get; set; } = 0.05m;
        public decimal TakeProfit
        {
            get { return _takeProfit; }
            set
            {
                _takeProfit = value;
                OnPropertyChanged("TakeProfit");
            }
        }
        private decimal _stopLoss { get; set; } = 0.05m;
        public decimal StopLoss
        {
            get { return _stopLoss; }
            set
            {
                _stopLoss = value;
                OnPropertyChanged("StopLoss");
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
        private int _betPlus { get; set; } = 0;
        public int BetPlus
        {
            get { return _betPlus; }
            set
            {
                _betPlus = value;
                OnPropertyChanged("BetPlus");
                if (_betMinus > 0) PercentWin = (double)_betPlus / (double)_betMinus;
            }
        }
        private int _betMinus { get; set; } = 0;
        public int BetMinus
        {
            get { return _betMinus; }
            set
            {
                _betMinus = value;
                OnPropertyChanged("BetMinus");
                if(_betMinus > 0) PercentWin = (double)_betPlus / (double)_betMinus;
            }
        }
        private int _betIndefinite { get; set; } = 0;
        public int BetIndefinite
        {
            get { return _betIndefinite; }
            set
            {
                _betIndefinite = value;
                OnPropertyChanged("BetIndefinite");
            }
        }
        private bool _isSelect { get; set; }
        public bool IsSelect
        {
            get { return _isSelect; }
            set
            {
                _isSelect = value;
                OnPropertyChanged("IsSelect");
            }
        }
        private double _percentWin { get; set; }
        public double PercentWin
        {
            get { return _percentWin; }
            set
            {
                _percentWin = value;
                OnPropertyChanged("PercentWin");
            }
        }
        private DateTime _startTime { get; set; } = new DateTime(2022, 9, 20, 0, 0, 0);
        public DateTime StartTime
        {
            get { return _startTime; }
            set
            {
                _startTime = value;
                OnPropertyChanged("StartTime");
            }
        }
        private DateTime _endTime { get; set; } = new DateTime(2022, 10, 20, 0, 0, 0);
        public DateTime EndTime
        {
            get { return _endTime; }
            set
            {
                _endTime = value;
                OnPropertyChanged("EndTime");
            }
        }
    }
}
