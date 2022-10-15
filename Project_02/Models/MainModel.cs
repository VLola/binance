using Project_02.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Project_02.Models
{
    internal class MainModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
        public ObservableCollection<SymbolViewModel> Symbols { get; set; } = new();
        private string _selectedClient { get; set; }
        public string SelectedClient
        {
            get { return _selectedClient; }
            set
            {
                _selectedClient = value;
                OnPropertyChanged("SelectedClient");
            }
        }
        private bool _isLogin { get; set; }
        public bool IsLogin
        {
            get { return _isLogin; }
            set
            {
                _isLogin = value;
                OnPropertyChanged("IsLogin");
            }
        }
        private bool _isReal { get; set; }
        public bool IsReal
        {
            get { return _isReal; }
            set
            {
                _isReal = value;
                OnPropertyChanged("IsReal");
            }
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
        private string _apiKey { get; set; }
        public string ApiKey
        {
            get { return _apiKey; }
            set
            {
                _apiKey = value;
                OnPropertyChanged("ApiKey");
            }
        }
        private string _secretKey { get; set; }
        public string SecretKey
        {
            get { return _secretKey; }
            set
            {
                _secretKey = value;
                OnPropertyChanged("SecretKey");
            }
        }
        private bool _selectAll { get; set; }
        public bool SelectAll
        {
            get { return _selectAll; }
            set
            {
                _selectAll = value;
                OnPropertyChanged("SelectAll");
            }
        }
    }
}
