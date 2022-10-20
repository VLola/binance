using Binance.Net.Clients;
using Binance.Net.Enums;
using Binance.Net.Interfaces;
using Binance.Net.Interfaces.Clients;
using Binance.Net.Objects.Models.Spot;
using CryptoExchange.Net.CommonObjects;
using CryptoExchange.Net.Sockets;
using Newtonsoft.Json;
using Project_04.Command;
using Project_04.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Threading;

namespace Project_04.ViewModels
{
    internal class SymbolViewModel
    {
        public SymbolModel Symbol { get; set; }
        private BinanceSocketClient _socketClient { get; set; }
        private BinanceClient _client { get; set; }
        private string _pathLog = $"{Directory.GetCurrentDirectory()}/log/";
        private RelayCommand? _calculateCommand;
        public RelayCommand CalculateCommand
        {
            get { return _calculateCommand ?? (_calculateCommand = new RelayCommand(obj => { ShowInfo(); })); }
        }
        public SymbolViewModel(BinanceClient client, BinanceSocketClient socketClient, string symbolName)
        {
            Symbol = new();
            _client = client;
            _socketClient = socketClient;
            Symbol.Name = symbolName;
        }

        DateTime dateTime = DateTime.Now;
        BetModel betModel = new();
        List<IBinanceKline> binanceKlines = new();
        public void ShowInfo()
        {
            Reload();

            for (int i = 0; i < 10; i++) {
                binanceKlines.InsertRange(0, Klines(KlineInterval.OneMinute, 480, endTime: dateTime));
                dateTime = dateTime.AddMinutes(-480);
            }
            while (binanceKlines.Count > 2880)
            {
                OpenOrder();
            }
        }
        private void OpenOrder()
        {
            int count = 0;
            decimal fifteenMinutesMax = 0m;
            decimal oneHourMax = 0m;
            decimal fourHourMax = 0m;
            decimal eightHourMax = 0m;
            decimal twelveHourMax = 0m;
            decimal oneDayMax = 0m;

            decimal fifteenMinutesMin = 1000000m;
            decimal oneHourMin = 1000000m;
            decimal fourHourMin = 1000000m;
            decimal eightHourMin = 1000000m;
            decimal twelveHourMin = 1000000m;
            decimal oneDayMin = 1000000m;
            decimal openPrice = 0m;
            DateTime openTime = DateTime.Now;
            binanceKlines.ForEach(kline =>
            {
                if (count > 1425 && count < 1440)
                {
                    if (fifteenMinutesMin > kline.LowPrice) fifteenMinutesMin = kline.LowPrice;
                    if (fifteenMinutesMax < kline.HighPrice) fifteenMinutesMax = kline.HighPrice;
                }
                if (count > 1380 && count < 1440)
                {
                    if (oneHourMin > kline.LowPrice) oneHourMin = kline.LowPrice;
                    if (oneHourMax < kline.HighPrice) oneHourMax = kline.HighPrice;
                }
                if (count > 1200 && count < 1440)
                {
                    if (fourHourMin > kline.LowPrice) fourHourMin = kline.LowPrice;
                    if (fourHourMax < kline.HighPrice) fourHourMax = kline.HighPrice;
                }
                if (count > 960 && count < 1440)
                {
                    if (eightHourMin > kline.LowPrice) eightHourMin = kline.LowPrice;
                    if (eightHourMax < kline.HighPrice) eightHourMax = kline.HighPrice;
                }
                if (count > 720 && count < 1440)
                {
                    if (twelveHourMin > kline.LowPrice) twelveHourMin = kline.LowPrice;
                    if (twelveHourMax < kline.HighPrice) twelveHourMax = kline.HighPrice;
                }
                if (count < 1440)
                {
                    if (oneDayMin > kline.LowPrice) oneDayMin = kline.LowPrice;
                    if (oneDayMax < kline.HighPrice) oneDayMax = kline.HighPrice;
                }
                if (count == 1440)
                {
                    openPrice = kline.OpenPrice;
                    openTime = kline.OpenTime;
                }
                count++;
            });

            Symbol.AverageFifteenMinutes = Math.Round((fifteenMinutesMax + fifteenMinutesMin) / 2, 9);

            Symbol.AverageOneHour = Math.Round((oneHourMax + oneHourMin) / 2, 9);

            Symbol.AverageFourHour = Math.Round((fourHourMax + fourHourMin) / 2, 9);

            Symbol.AverageEightHour = Math.Round((eightHourMax + eightHourMin) / 2, 9);

            Symbol.AverageTwelveHour = Math.Round((twelveHourMax + twelveHourMin) / 2, 9);

            Symbol.AverageOneDay = Math.Round((oneDayMax + oneDayMin) / 2, 9);

            if (Symbol.AverageFifteenMinutes < Symbol.AverageOneHour &&
                Symbol.AverageOneHour < Symbol.AverageFourHour &&
                Symbol.AverageFourHour < Symbol.AverageEightHour &&
                Symbol.AverageEightHour < Symbol.AverageTwelveHour &&
                Symbol.AverageTwelveHour < Symbol.AverageOneDay)
            {
                Symbol.IsOpenOrder = true;
                Symbol.Position = "Short";
            }
            else if (Symbol.AverageFifteenMinutes > Symbol.AverageOneHour &&
                Symbol.AverageOneHour > Symbol.AverageFourHour &&
                Symbol.AverageFourHour > Symbol.AverageEightHour &&
                Symbol.AverageEightHour > Symbol.AverageTwelveHour &&
                Symbol.AverageTwelveHour > Symbol.AverageOneDay)
            {
                Symbol.IsOpenOrder = true;
                Symbol.Position = "Long";
            }
            else
            {
                binanceKlines.RemoveAt(0);
            }

            if (Symbol.IsOpenOrder)
            {
                TimeSpan timeSpan = CloseOrder(openPrice, openTime);

                if(timeSpan == TimeSpan.FromMinutes(-1))
                {
                    binanceKlines.Clear();
                }
                else
                {
                    int minutes = (int)timeSpan.TotalMinutes + 1;
                    binanceKlines.RemoveRange(0, minutes);
                }
            }
        }
        private TimeSpan CloseOrder(decimal openPrice, DateTime openTime)
        {
            TimeSpan timeSpan = TimeSpan.FromMinutes(-1);
            bool checkCloseOrder = false;
            int count = 0;
            foreach (var item in binanceKlines)
            {
                if (count > 1440)
                {
                    if ((openPrice - (openPrice * Symbol.TakeProfit)) > item.LowPrice && (openPrice + (openPrice * Symbol.TakeProfit)) < item.HighPrice)
                    {
                        checkCloseOrder = true;
                        Symbol.IsShortTP = true;
                        Symbol.IsLongTP = true;
                        betModel.Status = "Indefinite";
                        betModel.OpenPrice = $"{openPrice}";
                        betModel.OpenTime = $"{openTime}";
                        betModel.ClosePrice = "NoPrice";
                        betModel.CloseTime = $"{item.OpenTime}";
                        timeSpan = item.OpenTime.Subtract(openTime);
                        WriteHistory();
                        break;
                    }
                    else if ((openPrice - (openPrice * Symbol.TakeProfit)) > item.LowPrice)
                    {
                        checkCloseOrder = true;
                        Symbol.IsShortTP = true;
                        betModel.OpenPrice = $"{openPrice}";
                        betModel.OpenTime = $"{openTime}";
                        betModel.ClosePrice = $"{openPrice - (openPrice * Symbol.TakeProfit)}";
                        betModel.CloseTime = $"{item.OpenTime}";
                        timeSpan = item.OpenTime.Subtract(openTime);
                        if (Symbol.Position == "Long")
                        {
                            betModel.Status = "Lose";
                            WriteHistory();
                        }
                        else
                        {
                            betModel.Status = "Win";
                            WriteHistory();
                        }
                        break;
                    }
                    else if ((openPrice + (openPrice * Symbol.TakeProfit)) < item.HighPrice)
                    {
                        checkCloseOrder = true;
                        Symbol.IsLongTP = true;
                        betModel.OpenPrice = $"{openPrice}";
                        betModel.OpenTime = $"{openTime}";
                        betModel.ClosePrice = $"{openPrice + (openPrice * Symbol.TakeProfit)}";
                        betModel.CloseTime = $"{item.OpenTime}";
                        timeSpan = item.OpenTime.Subtract(openTime);
                        if (Symbol.Position == "Long")
                        {
                            betModel.Status = "Win";
                            WriteHistory();
                        }
                        else
                        {
                            betModel.Status = "Lose";
                            WriteHistory();
                        }
                        break;
                    }
                }
                count++;
            }
            if (!checkCloseOrder)
            {
                betModel.Status = "Indefinite";
                betModel.OpenPrice = $"{openPrice}";
                betModel.OpenTime = $"{openTime}";
                betModel.ClosePrice = "NoPrice";
                betModel.CloseTime = "NoTime";
                WriteHistory();
            }
            return timeSpan;
        }
        public void WriteHistory()
        {
            WriteLog(JsonConvert.SerializeObject(betModel));
        }
        public List<IBinanceKline> Klines(KlineInterval interval, int limit, DateTime? startTime = null, DateTime? endTime = null)
        {
            try
            {
                var result = _client.UsdFuturesApi.ExchangeData.GetKlinesAsync(symbol: Symbol.Name, interval: interval, startTime: startTime, endTime: endTime, limit: limit).Result;
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
        private void Reload()
        {
            Symbol.IsOpenOrder = false;
            Symbol.IsLongTP = false;
            Symbol.IsShortTP = false;
            Symbol.Position = "";
            betModel.Status = "";
            betModel.OpenPrice = "";
            betModel.OpenTime = "";
            betModel.ClosePrice = "";
            betModel.CloseTime = "";
        }

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
