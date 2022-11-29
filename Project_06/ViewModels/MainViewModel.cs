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
using System.Collections.ObjectModel;
using System.Drawing;
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
        public ScatterPlot scatterPlotIndicatorLong { get; set; }
        public ScatterPlot scatterPlotIndicatorShort { get; set; }
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
                    //SaveAllSymbol();
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

        private void MainModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "SelectedSymbol")
            {
                MainModel.MyPlot.Dispatcher.Invoke(new Action(() =>
                {
                    MainModel.MyPlot.Plot.RenderLock();

                    MainModel.MyPlot.Plot.Remove(financePlot);
                    MainModel.MyPlot.Plot.Remove(scatterPlot);
                    financePlot = MainModel.MyPlot.Plot.AddCandlesticks(MainModel.SelectedSymbol.oHLCs.ToArray());
                    if (MainModel.SelectedSymbol.Algorithms.AlgorithmOne.x.Count > 0)
                    {
                        scatterPlot = MainModel.MyPlot.Plot.AddScatter(MainModel.SelectedSymbol.Algorithms.AlgorithmOne.x.ToArray(), MainModel.SelectedSymbol.Algorithms.AlgorithmOne.y.ToArray(), lineWidth: 0);
                    }
                    MainModel.MyPlot.Plot.RenderUnlock();
                    MainModel.MyPlot.Refresh();
                }));

                MainModel.MyPlotLine.Dispatcher.Invoke(new Action(() =>
                {
                    MainModel.MyPlotLine.Plot.RenderLock();

                    MainModel.MyPlotLine.Plot.Remove(scatterPlotIndicatorLong);
                    MainModel.MyPlotLine.Plot.Remove(scatterPlotIndicatorShort);
                    scatterPlotIndicatorLong = MainModel.MyPlotLine.Plot.AddScatter(MainModel.SelectedSymbol.Algorithms.AlgorithmOne.xIndicatorLong.ToArray(), MainModel.SelectedSymbol.Algorithms.AlgorithmOne.yIndicatorLong.ToArray(), color: Color.Green, lineWidth: 0);
                    
                    scatterPlotIndicatorShort = MainModel.MyPlotLine.Plot.AddScatter(MainModel.SelectedSymbol.Algorithms.AlgorithmOne.xIndicatorShort.ToArray(), MainModel.SelectedSymbol.Algorithms.AlgorithmOne.yIndicatorShort.ToArray(), color: Color.Red, lineWidth: 0);

                    MainModel.MyPlotLine.Plot.RenderUnlock();
                    MainModel.MyPlotLine.Refresh();
                }));
            }
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
        private async void SaveSymbol(string symbol, DateTime dateTime)
        {
            try
            {
                await Task.Run(async () =>
                {
                    List<IBinanceKline> binanceKlines = new();
                    for (int i = 0; i < 36; i++)
                    {
                        List<IBinanceKline>? list = Klines(symbol, KlineInterval.FifteenMinutes, 480, endTime: dateTime);
                        if (list == null) break;
                        else
                        {
                            binanceKlines.InsertRange(0, list);
                            dateTime = dateTime.AddMinutes(-480 * 15);
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
                foreach (var item in MainModel.Symbols)
                {
                    GetSymbol(item);
                }

            });
        }
        private async void GetSymbol(SymbolModel symbolModel)
        {
            await Task.Run(() =>
            {
                StartNewAlgo(symbolModel);

                symbolModel.Plus = symbolModel.Algorithms.AlgorithmOne.Plus;
                symbolModel.Minus = symbolModel.Algorithms.AlgorithmOne.Minus;
                symbolModel.PlusPercent = symbolModel.Algorithms.AlgorithmOne.PlusPercent;
                symbolModel.MinusPercent = symbolModel.Algorithms.AlgorithmOne.MinusPercent;

                MainModel.Plus += symbolModel.Plus;
                MainModel.PlusPercent += symbolModel.PlusPercent;
                MainModel.Minus += symbolModel.Minus;
                MainModel.MinusPercent += symbolModel.MinusPercent;

            });
        }
        private void StartNewAlgo(SymbolModel symbolModel)
        {
            string json = File.ReadAllText(_pathKlines + symbolModel.Name);
            List<BinanceSpotKline> binanceKlines = JsonConvert.DeserializeObject<List<BinanceSpotKline>>(json);
            //List<IBinanceKline> binanceKlines = Klines(symbolModel.Name, KlineInterval.FifteenMinutes, 400);

            symbolModel.oHLCs = binanceKlines.Select(item => new OHLC(
                    open: Decimal.ToDouble(item.OpenPrice),
                    high: Decimal.ToDouble(item.HighPrice),
                    low: Decimal.ToDouble(item.LowPrice),
                    close: Decimal.ToDouble(item.ClosePrice),
                    timeStart: item.OpenTime,
                    timeSpan: TimeSpan.FromMinutes(15),
                    volume: Decimal.ToDouble(item.Volume)
                )).ToList();

            symbolModel.Algorithms.CalculateAlgorithmOne(symbolModel);

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
