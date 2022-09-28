using Binance.Net.Clients;
using Binance.Net.Enums;
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
            await Task.Run(async () =>
            {
                try
                {
                    var result = client.UsdFuturesApi.ExchangeData.GetKlinesAsync(symbol: ChartModel.SelectedSymbol, interval: KlineInterval.OneMinute, limit: 499).Result;
                    if (result.Success)
                    {
                        decimal maxHighPrice = 0m;
                        int openTime = 0;
                        var list = result.Data.ToList();
                        list.Reverse();
                        foreach (var it in list)
                        {
                            if (maxHighPrice < it.HighPrice) maxHighPrice = it.HighPrice;
                        }
                        foreach (var it in list)
                        {
                            _chartView.Dispatcher.Invoke(new Action(() => { 
                                ChartModel.Candles.Insert(0, new(maxHighPrice, openTime, it.OpenPrice, it.ClosePrice, it.LowPrice, it.HighPrice));
                            }));
                            openTime += 7;
                        }
                    }
                }

                catch (Exception e)
                {

                }
            });
        }
    }
}
