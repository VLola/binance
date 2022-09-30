using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Project_01.Models
{
    public class ChartModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
        public ObservableCollection<string> Symbols { get; set; } = new();
        public ObservableCollection<CandleModel> Candles { get; set; } = new();
        public ObservableCollection<CandleModel> ChartCandles { get; set; } = new();
        public ObservableCollection<ObservableCollection<CandleModel>> ListsCandles { get; set; } = new();

        private string _selectedSymbol { get; set; }
        public string SelectedSymbol
        {
            get { return _selectedSymbol; }
            set
            {
                _selectedSymbol = value;
                OnPropertyChanged("SelectedSymbol");
            }
        }
    }
}
