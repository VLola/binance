using Binance.Net.Clients;
using Binance.Net.Objects.Models.Futures;
using Project_07.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.IO;
using System.Linq;
using Project_07.Command;
using System.Windows;

namespace Project_07.ViewModels
{
    internal class MainViewModel
    {
        private string _pathHistory = $"{Directory.GetCurrentDirectory()}/history/";
        private string _pathLog = $"{Directory.GetCurrentDirectory()}/log/";
        public MainModel MainModel { get; set; } = new();
        private BinanceClient? _client { get; set; }
        private BinanceSocketClient? _socketClient { get; set; }
        private RelayCommand? _showSumCommand;
        public RelayCommand ShowSumCommand
        {
            get
            {
                return _showSumCommand ?? (_showSumCommand = new RelayCommand(obj => {
                    ShowSum();
                }));
            }
        }
        public MainViewModel()
        {
            if (!Directory.Exists(_pathLog)) Directory.CreateDirectory(_pathLog);
            if (!Directory.Exists(_pathHistory)) Directory.CreateDirectory(_pathHistory);
            _client = new();
            _socketClient = new();
            GetSumbolName();
        }
        private void ShowSum()
        {
            MainModel.Total = 0;
            MainModel.ShortPlus = 0;
            MainModel.ShortMinus = 0;
            MainModel.LongPlus = 0;
            MainModel.LongMinus = 0;
            foreach (var item in MainModel.Symbols)
            {
                MainModel.Total += item.SymbolModel.Total;
                MainModel.ShortPlus += item.SymbolModel.ShortPlus;
                MainModel.ShortMinus += item.SymbolModel.ShortMinus;
                MainModel.LongPlus += item.SymbolModel.LongPlus;
                MainModel.LongMinus += item.SymbolModel.LongMinus;
            }
        }
        private async void GetSumbolName()
        {
            await Task.Run(() => {
                List<string> list = new();
                List<BinanceFuturesUsdtSymbol> symbols = ListSymbols();
                foreach (var it in symbols)
                {
                    if (it.Name.EndsWith("USDT"))
                    {
                        list.Add(it.Name);
                    }
                }
                list.Sort();
                foreach (var it in list)
                {
                    BinanceFuturesUsdtSymbol symbol = symbols.FirstOrDefault(x => x.Name == it);
                    AddSymbol(symbol);
                }
            });
        }
        private void AddSymbol(BinanceFuturesUsdtSymbol symbol)
        {
            SymbolViewModel symbolViewModel = new(_client, _socketClient, symbol);
            App.Current.Dispatcher.Invoke(new Action(() =>
            {
                MainModel.Symbols.Add(symbolViewModel);
            }));
        }
        private List<BinanceFuturesUsdtSymbol> ListSymbols()
        {
            try
            {
                var result = _client.UsdFuturesApi.ExchangeData.GetExchangeInfoAsync().Result;
                if (!result.Success) WriteLog($"Failed ListSymbols {result.Error?.Message}");
                return result.Data.Symbols.ToList();
            }
            catch (Exception ex)
            {
                WriteLog($"ListSymbols: {ex.Message}");
            }
            return null;
        }
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
