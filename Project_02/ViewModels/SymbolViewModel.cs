using Binance.Net.Clients;
using CryptoExchange.Net.Sockets;
using Project_02.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_02.ViewModels
{
    internal class SymbolViewModel
    {
        private UpdateSubscription? _updateSubscription { get; set; }
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
            if (e.PropertyName == "Select")
            {
                if (Symbol.Select) SubscribeToAggregatedTradeUpdatesAsync();
                else UnsubscribeAsync();
            }
        }
        private async void SubscribeToAggregatedTradeUpdatesAsync()
        {
            try
            {
                var result = await _socketClient.UsdFuturesStreams.SubscribeToAggregatedTradeUpdatesAsync(Symbol.Name, Message =>
                {
                    Symbol.Price = Message.Data.Price;
                    if (Symbol.Run)
                    {
                        // My algorithm
                    }
                    else
                    {

                    }
                });
                if (!result.Success) WriteLog($"Failed Success SubscribeToAggregatedTradeUpdatesAsync: {result.Error?.Message}");
                else
                {
                    _updateSubscription = result.Data;
                }
            }
            catch (Exception ex)
            {
                WriteLog($"Failed SubscribeToAggregatedTradeUpdatesAsync: {ex.Message}");
            }
        }
        private async void UnsubscribeAsync()
        {
            try
            {
                if (_updateSubscription != null)
                {
                    await _socketClient.UnsubscribeAsync(_updateSubscription);
                    Wait();
                }
            }
            catch (Exception ex)
            {
                WriteLog($"Failed UnsubscribeAsync: {ex.Message}");
            }
        }
        private void Wait()
        {
            Symbol.Price = 0m;
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
