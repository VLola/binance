using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Project_05.Models
{
    internal class MainModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
        public List<string> Symbols = new();

        private int _countEightHour { get; set; } = 1;
        public int CountEightHour
        {
            get { return _countEightHour; }
            set
            {
                _countEightHour = value;
                OnPropertyChanged("CountEightHour");
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
        private bool _isLoading { get; set; }
        public bool IsLoading
        {
            get { return _isLoading; }
            set
            {
                _isLoading = value;
                OnPropertyChanged("IsLoading");
            }
        }
    }
}
