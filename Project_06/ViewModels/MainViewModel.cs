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
using System.Threading.Tasks;
using System.Windows;

namespace Project_06.ViewModels
{
    public class MainViewModel
    {
        
        public FinancePlot financePlot { get; set; }
        private string _pathLog = $"{Directory.GetCurrentDirectory()}/log/";
        public MainModel MainModel { get; set; }
        private BinanceClient? _client { get; set; }
        private RelayCommand? _startCommand;
        public RelayCommand StartCommand
        {
            get { return _startCommand ?? (_startCommand = new RelayCommand(obj => { Start(); })); }
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
                MainModel.MyPlot.Configuration.LeftClickDragPan = true;
                MainModel.MyPlot.Configuration.RightClickDragZoom = false;
                MainModel.MyPlot.Configuration.Pan = false;
                MainModel.MyPlot.Plot.RenderUnlock();
            }));
            GetSumbolName();
        }
        private void Start()
        {
            List<IBinanceKline> binanceKlines = Klines(MainModel.SelectedSymbol.Name, KlineInterval.FifteenMinutes, 450);
            List<OHLC> oHLCs = binanceKlines.Select(item=>new OHLC(
                    open: Decimal.ToDouble(item.OpenPrice),
                    high: Decimal.ToDouble(item.HighPrice),
                    low: Decimal.ToDouble(item.LowPrice),
                    close: Decimal.ToDouble(item.ClosePrice),
                    timeStart: item.OpenTime,
                    timeSpan: TimeSpan.FromMinutes(1),
                    volume: Decimal.ToDouble(item.Volume)
                )).ToList();
            foreach (var item in oHLCs)
            {
                if(item.Close > item.Open)
                {
                    MainModel.SelectedSymbol.Plus += 1;
                    MainModel.SelectedSymbol.PlusPercent += Math.Round((item.Close - item.Open) / item.Open * 10000);
                }
                else if (item.Close < item.Open)
                {
                    MainModel.SelectedSymbol.Minus += 1;
                    MainModel.SelectedSymbol.MinusPercent += Math.Round((item.Open - item.Close) / item.Close * 10000);
                }
            }
            MainModel.MyPlot.Dispatcher.Invoke(new Action(() =>
            {
                MainModel.MyPlot.Plot.RenderLock();

                MainModel.MyPlot.Plot.Remove(financePlot);
                financePlot = MainModel.MyPlot.Plot.AddCandlesticks(oHLCs.ToArray());

                MainModel.MyPlot.Plot.RenderUnlock();
                MainModel.MyPlot.Refresh();
            }));
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
        private void GetSumbolName()
        {
            List<string> list = ListSymbols();
            list.Sort();
            foreach (var it in list)
            {
                MainModel.SymbolsName.Add(it);
                MainModel.Symbols.Add(new SymbolModel() { Name = it });
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
