using Binance.Net.Clients;
using Binance.Net.Enums;
using Binance.Net.Interfaces;
using Binance.Net.Interfaces.Clients.UsdFuturesApi;
using CryptoExchange.Net.CommonObjects;
using Project_01.Command;
using Project_01.Models;
using Project_01.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Net.Sockets;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Threading;

namespace Project_01.ViewModels
{
    public class ChartViewModel
    {
        public ChartModel ChartModel { get; set; } = new();
        public static BinanceClient client = new();
        private RelayCommand? _selectCommand;
        private ChartView _chartView;
        public ChartViewModel(ChartView chartView)
        {
            _chartView = chartView;
            Loading();
        }
        public RelayCommand SelectCommand
        {
            get { return _selectCommand ?? (_selectCommand = new RelayCommand(obj => { Select(); })); }
        }
        public void Loading()
        {
            try
            {
                var result = client.UsdFuturesApi.ExchangeData.GetPricesAsync().Result;
                if (result.Success)
                {
                    List<string> sort = new List<string>();
                    foreach (var it in result.Data.ToList())
                    {
                        sort.Add(it.Symbol);
                    }
                    sort.Sort();
                    foreach (var it in sort)
                    {
                        ChartModel.Symbols.Add(it);
                    }
                }
            }
            catch (Exception e)
            {

            }
        }
        private async void Select()
        {
            await Task.Run(() =>
            {
                try
                {
                    _chartView.Dispatcher.Invoke(new Action(() => {
                        if(ChartModel.Candles.Count > 0) ChartModel.Candles.Clear();
                    }));

                    DateTime endTime = DateTime.Now;
                    DateTime startTime = endTime.AddMinutes(-300);
                    List<IBinanceKline> list = new();
                    for (int i = 0; i < 5; i++)
                    {
                        var result = client.UsdFuturesApi.ExchangeData.GetKlinesAsync(symbol: ChartModel.SelectedSymbol, interval: KlineInterval.OneMinute, startTime: startTime, endTime: endTime).Result;
                        if (result.Success)
                        {
                            List<IBinanceKline> temp = result.Data.ToList();
                            temp.Reverse();
                            foreach (var item in temp)
                            {
                                list.Add(item);
                            }
                            endTime = endTime.AddMinutes(-300);
                            startTime = startTime.AddMinutes(-300);
                        }
                    }
                    

                    AddCandles(list);
                    Run();
                }

                catch (Exception e)
                {

                }
            });
        }
        private void AddCandles(List<IBinanceKline> list)
        {
            int openTime = 0;
            decimal maxHighPrice = 0m;
            foreach (var it in list)
            {
                if (maxHighPrice < it.HighPrice) maxHighPrice = it.HighPrice;
            }
            foreach (var it in list)
            {
                _chartView.Dispatcher.Invoke(new Action(() => {
                    ChartModel.Candles.Insert(0, new(maxHighPrice, openTime, it.CloseTime, it.OpenPrice, it.ClosePrice, it.LowPrice, it.HighPrice));
                }));
                openTime += 7;
            }

        }
        private async void Run()
        {
            await Task.Run(() =>
            {
                _chartView.Dispatcher.Invoke(new Action(() => {
                    for (int i = 0; i < ChartModel.Candles.Count - 3; i++)
                    {
                        for (int j = 0; j < ChartModel.Candles.Count - 3; j++)
                        {
                            if(i != j && i != j + 1 && i != j + 2 && i != j - 1 && i != j - 2 && ChartModel.Candles[i].ClosePrice != 0m && ChartModel.Candles[i + 1].ClosePrice != 0m && ChartModel.Candles[i + 2].ClosePrice != 0m && ChartModel.Candles[j].ClosePrice != 0m && ChartModel.Candles[j + 1].ClosePrice != 0m && ChartModel.Candles[j + 2].ClosePrice != 0m)
                            {
                                if (ChartModel.Candles[i].IsPositive == ChartModel.Candles[j].IsPositive && ChartModel.Candles[i + 1].IsPositive == ChartModel.Candles[j + 1].IsPositive && ChartModel.Candles[i + 2].IsPositive == ChartModel.Candles[j + 2].IsPositive) {

                                    decimal one1 = ChartModel.Candles[i].ClosePrice / ChartModel.Candles[i + 1].ClosePrice;
                                    decimal one2 = ChartModel.Candles[i].ClosePrice / ChartModel.Candles[i + 2].ClosePrice;
                                    decimal two1 = ChartModel.Candles[j].ClosePrice / ChartModel.Candles[j + 1].ClosePrice;
                                    decimal two2 = ChartModel.Candles[j].ClosePrice / ChartModel.Candles[j + 2].ClosePrice;
                                    decimal openOne1 = (ChartModel.Candles[i].OpenPrice - ChartModel.Candles[i + 1].OpenPrice) / ChartModel.Candles[i].ClosePrice;
                                    decimal openOne2 = (ChartModel.Candles[i].OpenPrice - ChartModel.Candles[i + 2].OpenPrice) / ChartModel.Candles[i].ClosePrice;
                                    decimal openTwo1 = (ChartModel.Candles[j].OpenPrice - ChartModel.Candles[j + 1].OpenPrice) / ChartModel.Candles[j].ClosePrice;
                                    decimal openTwo2 = (ChartModel.Candles[j].OpenPrice - ChartModel.Candles[j + 2].OpenPrice) / ChartModel.Candles[j].ClosePrice;


                                    if ((one1 + (one1 * 0.1m)) > two1 && (one1 - (one1 * 0.1m)) < two1 && (one2 + (one2 * 0.1m)) > two2 && (one2 - (one2 * 0.1m)) < two2 && (openOne1 + (openOne1 * 0.1m)) > openTwo1 && (openOne1 - (openOne1 * 0.1m)) < openTwo1 && (openOne2 + (openOne2 * 0.1m)) > openTwo2 && (openOne2 - (openOne2 * 0.1m)) < openTwo2)
                                    {
                                        ChartModel.Candles[i].Color = "White";
                                        ChartModel.Candles[i + 1].Color = "White";
                                        ChartModel.Candles[i + 2].Color = "White";
                                        ChartModel.Candles[j].Color = "White";
                                        ChartModel.Candles[j + 1].Color = "White";
                                        ChartModel.Candles[j + 2].Color = "White";
                                    }
                                }
                            }
                        }
                    }
                }));
            });
        } 
    }
}
