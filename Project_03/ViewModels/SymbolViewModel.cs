using Binance.Net.Clients;
using Binance.Net.Enums;
using Binance.Net.Objects.Models.Spot;
using CryptoExchange.Net.CommonObjects;
using CryptoExchange.Net.Sockets;
using Newtonsoft.Json;
using Project_03.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace Project_03.ViewModels
{
    internal class SymbolViewModel
    {
        public SymbolModel Symbol { get; set; }
        private BinanceSocketClient _socketClient { get; set; }
        private BinanceClient _client { get; set; }
        private string _pathLog = $"{Directory.GetCurrentDirectory()}/log/";
        public SymbolViewModel(BinanceClient client, BinanceSocketClient socketClient, string symbolName)
        {
            Symbol = new();
            _client = client;
            _socketClient = socketClient;
            Symbol.Name = symbolName;
            Symbol.PropertyChanged += Symbol_PropertyChanged;
        }

        private void Symbol_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            
        }
        public void ShowInfo()
        {
            decimal? fifteenMinutes = Candles(KlineInterval.OneMinute, 15);
            if (fifteenMinutes != null) Symbol.AverageFifteenMinutes = (decimal)fifteenMinutes;

            decimal? oneHour = Candles(KlineInterval.OneMinute, 60);
            if (oneHour != null) Symbol.AverageOneHour = (decimal)oneHour;

            decimal? fourHour = Candles(KlineInterval.FifteenMinutes, 16);
            if (fourHour != null) Symbol.AverageFourHour = (decimal)fourHour;

            decimal? eightHour = Candles(KlineInterval.FifteenMinutes, 32);
            if (eightHour != null) Symbol.AverageEightHour = (decimal)eightHour;

            decimal? twelveHour = Candles(KlineInterval.FifteenMinutes, 48);
            if (twelveHour != null) Symbol.AverageTwelveHour = (decimal)twelveHour;

            decimal? oneDay = Candles(KlineInterval.ThirtyMinutes, 24);
            if (oneDay != null) Symbol.AverageOneDay = (decimal)oneDay;

            if(Symbol.AverageFifteenMinutes <= Symbol.AverageOneHour &&
                Symbol.AverageOneHour <= Symbol.AverageFourHour &&
                Symbol.AverageFourHour <= Symbol.AverageEightHour &&
                Symbol.AverageEightHour <= Symbol.AverageTwelveHour &&
                Symbol.AverageTwelveHour <= Symbol.AverageOneDay)
            {
                Symbol.IsPositive = true;
            }
            else if (Symbol.AverageFifteenMinutes >= Symbol.AverageOneHour &&
                Symbol.AverageOneHour >= Symbol.AverageFourHour &&
                Symbol.AverageFourHour >= Symbol.AverageEightHour &&
                Symbol.AverageEightHour >= Symbol.AverageTwelveHour &&
                Symbol.AverageTwelveHour >= Symbol.AverageOneDay)
            {
                Symbol.IsPositive = true;
            }

        }

        #region - Candles -
        public decimal? Candles(KlineInterval interval, int limit, DateTime? startTime = null, DateTime? endTime = null)
        {
            try
            {
                var result = _client.UsdFuturesApi.ExchangeData.GetKlinesAsync(symbol: Symbol.Name, interval: interval, startTime: startTime, endTime: endTime, limit: limit).Result;
                if (!result.Success) WriteLog("Error Candles");
                else
                {
                    decimal sum = 0m;
                    int count = 0;
                    foreach (var it in result.Data.ToList())
                    {
                        sum += it.LowPrice;
                        sum += it.HighPrice;
                        count += 2;
                    }
                    return sum / count;
                }
            }
            catch (Exception e)
            {
                WriteLog($"Candles {e.Message}");
            }
            return null;
        }

        #endregion
        private void WriteLog(string text)
        {
            try
            {
                File.AppendAllText(_pathLog + Symbol.Name, $"{DateTime.Now} {text}\n");
            }
            catch { }
        }
    }
}
