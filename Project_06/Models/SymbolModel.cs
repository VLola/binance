using ScottPlot;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Project_06.Models
{
    public class SymbolModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
        public List<OHLC> oHLCs { get; set; }
        public Algorithms Algorithms { get; set; } = new();
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
        private int _plus { get; set; } = 0;
        public int Plus
        {
            get { return _plus; }
            set
            {
                _plus = value;
                OnPropertyChanged("Plus");
            }
        }
        private int _minus { get; set; } = 0;
        public int Minus
        {
            get { return _minus; }
            set
            {
                _minus = value;
                OnPropertyChanged("Minus");
            }
        }
        private double _plusPercent { get; set; } = 0;
        public double PlusPercent
        {
            get { return _plusPercent; }
            set
            {
                _plusPercent = value;
                OnPropertyChanged("PlusPercent");
            }
        }
        private double _minusPercent { get; set; } = 0;
        public double MinusPercent
        {
            get { return _minusPercent; }
            set
            {
                _minusPercent = value;
                OnPropertyChanged("MinusPercent");
            }
        }
    }
}
