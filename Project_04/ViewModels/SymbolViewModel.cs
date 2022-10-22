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
using static System.Net.Mime.MediaTypeNames;

namespace Project_04.ViewModels
{
    internal class SymbolViewModel
    {
        public SymbolModel Symbol { get; set; }
        private BinanceSocketClient _socketClient { get; set; }
        private BinanceClient _client { get; set; }
        private string _pathLog = $"{Directory.GetCurrentDirectory()}/log/";
        private string _pathHistory = $"{Directory.GetCurrentDirectory()}/history/";
        private string _pathStatistics = $"{Directory.GetCurrentDirectory()}/statistics/";
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

        BetModel betModel = new();
        List<BinanceSpotKline> binanceKlines = new();
        int _minutes = 2880;
        public void ShowInfo()
        {
            if (Symbol.IsSelect)
            {
                //Symbol.StartTime = new DateTime(2022, 9, 20, 0, 0, 0);
                //Symbol.EndTime = new DateTime(2022, 9, 22, 0, 0, 0);
                //for (int i = 0; i < 25; i++)
                //{
                //    ReloadSymbol();

                //    //for (int i = 0; i < 90; i++) {
                //    //    binanceKlines.InsertRange(0, Klines(KlineInterval.OneMinute, 480, endTime: dateTime));
                //    //    dateTime = dateTime.AddMinutes(-480);
                //    //}
                //    binanceKlines.Clear();
                //    string json = File.ReadAllText(_pathHistory + Symbol.Name);
                //    foreach (var kline in JsonConvert.DeserializeObject<List<BinanceSpotKline>>(json))
                //    {
                //        if (kline.OpenTime > Symbol.StartTime && kline.OpenTime < Symbol.EndTime) binanceKlines.Add(kline);
                //    }
                //    while (binanceKlines.Count > 1440)
                //    {
                //        ReloadBet();
                //        OpenOrder();
                //    }
                //    File.AppendAllText(_pathStatistics + Symbol.Name, Symbol.PercentWin.ToString() + "\n");
                //    Symbol.StartTime += TimeSpan.FromHours(24);
                //    Symbol.EndTime += TimeSpan.FromHours(24);
                //}
                ReloadSymbol();

                //for (int i = 0; i < 90; i++) {
                //    binanceKlines.InsertRange(0, Klines(KlineInterval.OneMinute, 480, endTime: dateTime));
                //    dateTime = dateTime.AddMinutes(-480);
                //}
                binanceKlines.Clear();
                string json = File.ReadAllText(_pathHistory + Symbol.Name);
                foreach (var kline in JsonConvert.DeserializeObject<List<BinanceSpotKline>>(json))
                {
                    if (kline.OpenTime > Symbol.StartTime && kline.OpenTime < Symbol.EndTime) binanceKlines.Add(kline);
                }
                while (binanceKlines.Count > _minutes)
                {
                    ReloadBet();
                    OpenOrder();
                }
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
            decimal twoDayMax = 0m;

            decimal fifteenMinutesMin = 1000000m;
            decimal oneHourMin = 1000000m;
            decimal fourHourMin = 1000000m;
            decimal eightHourMin = 1000000m;
            decimal twelveHourMin = 1000000m;
            decimal oneDayMin = 1000000m;
            decimal twoDayMin = 1000000m;
            decimal openPrice = 0m;
            DateTime openTime = DateTime.Now;
            binanceKlines.ForEach(kline =>
            {
                if (count > (_minutes - 15) && count < _minutes)
                {
                    if (fifteenMinutesMin > kline.LowPrice) fifteenMinutesMin = kline.LowPrice;
                    if (fifteenMinutesMax < kline.HighPrice) fifteenMinutesMax = kline.HighPrice;
                }
                if (count > (_minutes - 60) && count < _minutes)
                {
                    if (oneHourMin > kline.LowPrice) oneHourMin = kline.LowPrice;
                    if (oneHourMax < kline.HighPrice) oneHourMax = kline.HighPrice;
                }
                if (count > (_minutes - 240) && count < _minutes)
                {
                    if (fourHourMin > kline.LowPrice) fourHourMin = kline.LowPrice;
                    if (fourHourMax < kline.HighPrice) fourHourMax = kline.HighPrice;
                }
                if (count > (_minutes - 480) && count < _minutes)
                {
                    if (eightHourMin > kline.LowPrice) eightHourMin = kline.LowPrice;
                    if (eightHourMax < kline.HighPrice) eightHourMax = kline.HighPrice;
                }
                if (count > (_minutes - 720) && count < _minutes)
                {
                    if (twelveHourMin > kline.LowPrice) twelveHourMin = kline.LowPrice;
                    if (twelveHourMax < kline.HighPrice) twelveHourMax = kline.HighPrice;
                }
                if (count < _minutes - 1440)
                {
                    if (oneDayMin > kline.LowPrice) oneDayMin = kline.LowPrice;
                    if (oneDayMax < kline.HighPrice) oneDayMax = kline.HighPrice;
                }
                if (count < _minutes)
                {
                    if (twoDayMin > kline.LowPrice) twoDayMin = kline.LowPrice;
                    if (twoDayMax < kline.HighPrice) twoDayMax = kline.HighPrice;
                }
                if (count == _minutes)
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

            Symbol.AverageTwoDay = Math.Round((twoDayMax + twoDayMin) / 2, 9);

            if (Symbol.AverageFifteenMinutes < Symbol.AverageOneHour &&
                Symbol.AverageOneHour < Symbol.AverageFourHour &&
                Symbol.AverageFourHour < Symbol.AverageEightHour &&
                Symbol.AverageEightHour < Symbol.AverageTwelveHour &&
                Symbol.AverageTwelveHour < Symbol.AverageOneDay &&
                Symbol.AverageOneDay < Symbol.AverageTwoDay)
            {
                Symbol.IsOpenOrder = true;
                Symbol.Position = "Short";
            }
            else if (Symbol.AverageFifteenMinutes > Symbol.AverageOneHour &&
                Symbol.AverageOneHour > Symbol.AverageFourHour &&
                Symbol.AverageFourHour > Symbol.AverageEightHour &&
                Symbol.AverageEightHour > Symbol.AverageTwelveHour &&
                Symbol.AverageTwelveHour > Symbol.AverageOneDay &&
                Symbol.AverageOneDay > Symbol.AverageTwoDay)
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
                if (count > _minutes)
                {
                    if (Symbol.Position == "Long")
                    {
                        if ((openPrice - (openPrice * Symbol.StopLoss)) > item.LowPrice && (openPrice + (openPrice * Symbol.TakeProfit)) < item.HighPrice)
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
                            Symbol.BetIndefinite += 1;
                            break;
                        }
                        else if ((openPrice - (openPrice * Symbol.StopLoss)) > item.LowPrice)
                        {
                            checkCloseOrder = true;
                            Symbol.IsShortTP = true;
                            betModel.OpenPrice = $"{openPrice}";
                            betModel.OpenTime = $"{openTime}";
                            betModel.ClosePrice = $"{openPrice - (openPrice * Symbol.TakeProfit)}";
                            betModel.CloseTime = $"{item.OpenTime}";
                            timeSpan = item.OpenTime.Subtract(openTime);
                            betModel.Status = "Lose";
                            WriteHistory();
                            Symbol.BetMinus += 1;
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
                            betModel.Status = "Win";
                            WriteHistory();
                            Symbol.BetPlus += 1;
                            break;
                        }
                    }
                    else if (Symbol.Position == "Short")
                    {
                        if ((openPrice - (openPrice * Symbol.TakeProfit)) > item.LowPrice && (openPrice + (openPrice * Symbol.StopLoss)) < item.HighPrice)
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
                            Symbol.BetIndefinite += 1;
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

                            betModel.Status = "Win";
                            WriteHistory();
                            Symbol.BetPlus += 1;
                            break;
                        }
                        else if ((openPrice + (openPrice * Symbol.StopLoss)) < item.HighPrice)
                        {
                            checkCloseOrder = true;
                            Symbol.IsLongTP = true;
                            betModel.OpenPrice = $"{openPrice}";
                            betModel.OpenTime = $"{openTime}";
                            betModel.ClosePrice = $"{openPrice + (openPrice * Symbol.TakeProfit)}";
                            betModel.CloseTime = $"{item.OpenTime}";
                            timeSpan = item.OpenTime.Subtract(openTime);

                            betModel.Status = "Lose";
                            WriteHistory();
                            Symbol.BetMinus += 1;
                            break;
                        }
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
                Symbol.BetIndefinite += 1;
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
        private void ReloadBet()
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
        private void ReloadSymbol()
        {
            Symbol.BetPlus = 0;
            Symbol.BetMinus = 0;
            Symbol.BetIndefinite = 0;
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
