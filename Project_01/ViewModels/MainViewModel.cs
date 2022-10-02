using Binance.Net.Clients;
using Binance.Net.Enums;
using Binance.Net.Interfaces;
using Project_01.Command;
using Project_01.Models;
using Project_01.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Project_01.ViewModels
{
    internal class MainViewModel
    {
        public ChartModel ChartModel { get; set; } = new();
        MainView _mainView { get; set; }
        public static BinanceClient client = new();
        public MainModel MainModel { get; set; } = new();
        private RelayCommand? _selectCommand;
        public RelayCommand SelectCommand
        {
            get { return _selectCommand ?? (_selectCommand = new RelayCommand(obj => { Select(); })); }
        }
        public MainViewModel(MainView mainView)
        {
            _mainView = mainView;
            Loading();
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
                        MainModel.Symbols.Add(it);
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
                    _mainView.Dispatcher.Invoke(new Action(() =>
                    {
                        if (ChartModel.Candles.Count > 0) ChartModel.Candles.Clear();
                    }));

                    DateTime endTime = DateTime.Now;
                    DateTime startTime = endTime.AddMinutes(-360);
                    List<IBinanceKline> list = new();
                    for (int i = 0; i < (MainModel.Count * 4); i++)
                    {
                        var result = client.UsdFuturesApi.ExchangeData.GetKlinesAsync(symbol: MainModel.SelectedSymbol, interval: KlineInterval.OneMinute, startTime: startTime, endTime: endTime).Result;
                        if (result.Success)
                        {
                            List<IBinanceKline> temp = result.Data.ToList();
                            temp.Reverse();
                            foreach (var item in temp)
                            {
                                list.Add(item);
                            }
                            endTime = endTime.AddMinutes(-360);
                            startTime = startTime.AddMinutes(-360);
                        }
                    }


                    AddCandles(list);
                    Run();
                    decimal maxPrice = 0m;
                    foreach (var item in ChartModel.Candles)
                    {
                        if ((item.LowPrice + item.HighPrice) > maxPrice) maxPrice = item.LowPrice + item.HighPrice;
                    }

                    _mainView.Dispatcher.Invoke(new Action(() =>
                    {
                        ChartModel.ScaleY = Decimal.ToDouble(maxPrice / 10000);
                        ChartModel.Width = ChartModel.Candles.Count * 7;
                        ChartModel.Height = Decimal.ToDouble(maxPrice / 80);
                        ChartModel.Margin = new Thickness(0, ChartModel.Height * 11, 0, 0);
                    }));
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
            decimal minLowPrice = 1000000m;
            foreach (var it in list)
            {
                if (maxHighPrice < it.HighPrice) maxHighPrice = it.HighPrice;
                if (minLowPrice > it.LowPrice) minLowPrice = it.LowPrice;
            }
            foreach (var it in list)
            {
                _mainView.Dispatcher.Invoke(new Action(() =>
                {
                    ChartModel.Candles.Insert(0, new(maxHighPrice, minLowPrice, openTime, it.CloseTime, it.OpenPrice, it.ClosePrice, it.LowPrice, it.HighPrice));
                }));
                openTime += 7;
            }

        }
        private async void Run()
        {
            await Task.Run(() =>
            {
                _mainView.Dispatcher.Invoke(new Action(() => {

                    List<DateTime> times = new();
                    for (int i = 0; i < ChartModel.Candles.Count - 10; i++)
                    {

                        double openTime = 0;
                        BindingModel bindingModel = new();
                        for (int j = 0; j < ChartModel.Candles.Count - 10; j++)
                        {
                            if (i != j && i != j + 1 && i != j + 2 && i != j - 1 && i != j - 2 && ChartModel.Candles[i].ClosePrice != 0m && ChartModel.Candles[i + 1].ClosePrice != 0m && ChartModel.Candles[i + 2].ClosePrice != 0m && ChartModel.Candles[j].ClosePrice != 0m && ChartModel.Candles[j + 1].ClosePrice != 0m && ChartModel.Candles[j + 2].ClosePrice != 0m)
                            {
                                if (ChartModel.Candles[i].IsPositive == ChartModel.Candles[j].IsPositive && ChartModel.Candles[i + 1].IsPositive == ChartModel.Candles[j + 1].IsPositive && ChartModel.Candles[i + 2].IsPositive == ChartModel.Candles[j + 2].IsPositive)
                                {

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

                                        if (bindingModel.ChartModel.Candles.Count == 0 && !times.Contains(ChartModel.Candles[i].CloseTime))
                                        {
                                            times.Add(ChartModel.Candles[i].CloseTime);
                                            decimal[] prices = { ChartModel.Candles[i].OpenPrice, ChartModel.Candles[i + 1].OpenPrice, ChartModel.Candles[i + 2].OpenPrice, ChartModel.Candles[i + 3].OpenPrice, ChartModel.Candles[i + 4].OpenPrice, ChartModel.Candles[i + 5].OpenPrice, ChartModel.Candles[i + 6].OpenPrice, ChartModel.Candles[i + 7].OpenPrice, ChartModel.Candles[i + 8].OpenPrice, ChartModel.Candles[i + 9].OpenPrice };
                                            bindingModel.ChartModel.Candles.Add(new CandleModel(ChartModel.Candles[i + 9], openTime, prices.Min(), false));
                                            openTime += 7;
                                            bindingModel.ChartModel.Candles.Add(new CandleModel(ChartModel.Candles[i + 8], openTime, prices.Min(), false));
                                            openTime += 7;
                                            bindingModel.ChartModel.Candles.Add(new CandleModel(ChartModel.Candles[i + 7], openTime, prices.Min(), false));
                                            openTime += 7;
                                            bindingModel.ChartModel.Candles.Add(new CandleModel(ChartModel.Candles[i + 6], openTime, prices.Min(), false));
                                            openTime += 7;
                                            bindingModel.ChartModel.Candles.Add(new CandleModel(ChartModel.Candles[i + 5], openTime, prices.Min(), false));
                                            openTime += 7;
                                            bindingModel.ChartModel.Candles.Add(new CandleModel(ChartModel.Candles[i + 4], openTime, prices.Min(), false));
                                            openTime += 7;
                                            bindingModel.ChartModel.Candles.Add(new CandleModel(ChartModel.Candles[i + 3], openTime, prices.Min(), false));
                                            openTime += 7;
                                            bindingModel.ChartModel.Candles.Add(new CandleModel(ChartModel.Candles[i + 2], openTime, prices.Min(), true));
                                            openTime += 7;
                                            bindingModel.ChartModel.Candles.Add(new CandleModel(ChartModel.Candles[i + 1], openTime, prices.Min(), true));
                                            openTime += 7;
                                            bindingModel.ChartModel.Candles.Add(new CandleModel(ChartModel.Candles[i], openTime, prices.Min(), true));
                                            openTime += 30;
                                        }
                                        if (!times.Contains(ChartModel.Candles[j].CloseTime))
                                        {
                                            times.Add(ChartModel.Candles[j].CloseTime);
                                            decimal[] openPrices = { ChartModel.Candles[j].OpenPrice, ChartModel.Candles[j + 1].OpenPrice, ChartModel.Candles[j + 2].OpenPrice, ChartModel.Candles[j + 3].OpenPrice, ChartModel.Candles[j + 4].OpenPrice, ChartModel.Candles[j + 5].OpenPrice, ChartModel.Candles[j + 6].OpenPrice, ChartModel.Candles[j + 7].OpenPrice, ChartModel.Candles[j + 8].OpenPrice, ChartModel.Candles[j + 9].OpenPrice };
                                            bindingModel.ChartModel.Candles.Add(new CandleModel(ChartModel.Candles[j + 9], openTime, openPrices.Min(), false));
                                            openTime += 7;
                                            bindingModel.ChartModel.Candles.Add(new CandleModel(ChartModel.Candles[j + 8], openTime, openPrices.Min(), false));
                                            openTime += 7;
                                            bindingModel.ChartModel.Candles.Add(new CandleModel(ChartModel.Candles[j + 7], openTime, openPrices.Min(), false));
                                            openTime += 7;
                                            bindingModel.ChartModel.Candles.Add(new CandleModel(ChartModel.Candles[j + 6], openTime, openPrices.Min(), false));
                                            openTime += 7;
                                            bindingModel.ChartModel.Candles.Add(new CandleModel(ChartModel.Candles[j + 5], openTime, openPrices.Min(), false));
                                            openTime += 7;
                                            bindingModel.ChartModel.Candles.Add(new CandleModel(ChartModel.Candles[j + 4], openTime, openPrices.Min(), false));
                                            openTime += 7;
                                            bindingModel.ChartModel.Candles.Add(new CandleModel(ChartModel.Candles[j + 3], openTime, openPrices.Min(), false));
                                            openTime += 7;
                                            bindingModel.ChartModel.Candles.Add(new CandleModel(ChartModel.Candles[j + 2], openTime, openPrices.Min(), true));
                                            openTime += 7;
                                            bindingModel.ChartModel.Candles.Add(new CandleModel(ChartModel.Candles[j + 1], openTime, openPrices.Min(), true));
                                            openTime += 7;
                                            bindingModel.ChartModel.Candles.Add(new CandleModel(ChartModel.Candles[j], openTime, openPrices.Min(), true));
                                            openTime += 30;
                                        }
                                    }
                                }
                            }
                        }
                        _mainView.Dispatcher.Invoke(new Action(() =>
                        {
                            if (bindingModel.ChartModel.Candles.Count > 0) {
                                MainModel.Charts.Add(bindingModel);
                            }
                        }));
                    }
                }));
            });
        }
    }
}
