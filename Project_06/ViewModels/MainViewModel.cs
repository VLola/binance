using Binance.Net.Clients;
using Binance.Net.Enums;
using Binance.Net.Interfaces;
using Binance.Net.Objects.Models.Spot;
using CryptoExchange.Net.CommonObjects;
using Project_06.Command;
using Project_06.Models;
using ScottPlot;
using ScottPlot.Plottable;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;

namespace Project_06.ViewModels
{
    public class MainViewModel
    {
        
        public FinancePlot financePlot { get; set; }
        public ScatterPlot scatterPlot { get; set; }
        private string _pathLog = $"{Directory.GetCurrentDirectory()}/log/";
        public MainModel MainModel { get; set; }
        private BinanceClient? _client { get; set; }
        private RelayCommand? _startCommand;
        public RelayCommand StartCommand
        {
            get { return _startCommand ?? (_startCommand = new RelayCommand(obj => { 
                //StartNewAlgo(MainModel.SelectedSymbol);
                GetAllSymbol();
            })); }
        }
        public MainViewModel()
        {
            if (!Directory.Exists(_pathLog)) Directory.CreateDirectory(_pathLog);
            _client = new();
            MainModel = new();

            MainModel.MyPlot.Dispatcher.Invoke(new Action(() =>
            {
                MainModel.MyPlot.Plot.RenderLock();
                MainModel.MyPlot.Plot.Style(ScottPlot.Style.Gray2);
                MainModel.MyPlot.Plot.XAxis.TickLabelFormat("HH:mm:ss", dateTimeFormat: true);
                MainModel.MyPlot.Plot.YAxis.Hide();
                MainModel.MyPlot.Configuration.LeftClickDragPan = true;
                MainModel.MyPlot.Configuration.RightClickDragZoom = false;
                MainModel.MyPlot.Configuration.Pan = false;
                MainModel.MyPlot.Plot.RenderUnlock();
            })); 
            MainModel.MyPlotLine.Dispatcher.Invoke(new Action(() =>
            {
                MainModel.MyPlotLine.Plot.RenderLock();
                MainModel.MyPlotLine.Plot.Style(ScottPlot.Style.Gray2);
                MainModel.MyPlotLine.Plot.XAxis.TickLabelFormat("HH:mm:ss", dateTimeFormat: true);
                MainModel.MyPlotLine.Plot.YAxis.Hide();
                MainModel.MyPlotLine.Configuration.LeftClickDragPan = true;
                MainModel.MyPlotLine.Configuration.RightClickDragZoom = false;
                MainModel.MyPlotLine.Configuration.Pan = false;
                MainModel.MyPlotLine.Plot.RenderUnlock();
            }));
            GetSumbolName();
        }
        private async void GetAllSymbol()
        {
            await Task.Run(() =>
            {
                foreach (var item in MainModel.Symbols)
                {
                    StartNewAlgo(item);
                    MainModel.Plus += item.Plus;
                    MainModel.PlusPercent += item.PlusPercent;
                    MainModel.Minus += item.Minus;
                    MainModel.MinusPercent += item.MinusPercent;
                }

            });
        }
        private void StartNewAlgo(SymbolModel symbolModel)
        {
            List<IBinanceKline> binanceKlines = Klines(symbolModel.Name, KlineInterval.OneMinute, 400);

            List<OHLC> oHLCs = binanceKlines.Select(item => new OHLC(
                    open: Decimal.ToDouble(item.OpenPrice),
                    high: Decimal.ToDouble(item.HighPrice),
                    low: Decimal.ToDouble(item.LowPrice),
                    close: Decimal.ToDouble(item.ClosePrice),
                    timeStart: item.OpenTime,
                    timeSpan: TimeSpan.FromMinutes(1),
                    volume: Decimal.ToDouble(item.Volume)
                )).ToList();

            List<double> x = new();
            List<double> y = new();
            int mul = 10;

            symbolModel.Plus = 0;
            symbolModel.PlusPercent = 0;
            symbolModel.Minus = 0;
            symbolModel.MinusPercent = 0;
            for (int i = 0; i < oHLCs.Count - 3; i++)
            {
                if(i > 30)
                {
                    double sum = 0;
                    for (int j = i; j > (i - 30); j--)
                    {
                        sum += (oHLCs[j].High - oHLCs[j].Low);
                    }
                    double average = (sum / 30);
                    if((oHLCs[i + 1].High - oHLCs[i + 1].Low) > (average * mul))
                    {
                        x.Add(oHLCs[i + 1].DateTime.ToOADate());
                        y.Add(oHLCs[i + 1].Close);

                        if (oHLCs[i + 1].Close < oHLCs[i + 1].Open)
                        {
                            // Short
                            if (oHLCs[i + 1].Close > oHLCs[i + 2].Close)
                            {
                                symbolModel.Plus += 1;
                                symbolModel.PlusPercent += Math.Round((oHLCs[i + 1].Close - oHLCs[i + 2].Close) / oHLCs[i + 2].Close * 10000);
                            }
                            else
                            {
                                symbolModel.Minus += 1;
                                symbolModel.MinusPercent += Math.Round((oHLCs[i + 2].Close - oHLCs[i + 1].Close) / oHLCs[i + 1].Close * 10000);
                            }
                        }
                        else
                        {
                            // Long
                            if (oHLCs[i + 1].Close < oHLCs[i + 2].Close)
                            {
                                symbolModel.Plus += 1;
                                symbolModel.PlusPercent += Math.Round((oHLCs[i + 2].Close - oHLCs[i + 1].Close) / oHLCs[i + 1].Close * 10000);
                            }
                            else
                            {
                                symbolModel.Minus += 1;
                                symbolModel.MinusPercent += Math.Round((oHLCs[i + 1].Close - oHLCs[i + 2].Close) / oHLCs[i + 2].Close * 10000);
                            }
                        }

                    }
                }
            }



            //MainModel.MyPlot.Dispatcher.Invoke(new Action(() =>
            //{
            //    MainModel.MyPlot.Plot.RenderLock();

            //    MainModel.MyPlot.Plot.Remove(financePlot);
            //    MainModel.MyPlot.Plot.Remove(scatterPlot);
            //    financePlot = MainModel.MyPlot.Plot.AddCandlesticks(oHLCs.ToArray());
            //    if (x.Count > 0)
            //    {
            //        scatterPlot = MainModel.MyPlot.Plot.AddScatter(x.ToArray(), y.ToArray(), lineWidth: 0);
            //    }

            //    MainModel.MyPlot.Plot.RenderUnlock();
            //    MainModel.MyPlot.Refresh();
            //}));

        }
        //private void Start()
        //{
        //    List<IBinanceKline> binanceKlines = Klines(MainModel.SelectedSymbol.Name, KlineInterval.FifteenMinutes, 200);

        //    List<IBinanceKline> binanceKlinesNew = new List<IBinanceKline>();

        //    for (int i = 0; i < binanceKlines.Count - 1; i++)
        //    {
        //        if(i > (binanceKlines.Count / 2)) binanceKlinesNew.Add(binanceKlines[i]);
        //    }

        //    List<OHLC> oHLCs = binanceKlinesNew.Select(item=>new OHLC(
        //            open: Decimal.ToDouble(item.OpenPrice),
        //            high: Decimal.ToDouble(item.HighPrice),
        //            low: Decimal.ToDouble(item.LowPrice),
        //            close: Decimal.ToDouble(item.ClosePrice),
        //            timeStart: item.OpenTime,
        //            timeSpan: TimeSpan.FromMinutes(1),
        //            volume: Decimal.ToDouble(item.Volume)
        //        )).ToList();

        //    List<double> x = new();
        //    List<double> y = new();
        //    List<int> jlist = new();
        //    int time = -oHLCs.Count * 15;

        //    for (int i = 0; i < oHLCs.Count - 1; i++)
        //    {
        //        decimal minus = 0m;
        //        decimal plus = 0m;
        //        int plusBet = 0;
        //        int minusBet = 0;
        //        DateTime dateTime = DateTime.Now;
        //        foreach (var item in binanceKlines)
        //        {
        //            if (item.OpenTime <= oHLCs[i].DateTime.AddMinutes(15) && item.OpenTime >= oHLCs[i].DateTime.AddMinutes(time))
        //            {
        //                if (item.ClosePrice > item.OpenPrice)
        //                {
        //                    plusBet++;
        //                    plus += Math.Round((item.ClosePrice - item.OpenPrice) / item.OpenPrice * 10000);
        //                }
        //                else if (item.ClosePrice < item.OpenPrice)
        //                {
        //                    minusBet++;
        //                    minus += Math.Round((item.OpenPrice - item.ClosePrice) / item.ClosePrice * 10000);
        //                }
        //                dateTime = item.OpenTime;
        //            }
        //        }
        //        if (plusBet > minusBet && plus < minus || plusBet < minusBet && plus > minus)
        //        {
        //            decimal result = plus / minus;
        //            y.Add(Decimal.ToDouble(result));
        //            x.Add(dateTime.ToOADate());
        //        }
        //    }

        //    string path = Directory.GetCurrentDirectory() + "/log.txt";
        //    File.WriteAllText(path, JsonSerializer.Serialize(jlist));

        //    foreach (var item in oHLCs)
        //    {
        //        if(item.Close > item.Open)
        //        {
        //            MainModel.SelectedSymbol.Plus += 1;
        //            MainModel.SelectedSymbol.PlusPercent += Math.Round((item.Close - item.Open) / item.Open * 10000);
        //        }
        //        else if (item.Close < item.Open)
        //        {
        //            MainModel.SelectedSymbol.Minus += 1;
        //            MainModel.SelectedSymbol.MinusPercent += Math.Round((item.Open - item.Close) / item.Close * 10000);
        //        }
        //    }
        //    oHLCs.RemoveAt(0);
        //    MainModel.MyPlot.Dispatcher.Invoke(new Action(() =>
        //    {
        //        MainModel.MyPlot.Plot.RenderLock();

        //        MainModel.MyPlot.Plot.Remove(financePlot);
        //        financePlot = MainModel.MyPlot.Plot.AddCandlesticks(oHLCs.ToArray());

        //        MainModel.MyPlot.Plot.RenderUnlock();
        //        MainModel.MyPlot.Refresh();
        //    })); 

        //    MainModel.MyPlotLine.Dispatcher.Invoke(new Action(() =>
        //    {
        //        MainModel.MyPlotLine.Plot.RenderLock();

        //        MainModel.MyPlotLine.Plot.Remove(scatterPlot);
        //        scatterPlot = MainModel.MyPlotLine.Plot.AddScatter(x.ToArray(), y.ToArray());

        //        MainModel.MyPlotLine.Plot.RenderUnlock();
        //        MainModel.MyPlotLine.Refresh();
        //    }));
        //}
        public List<IBinanceKline> Klines(string symbol, KlineInterval interval, int limit)
        {
            try
            {
                //var result = _client.UsdFuturesApi.ExchangeData.GetKlinesAsync(symbol: symbol, interval: interval, limit: limit, startTime: DateTime.UtcNow.AddDays(-30)).Result;
                var result = _client.UsdFuturesApi.ExchangeData.GetKlinesAsync(symbol: symbol, interval: interval, limit: limit).Result;
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
        private void GetSumbolName()
        {
            List<string> list = ListSymbols();
            list.Sort();
            foreach (var it in list)
            {
                if (it.Contains("USDT"))
                {
                    MainModel.SymbolsName.Add(it);
                    MainModel.Symbols.Add(new SymbolModel() { Name = it });
                }
            }
        }
        private List<string> ListSymbols()
        {
            try
            {
                var result = _client.UsdFuturesApi.ExchangeData.GetPricesAsync().Result;
                if (!result.Success) WriteLog($"Failed Success ListSymbols: {result.Error?.Message}");
                return result.Data.Select(item => item.Symbol).ToList();
            }
            catch (Exception ex)
            {
                WriteLog($"Failed ListSymbols: {ex.Message}");
                return null;
            }
        }
        private void WriteLog(string text)
        {
            try
            {
                File.AppendAllText(_pathLog + "_MAIN_LOG", $"{DateTime.Now} {text}\n");
            }
            catch { }
        }
    }
}
