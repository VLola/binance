using Binance.Net.Clients;
using Binance.Net.Objects;
using Binance.Net.Objects.Models.Spot;
using CryptoExchange.Net.Authentication;
using Newtonsoft.Json;
using Project_03.Command;
using Project_03.Models;
using Project_03.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Project_03.ViewModels
{
    internal class MainViewModel
    {
        private string _link = "https://drive.google.com/u/0/uc?id=13RLR9SIMLL2ibwDh8ouByOElk6Yw784J&export=download";
        private string _pathLog = $"{Directory.GetCurrentDirectory()}/log/";
        public MainModel MainModel { get; set; } = new();
        private BinanceClient? _client { get; set; }
        private BinanceSocketClient? _socketClient { get; set; }
        private RelayCommand? _loginCommand;
        public RelayCommand LoginCommand
        {
            get { return _loginCommand ?? (_loginCommand = new RelayCommand(obj => { LoginClient(); })); }
        }
        private RelayCommand? _calculateCommand;
        public RelayCommand CalculateCommand
        {
            get { return _calculateCommand ?? (_calculateCommand = new RelayCommand(obj => { Calculate(); })); }
        }
        public MainViewModel()
        {
            if (!Directory.Exists(_pathLog)) Directory.CreateDirectory(_pathLog);
            MainModel.PropertyChanged += MainModel_PropertyChanged;
        }
        private async void Calculate()
        {
            await Task.Run(() =>
            {
                foreach (var item in MainModel.Symbols)
                {
                    item.ShowInfo();
                }
            });
        }
        private void LoginClient()
        {
            using (var client = new WebClient())
            {
                string json = client.DownloadString(_link);
                List<ClientModel>? clientModels = JsonConvert.DeserializeObject<List<ClientModel>>(json);
                if (clientModels != null)
                {
                    bool check = false;
                    foreach (var item in clientModels)
                    {
                        if (item.ClientName == MainModel.Name) check = item.Access;
                    }
                    if (check)
                    {
                        Load();
                    }
                    else
                    {
                        MessageBox.Show("Login name failed!");
                    }
                }
            }
        }
        private void Load()
        {
            if (!MainModel.IsReal)
            {
                // ------------- Test Api ----------------
                BinanceClientOptions clientOption = new();
                clientOption.UsdFuturesApiOptions.BaseAddress = "https://testnet.binancefuture.com";
                _client = new(clientOption);

                BinanceSocketClientOptions socketClientOption = new BinanceSocketClientOptions
                {
                    AutoReconnect = true,
                    ReconnectInterval = TimeSpan.FromMinutes(1)
                };
                socketClientOption.UsdFuturesStreamsOptions.BaseAddress = "wss://stream.binancefuture.com";
                _socketClient = new BinanceSocketClient(socketClientOption);
                // ------------- Test Api ----------------
            }
            else
            {
                // ------------- Real Api ----------------
                _client = new();
                _socketClient = new();
                // ------------- Real Api ----------------
            }

            try
            {
                _client.SetApiCredentials(new ApiCredentials(MainModel.ApiKey, MainModel.SecretKey));
                _socketClient.SetApiCredentials(new ApiCredentials(MainModel.ApiKey, MainModel.SecretKey));

                MainModel.ApiKey = "";
                MainModel.SecretKey = "";
                if (CheckLogin())
                {
                    MainModel.IsLogin = true;
                    GetSumbolName();
                }
                else MessageBox.Show("Login failed!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private bool CheckLogin()
        {
            try
            {
                var result = _client.UsdFuturesApi.Account.GetAccountInfoAsync().Result;
                if (!result.Success)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        private void MainModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            
        }
        #region - List Sumbols -
        private void GetSumbolName()
        {
            List<string> list = new();
            foreach (var it in ListSymbols())
            {
                list.Add(it.Symbol);
            }
            list.Sort();
            foreach (var it in list)
            {
                SymbolViewModel symbolViewModel = new(_client, _socketClient, it);
                MainModel.Symbols.Add(symbolViewModel);
            }
        }
        private List<BinancePrice> ListSymbols()
        {
            try
            {
                var result = _client.UsdFuturesApi.ExchangeData.GetPricesAsync().Result;
                if (!result.Success) WriteLog($"Failed Success ListSymbols: {result.Error?.Message}");
                return result.Data.ToList();
            }
            catch (Exception ex)
            {
                WriteLog($"Failed ListSymbols: {ex.Message}");
                return null;
            }
        }
        #endregion

        private void WriteLog(string text)
        {
            try
            {
                File.AppendAllText(_pathLog + "_MAIN_LOG", $"{DateTime.Now} {text}\n");
            }
            catch { }
        }
    }
}
