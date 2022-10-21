using Binance.Net.Clients;
using Binance.Net.Objects;
using Binance.Net.Objects.Models.Spot;
using CryptoExchange.Net.Authentication;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Project_04.Command;
using Project_04.Models;
using Project_04.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace Project_04.ViewModels
{
    internal class MainViewModel
    {
        private string _pathLog = $"{Directory.GetCurrentDirectory()}/log/";
        private string _pathHistory = $"{Directory.GetCurrentDirectory()}/history/";
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
            if (!Directory.Exists(_pathHistory)) Directory.CreateDirectory(_pathHistory);
            MainModel.PropertyChanged += MainModel_PropertyChanged;
            // Delete
            _client = new();
            MainModel.IsLogin = true;
            GetSumbolName();
        }

        private async void Calculate()
        {
            await Task.Run(() =>
            {
                foreach (var item in MainModel.Symbols)
                {
                    Task.Run(() => {
                        item.ShowInfo();
                    });
                }
            });
        }
        private void LoginClient()
        {
            using (var client = new WebClient())
            {
                new ConfigurationBuilder().AddUserSecrets<MainViewModel>()
                 .Build()
                 .Providers
                 .First()
                 .TryGet("linkStr", out var linkStr);
                string json = client.DownloadString(linkStr);
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
            if(e.PropertyName == "TakeProfit")
            {
                if(MainModel.TakeProfit > 0m)
                {
                    foreach (var item in MainModel.Symbols)
                    {
                        item.Symbol.TakeProfit = MainModel.TakeProfit;
                    }
                }
            }
            else if (e.PropertyName == "StopLoss")
            {
                if (MainModel.TakeProfit > 0m)
                {
                    foreach (var item in MainModel.Symbols)
                    {
                        item.Symbol.StopLoss = MainModel.StopLoss;
                    }
                }
            }
            else if (e.PropertyName == "IsSelect")
            {
                foreach (var item in MainModel.Symbols)
                {
                    item.Symbol.IsSelect = MainModel.IsSelect;
                }
            }
            else if (e.PropertyName == "StartTime")
            {
                foreach (var item in MainModel.Symbols)
                {
                    item.Symbol.StartTime = MainModel.StartTime;
                }
            }
            else if (e.PropertyName == "EndTime")
            {
                foreach (var item in MainModel.Symbols)
                {
                    item.Symbol.EndTime = MainModel.EndTime;
                }
            }
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
