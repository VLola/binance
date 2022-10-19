using Binance.Net.Clients;
using Binance.Net.Objects.Models.Spot;
using CryptoExchange.Net.CommonObjects;
using CryptoExchange.Net.Sockets;
using Newtonsoft.Json;
using Project_02.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace Project_02.ViewModels
{
    internal class SymbolViewModel
    {
        public DispatcherTimer timer = new DispatcherTimer();
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
            timer.Interval = TimeSpan.FromMinutes(1);
            timer.Tick += Timer_Tick;
            timer.Start();
            SubscribeToAggregatedTradeUpdatesAsync();
        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            if(Symbol.Price > 0m)
            {

                if (Symbol.Select)
                {
                    Models.Order order = new Models.Order();
                    order.OpenPrice = Symbol.Price;
                    order.OpenTime = Symbol.Time;
                    order.IsClose = false;
                    order.PositionSide = "Long";
                    order.Price = (Symbol.Price + (Symbol.Price * 0.005m));

                    Models.Order order1 = new Models.Order();
                    order1.OpenPrice = Symbol.Price;
                    order1.OpenTime = Symbol.Time;
                    order1.IsClose = false;
                    order1.PositionSide = "Short";
                    order1.Price = (Symbol.Price - (Symbol.Price * 0.005m));

                    Symbol.Orders.Add(order);
                    Symbol.Orders.Add(order1);
                }
            }
        }

        private void Symbol_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Select")
            {
                //if (Symbol.Select) SubscribeToAggregatedTradeUpdatesAsync();
                //else UnsubscribeAsync();
                if (!Symbol.Select)
                {
                    if (Symbol.Orders.Count > 0) Symbol.Orders.ForEach(order =>
                    {
                        if (!order.IsClose)
                        {
                            order.IsClose = true;
                            order.ClosePrice = Symbol.Price;
                            order.CloseTime = Symbol.Time;
                            if (order.PositionSide == "Short") order.Profit = (order.OpenPrice - order.ClosePrice) / order.ClosePrice;
                            else order.Profit = (order.ClosePrice - order.OpenPrice) / order.OpenPrice;
                            Symbol.OrderCount += 1;
                            Symbol.AverageOrderProfit = Symbol.Orders.Sum(order => order.Profit) / Symbol.OrderCount;
                        }
                    });
                }
            }
            //else if (e.PropertyName == "Delta")
            //{
            //    if (!Symbol.IsOpenOrder && Symbol.Delta > Symbol.MaxDelta)
            //    {
            //        Symbol.IsOpenOrder = true;
            //        Models.Order order = new Models.Order();
            //        order.OpenPrice = Symbol.Price;
            //        order.OpenTime = Symbol.Time;
            //        order.IsClose = false;
            //        order.Delta = Symbol.MaxDelta;
            //        if (Symbol.Price < Symbol.AveragePrice) order.PositionSide = "Short";
            //        else order.PositionSide = "Long";
            //        Symbol.Orders.Add(order);
            //        Symbol.MaxDelta += 1m;
            //    }
            //}
            //else if (e.PropertyName == "Time")
            //{
            //    Symbol.Orders.ForEach(order =>
            //    {
            //        if (!order.IsClose)
            //        {
            //            if(order.OpenTime < Symbol.Time.AddMinutes(-1))
            //            {
            //                order.IsClose = true;
            //                order.ClosePrice = Symbol.Price;
            //                order.CloseTime = Symbol.Time;
            //                if(order.PositionSide == "Short") order.Profit = (order.OpenPrice - order.ClosePrice) / order.ClosePrice;
            //                else order.Profit = (order.ClosePrice - order.OpenPrice) / order.OpenPrice;
            //                //WriteLog($"Profit all - {Symbol.Orders.Sum(order=> order.Profit)} - {JsonConvert.SerializeObject(order)}");
            //                //Symbol.IsOpenOrder = false;
            //            }
            //        }
            //    });
            //}
        }
        private async void SubscribeToAggregatedTradeUpdatesAsync()
        {
            try
            {
                var result = await _socketClient.UsdFuturesStreams.SubscribeToAggregatedTradeUpdatesAsync(Symbol.Name, Message =>
                {
                    Symbol.Price = Message.Data.Price;
                    Symbol.Time = Message.Data.TradeTime;
                    Symbol.AddPrice(new Price() { Value = Message.Data.Price, Time = Message.Data.TradeTime });

                    if (Symbol.Orders.Count > 0) Symbol.Orders.ForEach(order =>
                    {
                        if (!order.IsClose)
                        {
                            if (order.PositionSide == "Short")
                            {
                                if (Symbol.Price < order.Price)
                                {
                                    order.IsClose = true;
                                    order.ClosePrice = Symbol.Price;
                                    order.CloseTime = Symbol.Time;
                                    order.Profit = (order.OpenPrice - order.ClosePrice) / order.ClosePrice;
                                    Symbol.OrderCount += 1;
                                    Symbol.AverageOrderProfit = Symbol.Orders.Sum(order => order.Profit) / Symbol.OrderCount;
                                }
                            }
                            else
                            {
                                if (Symbol.Price > order.Price)
                                {
                                    order.IsClose = true;
                                    order.ClosePrice = Symbol.Price;
                                    order.CloseTime = Symbol.Time;
                                    order.Profit = (order.ClosePrice - order.OpenPrice) / order.OpenPrice;
                                    Symbol.OrderCount += 1;
                                    Symbol.AverageOrderProfit = Symbol.Orders.Sum(order => order.Profit) / Symbol.OrderCount;
                                }
                            }
                        }
                    });
                    //if (Symbol.Run)
                    //{
                    //    // My algorithm
                    //}
                    //else
                    //{

                    //}
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
