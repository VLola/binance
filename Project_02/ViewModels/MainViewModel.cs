using Project_02.Command;
using Project_02.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;

namespace Project_02.ViewModels
{
    internal class MainViewModel
    {
        private string _pathClients = Directory.GetCurrentDirectory() + @"\clients\";
        public MainModel MainModel { get; set; } = new();
        private RelayCommand? _loginCommand;
        public RelayCommand LoginCommand
        {
            get { return _loginCommand ?? (_loginCommand = new RelayCommand(obj => { LoginClient(); })); }
        }
        private RelayCommand? _saveCommand;
        public RelayCommand SaveCommand
        {
            get { return _saveCommand ?? (_saveCommand = new RelayCommand(obj => { SaveClient(); })); }
        }
        public MainViewModel()
        {
            LoadClients();
        }
        private void SaveClient()
        {
            if (!Directory.Exists(_pathClients)) Directory.CreateDirectory(_pathClients);
            Client client = new();
            client.Name = MainModel.Name;
            client.ApiKey = MainModel.ApiKey;
            client.SecretKey = MainModel.SecretKey;
            client.IsReal = MainModel.IsReal;
            string fileName;
            if (MainModel.IsReal) fileName = MainModel.Name + "-real";
            else fileName = MainModel.Name + "-test";
            File.WriteAllText(_pathClients + fileName, JsonSerializer.Serialize(client));
            MainModel.Name = "";
            MainModel.ApiKey = "";
            MainModel.SecretKey = "";
            LoadClients();
        }
        private void LoginClient()
        {
            if (!Directory.Exists(_pathClients)) Directory.CreateDirectory(_pathClients);
            Client? client = JsonSerializer.Deserialize<Client>(File.ReadAllText(_pathClients + MainModel.SelectedClient));
            if(client != null)
            {
                MessageBox.Show(client.Name);
            }
        }
        private void LoadClients()
        {
            if (!Directory.Exists(_pathClients)) Directory.CreateDirectory(_pathClients);
            MainModel.Clients.Clear();
            List<string> filesDir = (from file in Directory.GetFiles(_pathClients) select Path.GetFileName(file)).ToList();
            if(filesDir.Count > 0)
            {
                foreach (var item in filesDir)
                {
                    MainModel.Clients.Add(item);
                }
                MainModel.SelectedClient = MainModel.Clients[0];
            }
        }
    }
}
