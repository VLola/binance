using Binance.Net.Clients;
using Binance.Net.Enums;
using Binance.Net.Interfaces;
using Binance.Net.Objects.Models.Futures;
using Newtonsoft.Json;
using Project_07.Models;
using System;
using System.IO;
using System.Windows.Media;

namespace Project_07.ViewModels
{
    public class SymbolViewModel
    {
        private string _pathHistory = $"{Directory.GetCurrentDirectory()}/history/";
        private string _pathLog = $"{Directory.GetCurrentDirectory()}/log/";
        public BinanceClient? Client { get; set; }
        public BinanceSocketClient? SocketClient { get; set; }
        public SymbolModel SymbolModel { get; set; } = new(); 
        public SymbolViewModel(BinanceClient? client, BinanceSocketClient? socketClient, BinanceFuturesUsdtSymbol symbol)
        {
            SymbolModel.Name = symbol.Name;
            Client = client;
            SocketClient = socketClient;
            SymbolModel.PropertyChanged += SymbolModel_PropertyChanged;
            StartKlineAsync();
        }

        private void SymbolModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if(e.PropertyName == "Price")
            {
                CheckOpenOrder();
                CheckCloseOrder();
            }
        }
        private void CheckOpenOrder()
        {
            if (!SymbolModel.IsOpenShort)
            {
                if (SymbolModel.Price < SymbolModel.PriceShort && SymbolModel.PriceShort > 0m)
                {
                    if (SymbolModel.Bets.Count == 0 || SymbolModel.Bets[0].KlineOpenTime != SymbolModel.OpenTime)
                    {
                        decimal open = SymbolModel.PriceShort;
                        SymbolModel.TakeProfitShort = open - (open * 0.005m);
                        SymbolModel.StopLossShort = open + (open * 0.005m);
                        SymbolModel.Bets.Insert(0, new Bet()
                        {
                            Name = SymbolModel.Name,
                            IsLong = false,
                            OpenPrice = SymbolModel.Price,
                            OpenTime = DateTime.UtcNow,
                            KlineOpenTime = SymbolModel.OpenTime
                        });
                        SymbolModel.IsOpenShort = true;
                    }
                }
            }
            if (!SymbolModel.IsOpenLong)
            {
                if (SymbolModel.Price > SymbolModel.PriceLong && SymbolModel.PriceLong > 0m)
                {
                    if(SymbolModel.Bets.Count == 0 || SymbolModel.Bets[0].KlineOpenTime != SymbolModel.OpenTime)
                    {
                        decimal open = SymbolModel.PriceLong;
                        SymbolModel.TakeProfitLong = open + (open * 0.005m);
                        SymbolModel.StopLossLong = open - (open * 0.005m);
                        SymbolModel.Bets.Insert(0, new Bet()
                        {
                            Name = SymbolModel.Name,
                            IsLong = true,
                            OpenPrice = SymbolModel.Price,
                            OpenTime = DateTime.UtcNow,
                            KlineOpenTime = SymbolModel.OpenTime
                        });
                        SymbolModel.IsOpenLong = true;
                    }
                }
            }
        }
        private void CheckCloseOrder()
        {
            if (SymbolModel.IsOpenShort)
            {
                if (SymbolModel.Price < SymbolModel.TakeProfitShort)
                {
                    SymbolModel.Bets[0].IsPositive = true;
                    SymbolModel.Bets[0].ClosePrice = SymbolModel.Price;
                    SymbolModel.Bets[0].CloseTime = DateTime.UtcNow;
                    WriteHistory();
                    SymbolModel.Total += 0.5;
                    SymbolModel.ShortPlus += 1;
                    SymbolModel.IsOpenShort = false;
                }
                else if (SymbolModel.Price > SymbolModel.StopLossShort)
                {
                    SymbolModel.Bets[0].IsPositive = false;
                    SymbolModel.Bets[0].ClosePrice = SymbolModel.Price;
                    SymbolModel.Bets[0].CloseTime = DateTime.UtcNow;
                    WriteHistory();
                    SymbolModel.Total -= 0.5;
                    SymbolModel.ShortMinus += 1;
                    SymbolModel.IsOpenShort = false;
                }
            }
            if (SymbolModel.IsOpenLong)
            {
                if (SymbolModel.Price > SymbolModel.TakeProfitLong)
                {
                    SymbolModel.Bets[0].IsPositive = true;
                    SymbolModel.Bets[0].ClosePrice = SymbolModel.Price;
                    SymbolModel.Bets[0].CloseTime = DateTime.UtcNow;
                    WriteHistory();
                    SymbolModel.Total += 0.5;
                    SymbolModel.LongPlus += 1;
                    SymbolModel.IsOpenLong = false;
                }
                else if (SymbolModel.Price < SymbolModel.StopLossLong)
                {
                    SymbolModel.Bets[0].IsPositive = false;
                    SymbolModel.Bets[0].ClosePrice = SymbolModel.Price;
                    SymbolModel.Bets[0].CloseTime = DateTime.UtcNow;
                    WriteHistory();
                    SymbolModel.Total -= 0.5;
                    SymbolModel.LongMinus += 1;
                    SymbolModel.IsOpenLong = false;
                }
            }
        }

        public async void StartKlineAsync()
        {
            try
            {
                var result = await SocketClient.UsdFuturesStreams.SubscribeToKlineUpdatesAsync(SymbolModel.Name, KlineInterval.OneMinute, Message =>
                {
                    SymbolModel.Price = Message.Data.Data.ClosePrice;
                    if (Message.Data.Data.OpenTime == SymbolModel.OpenTime) UpdateKline(Message.Data.Data);
                    else NewKline(Message.Data.Data);
                });
                if (!result.Success)
                {
                    WriteLog($"Failed StartKlineAsync: {result.Error?.Message}");
                }
            }
            catch (Exception eX)
            {
                WriteLog($"StartKlineAsync {eX.Message}");
            }
        }
        private void UpdateKline(IBinanceKline binanceKline)
        {

        }
        private void NewKline(IBinanceKline binanceKline)
        {
            SymbolModel.OpenTime = binanceKline.OpenTime;
            decimal open = binanceKline.OpenPrice;
            SymbolModel.PriceShort = open - (open * 0.01m);
            SymbolModel.PriceLong = open + (open * 0.01m);
        }
        private void WriteHistory()
        {
            try
            {
                File.WriteAllText(_pathHistory + SymbolModel.Name, JsonConvert.SerializeObject(SymbolModel.Bets));
            }
            catch { }
        }
        private void WriteLog(string text)
        {
            try
            {
                File.AppendAllText(_pathLog + SymbolModel.Name, $"{DateTime.Now} {text}\n");
            }
            catch { }
        }
    }
}
