using Project_01.Command;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace Project_01.Models
{
    public class ChartModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
        public ObservableCollection<CandleModel> Candles { get; set; } = new();
        private double _width { get; set; }
        public double Width
        {
            get { return _width; }
            set
            {
                _width = value;
                OnPropertyChanged("Width");
            }
        }
        private double _height { get; set; }
        public double Height
        {
            get { return _height; }
            set
            {
                _height = value;
                OnPropertyChanged("Height");
            }
        }
        private double _scaleY { get; set; } = 1;
        public double ScaleY
        {
            get { return _scaleY; }
            set
            {
                _scaleY = value;
                OnPropertyChanged("ScaleY");
            }
        }
        private Thickness _margin { get; set; }
        public Thickness Margin
        {
            get { return _margin; }
            set
            {
                _margin = value;
                OnPropertyChanged("Margin");
            }
        }

        private RelayCommand? _increaseCommand;
        public RelayCommand IncreaseCommand
        {
            get { return _increaseCommand ?? (_increaseCommand = new RelayCommand(obj => { Increase(); })); }
        }
        private RelayCommand? _decreaseCommand;
        public RelayCommand DecreaseCommand
        {
            get { return _decreaseCommand ?? (_decreaseCommand = new RelayCommand(obj => { Decrease(); })); }
        }
        private void Increase()
        {
            ScaleY = ScaleY + 0.1;
            Height = 200 * ScaleY;
            Margin = new Thickness(0, 200 * ScaleY * 7, 0, 0);
        }
        private void Decrease()
        {
            ScaleY = ScaleY - 0.1;
            Height = 200 * ScaleY;
            Margin = new Thickness(0, 200 * ScaleY * 7, 0, 0);
        }
    }
}
