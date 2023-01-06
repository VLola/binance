using Binance.Net.Clients;
using Binance.Net.Enums;
using Binance.Net.Interfaces;
using Binance.Net.Objects.Models.Futures;
using Project_07.Models;
using System;
using System.IO;
using System.Windows.Media;

namespace Project_07.ViewModels
{
    public class SymbolViewModel
    {
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
                if (SymbolModel.Price < SymbolModel.PriceShort)
                {
                    SymbolModel.IsOpenShort = true;
                }
            }
            if (!SymbolModel.IsOpenLong)
            {
                if (SymbolModel.Price > SymbolModel.PriceLong)
                {
                    SymbolModel.IsOpenLong = true;
                }
            }
        }
        private void CheckCloseOrder()
        {
            if (SymbolModel.IsOpenShort)
            {
                if (SymbolModel.Price < SymbolModel.TakeProfitShort)
                {
                    SymbolModel.Total += 0.5;
                    SymbolModel.IsOpenShort = false;
                }
                else if (SymbolModel.Price > SymbolModel.StopLossShort)
                {
                    SymbolModel.Total -= 0.5;
                    SymbolModel.IsOpenShort = false;
                }
            }
            if (SymbolModel.IsOpenLong)
            {
                if (SymbolModel.Price > SymbolModel.TakeProfitLong)
                {
                    SymbolModel.Total += 0.5;
                    SymbolModel.IsOpenLong = false;
                }
                else if (SymbolModel.Price < SymbolModel.StopLossLong)
                {
                    SymbolModel.Total -= 0.5;
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
            decimal open = binanceKline.OpenPrice;
            if (!SymbolModel.IsOpenShort)
            {
                SymbolModel.PriceShort = open - (open * 0.01m);
                SymbolModel.TakeProfitShort = open - (open * 0.015m);
                SymbolModel.StopLossShort = open - (open * 0.005m);
            }
            if (!SymbolModel.IsOpenLong)
            {
                SymbolModel.PriceLong = open + (open * 0.01m);
                SymbolModel.TakeProfitLong = open + (open * 0.015m);
                SymbolModel.StopLossLong = open + (open * 0.005m);
            }
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
