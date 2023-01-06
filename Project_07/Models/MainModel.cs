using Project_07.ViewModels;
using System.Collections.ObjectModel;

namespace Project_07.Models
{
    public class MainModel: ChangedModel
    {
        public ObservableCollection<SymbolViewModel> Symbols { get; set; } = new();
        private SymbolModel _selectedSymbol { get; set; }
        public SymbolModel SelectedSymbol
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
