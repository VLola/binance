using Binance.Net.Clients;
using Binance.Net.Enums;
using Binance.Net.Interfaces;
using Binance.Net.Objects.Models.Spot;
using CryptoExchange.Net.CommonObjects;
using CryptoExchange.Net.Interfaces;
using Newtonsoft.Json;
using Project_05.Command;
using Project_05.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_05.ViewModels
{
    internal class MainViewModel
    {
        private string _pathLog = $"{Directory.GetCurrentDirectory()}/log/";
        private string _pathHistory = $"{Directory.GetCurrentDirectory()}/history/";
        public MainModel MainModel { get; set; }
        private BinanceClient? _client { get; set; }
        private RelayCommand? _startCommand;
        public RelayCommand StartCommand
        {
            get { return _startCommand ?? (_startCommand = new RelayCommand(obj => { Start(); })); }
        }
        public MainViewModel()
        {
            if (!Directory.Exists(_pathHistory)) Directory.CreateDirectory(_pathHistory);
            if (!Directory.Exists(_pathLog)) Directory.CreateDirectory(_pathLog);
            _client = new();
            MainModel = new();
            GetSumbolName();
        }
        private async void Start()
        {
            await Task.Run(() =>{
                foreach (var item in MainModel.Symbols)
                {
                    Loading(item, MainModel.EndTime);
                }
            });
        }
        public void Loading(string symbol, DateTime dateTime)
        {
            List<IBinanceKline> binanceKlines = new();
            for (int i = 0; i < MainModel.CountEightHour; i++)
            {
                binanceKlines.InsertRange(0, Klines(symbol, KlineInterval.OneMinute, 480, endTime: dateTime));
                dateTime = dateTime.AddMinutes(-480);
                ShowLoad();
            }
            File.WriteAllText(_pathHistory + symbol, JsonConvert.SerializeObject(binanceKlines));
        }
        private async void ShowLoad()
        {
            await Task.Run(async () =>
            {
                MainModel.IsLoading = true;
                await Task.Delay(200);
                MainModel.IsLoading = false;
            });
        }
        public List<IBinanceKline> Klines(string symbol, KlineInterval interval, int limit, DateTime? startTime = null, DateTime? endTime = null)
        {
            try
            {
                var result = _client.UsdFuturesApi.ExchangeData.GetKlinesAsync(symbol: symbol, interval: interval, startTime: startTime, endTime: endTime, limit: limit).Result;
                if (!result.Success) WriteLog("Error Candles");
                else
                {
                    return result.Data.ToList();
                }
            }
            catch (Exception e)
            {
                WriteLog($"Candles {e.Message}");
            }
            return null;
        }
        #region - List Sumbols -
        private void GetSumbolName()
        {
            foreach (var it in ListSymbols())
            {
                MainModel.Symbols.Add(it.Symbol);
            }
            MainModel.Symbols.Sort();
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
