﻿using Binance.Net.Clients;
using Binance.Net.Enums;
using Binance.Net.Interfaces;
using Binance.Net.Interfaces.Clients;
using Binance.Net.Objects.Models.Spot;
using CryptoExchange.Net.CommonObjects;
using CryptoExchange.Net.Sockets;
using Newtonsoft.Json;
using Project_03.Command;
using Project_03.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Threading;

namespace Project_03.ViewModels
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
        public void ShowInfo()
        {
            Reload();
            List<IBinanceKline> binanceKlines = new();
            
            binanceKlines.AddRange(Klines(KlineInterval.OneMinute, 480, endTime: dateTime));
            dateTime = dateTime.AddMinutes(-480);
            binanceKlines.InsertRange(0, Klines(KlineInterval.OneMinute, 480, endTime: dateTime));
            dateTime = dateTime.AddMinutes(-480);
            binanceKlines.InsertRange(0, Klines(KlineInterval.OneMinute, 480, endTime: dateTime));
            dateTime = dateTime.AddMinutes(-480);
            binanceKlines.InsertRange(0, Klines(KlineInterval.OneMinute, 480, endTime: dateTime));
            int count = 0;
            List<string> list = new();

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
                list.Add(kline.OpenTime.ToString());
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
                Symbol.IsPositive = true;
                Symbol.Position = "Short";
            }
            else if (Symbol.AverageFifteenMinutes > Symbol.AverageOneHour &&
                Symbol.AverageOneHour > Symbol.AverageFourHour &&
                Symbol.AverageFourHour > Symbol.AverageEightHour &&
                Symbol.AverageEightHour > Symbol.AverageTwelveHour &&
                Symbol.AverageTwelveHour > Symbol.AverageOneDay)
            {
                Symbol.IsPositive = true;
                Symbol.Position = "Long";
            }

            if(Symbol.Position == "Long" || Symbol.Position == "Short")
            {
                bool checkOpenOrder = false;
                count = 0;
                foreach (var item in binanceKlines)
                {
                    if (count > 1440)
                    {
                        if ((openPrice - (openPrice * Symbol.TakeProfit)) > item.LowPrice && (openPrice + (openPrice * Symbol.TakeProfit)) < item.HighPrice)
                        {
                            checkOpenOrder = true;
                            Symbol.IsShortTP = true;
                            Symbol.IsLongTP = true;
                            betModel.Status = "Indefinite";
                            betModel.OpenPrice = $"{openPrice}";
                            betModel.OpenTime = $"{openTime}";
                            betModel.ClosePrice = "NoPrice";
                            betModel.CloseTime = $"{item.OpenTime}";
                            //WriteLog($"Indefinite  OpenPrice:{openPrice} OpenTime:{openTime} ClosePrice:NoPrice CloseTime:{item.OpenTime}");
                            break;
                        }
                        else if ((openPrice - (openPrice * Symbol.TakeProfit)) > item.LowPrice)
                        {
                            checkOpenOrder = true;
                            Symbol.IsShortTP = true;
                            betModel.OpenPrice = $"{openPrice}";
                            betModel.OpenTime = $"{openTime}";
                            betModel.ClosePrice = $"{openPrice - (openPrice * Symbol.TakeProfit)}";
                            betModel.CloseTime = $"{item.OpenTime}";
                            if (Symbol.Position == "Long")
                            {
                                betModel.Status = "Lose";
                                //WriteLog($"Lose OpenPrice:{openPrice} OpenTime:{openTime} ClosePrice:{openPrice - (openPrice * Symbol.TakeProfit)} CloseTime:{item.OpenTime}");
                            }
                            else
                            {
                                betModel.Status = "Win";
                                //WriteLog($"Win  OpenPrice:{openPrice} OpenTime:{openTime} ClosePrice:{openPrice - (openPrice * Symbol.TakeProfit)} CloseTime:{item.OpenTime}");
                            }
                            break;
                        }
                        else if ((openPrice + (openPrice * Symbol.TakeProfit)) < item.HighPrice)
                        {
                            checkOpenOrder = true;
                            Symbol.IsLongTP = true;
                            betModel.OpenPrice = $"{openPrice}";
                            betModel.OpenTime = $"{openTime}";
                            betModel.ClosePrice = $"{openPrice + (openPrice * Symbol.TakeProfit)}";
                            betModel.CloseTime = $"{item.OpenTime}";
                            if (Symbol.Position == "Long")
                            {
                                betModel.Status = "Win";
                                //WriteLog($"Win  OpenPrice:{openPrice} OpenTime:{openTime} ClosePrice:{openPrice + (openPrice * Symbol.TakeProfit)} CloseTime:{item.OpenTime}");
                            }
                            else
                            {
                                betModel.Status = "Lose";
                                //WriteLog($"Lose  OpenPrice:{openPrice} OpenTime:{openTime} ClosePrice:{openPrice + (openPrice * Symbol.TakeProfit)} CloseTime:{item.OpenTime}");
                            }
                            break;
                        }
                    }
                    count++;
                }
                if (!checkOpenOrder)
                {
                    betModel.Status = "Indefinite";
                    betModel.OpenPrice = $"{openPrice}";
                    betModel.OpenTime = $"{openTime}";
                    betModel.ClosePrice = "NoPrice";
                    betModel.CloseTime = "NoTime";
                    //WriteLog($"Indefinite  OpenPrice:{openPrice} OpenTime:{openTime} ClosePrice:NoPrice CloseTime:NoTime");
                }
            }
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
            Symbol.IsPositive = false;
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
