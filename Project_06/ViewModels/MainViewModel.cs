using Binance.Net.Clients;
using Binance.Net.Enums;
using Binance.Net.Interfaces;
using Binance.Net.Objects.Models.Spot;
using CryptoExchange.Net.CommonObjects;
using Newtonsoft.Json;
using Project_06.Command;
using Project_06.Models;
using ScottPlot;
using ScottPlot.Plottable;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;

namespace Project_06.ViewModels
{
    public class MainViewModel
    {
        
        public FinancePlot financePlot { get; set; }
        public ScatterPlot scatterPlot { get; set; }
        public ScatterPlot scatterPlotClose { get; set; }

        private string _pathLog = $"{Directory.GetCurrentDirectory()}/log/";
        private string _pathKlines = $"{Directory.GetCurrentDirectory()}/klines/";
        public MainModel MainModel { get; set; }
        private BinanceClient? _client { get; set; }
        private RelayCommand? _startCommand;
        public RelayCommand StartCommand
        {
            get
            {
                return _startCommand ?? (_startCommand = new RelayCommand(obj => {
                    //StartNewAlgo(MainModel.SelectedSymbol);
                    GetAllSymbol();
                }));
            }
        }
        private RelayCommand? _saveKlinesCommand;
        public RelayCommand SaveKlinesCommand
        {
            get
            {
                return _saveKlinesCommand ?? (_saveKlinesCommand = new RelayCommand(obj => {
                    SaveAllSymbol();
                }));
            }
        }
        public MainViewModel()
        {
            if (!Directory.Exists(_pathLog)) Directory.CreateDirectory(_pathLog);
            if (!Directory.Exists(_pathKlines)) Directory.CreateDirectory(_pathKlines);
            _client = new();
            MainModel = new();
            MainModel.PropertyChanged += MainModel_PropertyChanged;
            MainModel.MyPlot.Dispatcher.Invoke(new Action(() =>
            {
                MainModel.MyPlot.Plot.RenderLock();
                MainModel.MyPlot.Plot.Style(ScottPlot.Style.Gray2);
                //MainModel.MyPlot.Plot.XAxis.TickLabelFormat("HH:mm:ss", dateTimeFormat: true);
                MainModel.MyPlot.Plot.XAxis.Hide();
                MainModel.MyPlot.Plot.XAxis2.Hide();
                MainModel.MyPlot.Plot.YAxis.Hide();
                MainModel.MyPlot.Plot.YAxis2.Hide();
                MainModel.MyPlot.Configuration.LeftClickDragPan = true;
                MainModel.MyPlot.Configuration.RightClickDragZoom = false;
                MainModel.MyPlot.Configuration.Pan = false;
                MainModel.MyPlot.Plot.RenderUnlock();
            })); 
            MainModel.MyPlotLine.Dispatcher.Invoke(new Action(() =>
            {
                MainModel.MyPlotLine.Plot.RenderLock();
                MainModel.MyPlotLine.Plot.Style(ScottPlot.Style.Gray2);
                //MainModel.MyPlotLine.Plot.XAxis.TickLabelFormat("HH:mm:ss", dateTimeFormat: true);
                MainModel.MyPlotLine.Plot.XAxis.Hide();
                MainModel.MyPlotLine.Plot.XAxis2.Hide();
                MainModel.MyPlotLine.Plot.YAxis.Hide();
                MainModel.MyPlotLine.Plot.YAxis2.Hide();
                MainModel.MyPlotLine.Configuration.LeftClickDragPan = true;
                MainModel.MyPlotLine.Configuration.RightClickDragZoom = false;
                MainModel.MyPlotLine.Configuration.Pan = false;
                MainModel.MyPlotLine.Plot.RenderUnlock();
            }));
            GetSumbolName();
        }

        private void MainModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "SelectedSymbol")
            {
                LoadKlinesChart();
            }
            else if (e.PropertyName == "SelectedStatistics")
            {
                MainModel.Number = MainModel.SelectedStatistics.Number;
            }
            else if (e.PropertyName == "Number")
            {
                LoadResults();
                LoadStrategyChart();
                LoadKlinesChart();
            }
        }
        private void LoadResults()
        {
            MainModel.Plus = 0;
            MainModel.PlusPercent = 0;
            MainModel.Minus = 0;
            MainModel.MinusPercent = 0;

            foreach (var symbolModel in MainModel.Symbols)
            {
                symbolModel.Plus = symbolModel.Algorithms.ListAlgorithms[MainModel.Number].Plus;
                symbolModel.Minus = symbolModel.Algorithms.ListAlgorithms[MainModel.Number].Minus;
                symbolModel.PlusPercent = symbolModel.Algorithms.ListAlgorithms[MainModel.Number].PlusPercent;
                symbolModel.MinusPercent = symbolModel.Algorithms.ListAlgorithms[MainModel.Number].MinusPercent;

                MainModel.Plus += symbolModel.Plus;
                MainModel.PlusPercent += symbolModel.PlusPercent;
                MainModel.Minus += symbolModel.Minus;
                MainModel.MinusPercent += symbolModel.MinusPercent;
            }
        }
        private void LoadKlinesChart()
        {
            try
            {
                AlgorithmModel algorithmModel = MainModel.SelectedSymbol.Algorithms.ListAlgorithms[MainModel.Number];
                MainModel.MyPlot.Dispatcher.Invoke(new Action(() =>
                {
                    MainModel.MyPlot.Plot.RenderLock();

                    MainModel.MyPlot.Plot.Remove(financePlot);
                    MainModel.MyPlot.Plot.Remove(scatterPlot);
                    MainModel.MyPlot.Plot.Remove(scatterPlotClose);
                    financePlot = MainModel.MyPlot.Plot.AddCandlesticks(MainModel.SelectedSymbol.oHLCs.ToArray());
                    if (algorithmModel.x.Count > 0)
                    {
                        scatterPlot = MainModel.MyPlot.Plot.AddScatter(algorithmModel.x.ToArray(), algorithmModel.y.ToArray(), color: Color.Orange, lineWidth: 0, markerSize: 7);
                        scatterPlotClose = MainModel.MyPlot.Plot.AddScatter(algorithmModel.xClose.ToArray(), algorithmModel.yClose.ToArray(), color: Color.SkyBlue, lineWidth: 0, markerSize: 7);
                    }
                    MainModel.MyPlot.Plot.RenderUnlock();
                    MainModel.MyPlot.Refresh();
                }));
            }
            catch
            {

            }
           

        }
        private void LoadStrategyChart()
        {
            List<PointModel> list = new();
            foreach (var item in MainModel.Symbols)
            {
                list.AddRange(item.Algorithms.ListAlgorithms[MainModel.Number].Points);
            }
            list.Sort((x, y) => x.Time.CompareTo(y.Time));
            MainModel.MyPlotLine.Dispatcher.Invoke(new Action(() =>
            {
                MainModel.MyPlotLine.Plot.RenderLock();
                MainModel.MyPlotLine.Plot.Clear();

                MainModel.MyPlotLine.Plot.AddPoint(x: MainModel.SelectedSymbol.oHLCs[0].DateTime.ToOADate(), y: 0, color: Color.Orange);
                MainModel.MyPlotLine.Plot.AddPoint(x: MainModel.SelectedSymbol.oHLCs[MainModel.SelectedSymbol.oHLCs.Count - 1].DateTime.ToOADate(), y: 0, color: Color.Orange);

                double i = 0;
                foreach (var item in list)
                {
                    if(item.IsPositive)
                    {
                        i += item.Percent;
                        Color color;
                        if(item.IsLong) color = Color.Green;
                        else color = Color.Red;

                        MainModel.MyPlotLine.Plot.AddPoint(x: item.Time, y: i, color: color);
                    }
                    else
                    {
                        i -= item.Percent;
                        Color color;
                        if (item.IsLong) color = Color.Green;
                        else color = Color.Red;

                        MainModel.MyPlotLine.Plot.AddPoint(x: item.Time, y: i, color: color);
                    }
                }
                MainModel.MyPlotLine.Plot.RenderUnlock();
                MainModel.MyPlotLine.Refresh();
            }));
        }
        private void CalculateStatistics()
        {
            App.Current.Dispatcher.Invoke(new Action(() =>
            {
                MainModel.Statistics.Clear();
            }));
            List<StatisticsModel> list = new();
            for (int i = 0; i < MainModel.Symbols[0].Algorithms.ListAlgorithms.Count - 1; i++)
            {
                int plus = 0;
                int minus = 0;
                double plusPercent = 0;
                double minusPercent = 0;
                int open = 0;
                int close = 0;
                foreach (var symbolModel in MainModel.Symbols)
                {
                    symbolModel.Plus = symbolModel.Algorithms.ListAlgorithms[i].Plus;
                    symbolModel.Minus = symbolModel.Algorithms.ListAlgorithms[i].Minus;
                    symbolModel.PlusPercent = symbolModel.Algorithms.ListAlgorithms[i].PlusPercent;
                    symbolModel.MinusPercent = symbolModel.Algorithms.ListAlgorithms[i].MinusPercent;

                    open = symbolModel.Algorithms.ListAlgorithms[i].Open;
                    close = symbolModel.Algorithms.ListAlgorithms[i].Close;
                    plus += symbolModel.Plus;
                    plusPercent += symbolModel.PlusPercent;
                    minus += symbolModel.Minus;
                    minusPercent += symbolModel.MinusPercent;
                }
                StatisticsModel statisticsModel = new();
                statisticsModel.Plus = plus;
                statisticsModel.Minus = minus;
                statisticsModel.PlusPercent = plusPercent;
                statisticsModel.MinusPercent = minusPercent;
                statisticsModel.Win = Math.Round(plusPercent / minusPercent, 2);
                statisticsModel.Number = i;
                statisticsModel.Open = open; 
                statisticsModel.Close = close;
                list.Add(statisticsModel);
            }
            var result = list.OrderByDescending(a => a.Win);

            App.Current.Dispatcher.Invoke(new Action(() =>
            {
                foreach (var item in result)
                {
                    MainModel.Statistics.Add(item);
                }
            }));
        }
        private async void SaveAllSymbol()
        {
            await Task.Run(() =>
            {
                foreach (var item in MainModel.Symbols)
                {
                    SaveSymbol(item.Name, DateTime.UtcNow);
                }

            });
        }
        int Interval = 5;
        int Size = 110;
        private async void SaveSymbol(string symbol, DateTime dateTime)
        {
            try
            {
                await Task.Run(async () =>
                {
                    KlineInterval klineInterval = KlineInterval.OneMinute;
                    if(Interval == 5) klineInterval = KlineInterval.FiveMinutes;
                    else if(Interval == 15) klineInterval = KlineInterval.FifteenMinutes;

                    List<IBinanceKline> binanceKlines = new();
                    for (int i = 0; i < Size; i++)
                    {
                        List<IBinanceKline>? list = Klines(symbol, klineInterval, 480, endTime: dateTime);
                        if (list == null) break;
                        else
                        {
                            binanceKlines.InsertRange(0, list);
                            dateTime = dateTime.AddMinutes(-480 * Interval);
                            ShowLoad();
                            await Task.Delay(10000);
                        }
                    }
                    File.WriteAllText(_pathKlines + symbol, JsonConvert.SerializeObject(binanceKlines));
                });
            }
            catch (Exception ex)
            {
                WriteLog("Error SaveSymbol: " + ex);
            }
        }
        private async void ShowLoad()
        {
            await Task.Run(async () =>
            {
                MainModel.IsLoading = true;
                await Task.Delay(200);
                MainModel.IsLoading = false;
            });
        }
        private async void GetAllSymbol()
        {
            await Task.Run(() =>
            {
                List<Task> tasks = new();
                foreach (var item in MainModel.Symbols)
                {
                    Task task = GetSymbol(item);
                    tasks.Add(task);
                }
                Task.WaitAll(tasks.ToArray()); 
                MainModel.SelectedSymbol = MainModel.Symbols[32];
                CalculateStatistics();

            });
        }
        private async Task GetSymbol(SymbolModel symbolModel)
        {
            await Task.Run(async() =>
            {
                StartNewAlgo(symbolModel);

                symbolModel.Plus = symbolModel.Algorithms.ListAlgorithms[MainModel.Number].Plus;
                symbolModel.Minus = symbolModel.Algorithms.ListAlgorithms[MainModel.Number].Minus;
                symbolModel.PlusPercent = symbolModel.Algorithms.ListAlgorithms[MainModel.Number].PlusPercent;
                symbolModel.MinusPercent = symbolModel.Algorithms.ListAlgorithms[MainModel.Number].MinusPercent;

                MainModel.Plus += symbolModel.Plus;
                MainModel.PlusPercent += symbolModel.PlusPercent;
                MainModel.Minus += symbolModel.Minus;
                MainModel.MinusPercent += symbolModel.MinusPercent;

            });
        }
        private void StartNewAlgo(SymbolModel symbolModel)
        {
            string json = File.ReadAllText(_pathKlines + symbolModel.Name);
            List<BinanceSpotKline>? binanceKlines = JsonConvert.DeserializeObject<List<BinanceSpotKline>>(json);
            //List<IBinanceKline> binanceKlines = Klines(symbolModel.Name, KlineInterval.OneMinute, 400);

            if (binanceKlines != null)
            {
                symbolModel.oHLCs = binanceKlines.Select(item => new OHLC(
                        open: Decimal.ToDouble(item.OpenPrice),
                        high: Decimal.ToDouble(item.HighPrice),
                        low: Decimal.ToDouble(item.LowPrice),
                        close: Decimal.ToDouble(item.ClosePrice),
                        timeStart: item.OpenTime,
                        timeSpan: TimeSpan.FromMinutes(Interval),
                        volume: Decimal.ToDouble(item.Volume)
                    )).ToList();


                //symbolModel.Algorithms.CalculateAlgorithmThree(symbolModel);
                symbolModel.Algorithms.CalculateAlgorithmFour(symbolModel);
            }
        }

        public List<IBinanceKline> Klines(string symbol, KlineInterval interval, int limit)
        {
            try
            {
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
        public List<IBinanceKline>? Klines(string symbol, KlineInterval interval, int limit, DateTime? startTime = null, DateTime? endTime = null)
        {
            try
            {
                var result = _client.UsdFuturesApi.ExchangeData.GetKlinesAsync(symbol: symbol, interval: interval, startTime: startTime, endTime: endTime, limit: limit).Result;
                if (!result.Success) WriteLog("Error Klines");
                else
                {
                    return result.Data.ToList();
                }
            }
            catch (Exception e)
            {
                WriteLog($"Klines {e.Message}");
            }
            return null;
        }
        private void GetSumbolName()
        {
            List<string> list = ListSymbols();
            list.Sort();
            foreach (var it in list)
            {
                if (it.Contains("USDT") && it != "FTTUSDT")
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
