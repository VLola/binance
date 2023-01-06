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
        private double _total { get; set; }
        public double Total
        {
            get { return _total; }
            set
            {
                _total = value;
                OnPropertyChanged("Total");
            }
        }
        private int _shortPlus { get; set; }
        public int ShortPlus
        {
            get { return _shortPlus; }
            set
            {
                _shortPlus = value;
                OnPropertyChanged("ShortPlus");
            }
        }
        private int _shortMinus { get; set; }
        public int ShortMinus
        {
            get { return _shortMinus; }
            set
            {
                _shortMinus = value;
                OnPropertyChanged("ShortMinus");
            }
        }
        private int _longPlus { get; set; }
        public int LongPlus
        {
            get { return _longPlus; }
            set
            {
                _longPlus = value;
                OnPropertyChanged("LongPlus");
            }
        }
        private int _longMinus { get; set; }
        public int LongMinus
        {
            get { return _longMinus; }
            set
            {
                _longMinus = value;
                OnPropertyChanged("LongMinus");
            }
        }
    }
}
