using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Project_01.Models
{
    public class MainModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
        public ObservableCollection<BindingModel> Charts { get; set; } = new();
        public ObservableCollection<string> Symbols { get; set; } = new();
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
        private int _count { get; set; } = 1;
        public int Count
        {
            get { return _count; }
            set
            {
                _count = value;
                OnPropertyChanged("Count");
            }
        }
    }
}
