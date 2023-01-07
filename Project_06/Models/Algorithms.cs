using CryptoExchange.Net.CommonObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Documents;

namespace Project_06.Models
{
    public class Algorithms
    {
        public List<AlgorithmModel> ListAlgorithms { get; set; } = new();
        public void CalculateAlgorithmOne(SymbolModel symbolModel)
        {
            AlgorithmModel algorithmModel = new AlgorithmModel();
            int mul = 4;

            bool reverse = false;
            bool minus = false;

            for (int i = 0; i < symbolModel.oHLCs.Count - 3; i++)
            {
                if (i > 30)
                {
                    double sum = 0;
                    for (int j = i; j > (i - 30); j--)
                    {
                        sum += (symbolModel.oHLCs[j].High - symbolModel.oHLCs[j].Low);
                    }
                    double average = (sum / 30);
                    if ((symbolModel.oHLCs[i + 1].High - symbolModel.oHLCs[i + 1].Low) > (average * mul))
                    {
                        algorithmModel.x.Add(symbolModel.oHLCs[i + 1].DateTime.ToOADate());
                        algorithmModel.y.Add(symbolModel.oHLCs[i + 1].Close);

                        //if (oHLCs[i + 1].Close > oHLCs[i - 30].Close)
                        if (!reverse)
                        {
                            if (symbolModel.oHLCs[i + 1].Close < symbolModel.oHLCs[i + 1].Open)
                            {
                                // Short
                                if (symbolModel.oHLCs[i + 1].Close > symbolModel.oHLCs[i + 2].Close)
                                {
                                    minus = false;


                                    algorithmModel.Plus += 1;
                                    algorithmModel.PlusPercent += Math.Round((symbolModel.oHLCs[i + 1].Close - symbolModel.oHLCs[i + 2].Close) / symbolModel.oHLCs[i + 2].Close * 10000);
                                }
                                else
                                {
                                    if (minus)
                                    {
                                        if (reverse) reverse = false;
                                        else reverse = true;
                                    }
                                    minus = true;


                                    algorithmModel.Minus += 1;
                                    algorithmModel.MinusPercent += Math.Round((symbolModel.oHLCs[i + 2].Close - symbolModel.oHLCs[i + 1].Close) / symbolModel.oHLCs[i + 1].Close * 10000);
                                }
                            }
                            else
                            {
                                // Long
                                if (symbolModel.oHLCs[i + 1].Close < symbolModel.oHLCs[i + 2].Close)
                                {
                                    minus = false;


                                    algorithmModel.Plus += 1;
                                    algorithmModel.PlusPercent += Math.Round((symbolModel.oHLCs[i + 2].Close - symbolModel.oHLCs[i + 1].Close) / symbolModel.oHLCs[i + 1].Close * 10000);
                                }
                                else
                                {
                                    if (minus)
                                    {
                                        if (reverse) reverse = false;
                                        else reverse = true;
                                    }
                                    minus = true;


                                    algorithmModel.Minus += 1;
                                    algorithmModel.MinusPercent += Math.Round((symbolModel.oHLCs[i + 1].Close - symbolModel.oHLCs[i + 2].Close) / symbolModel.oHLCs[i + 2].Close * 10000);
                                }
                            }
                        }
                        else
                        {
                            if (symbolModel.oHLCs[i + 1].Close > symbolModel.oHLCs[i + 1].Open)
                            {
                                // Short
                                if (symbolModel.oHLCs[i + 1].Close > symbolModel.oHLCs[i + 2].Close)
                                {

                                    symbolModel.Plus += 1;
                                    symbolModel.PlusPercent += Math.Round((symbolModel.oHLCs[i + 1].Close - symbolModel.oHLCs[i + 2].Close) / symbolModel.oHLCs[i + 2].Close * 10000);
                                }
                                else
                                {

                                    algorithmModel.Minus += 1;
                                    algorithmModel.MinusPercent += Math.Round((symbolModel.oHLCs[i + 2].Close - symbolModel.oHLCs[i + 1].Close) / symbolModel.oHLCs[i + 1].Close * 10000);
                                }
                            }
                            else
                            {
                                // Long
                                if (symbolModel.oHLCs[i + 1].Close < symbolModel.oHLCs[i + 2].Close)
                                {

                                    algorithmModel.Plus += 1;
                                    algorithmModel.PlusPercent += Math.Round((symbolModel.oHLCs[i + 2].Close - symbolModel.oHLCs[i + 1].Close) / symbolModel.oHLCs[i + 1].Close * 10000);
                                }
                                else
                                {

                                    algorithmModel.Minus += 1;
                                    algorithmModel.MinusPercent += Math.Round((symbolModel.oHLCs[i + 1].Close - symbolModel.oHLCs[i + 2].Close) / symbolModel.oHLCs[i + 2].Close * 10000);
                                }
                            }
                        }

                    }
                }
            }

            ListAlgorithms.Add(algorithmModel);
        }
        public void CalculateAlgorithmTwo(SymbolModel symbolModel)
        {
            for (int i = 2; i < 20; i++)
            {
                AlgorithmTwo(symbolModel, i);
            }
            
        }

        private void AlgorithmTwo(SymbolModel symbolModel, int mul)
        {
            AlgorithmModel algorithmModel = new AlgorithmModel();


            for (int i = 0; i < symbolModel.oHLCs.Count - 3; i++)
            {
                if (i > 30)
                {
                    double sum = 0;
                    for (int j = i; j > (i - 30); j--)
                    {
                        sum += (symbolModel.oHLCs[j].High - symbolModel.oHLCs[j].Low);
                    }
                    double average = (sum / 30);
                    if ((symbolModel.oHLCs[i + 1].High - symbolModel.oHLCs[i + 1].Low) > (average * mul))
                    {
                        algorithmModel.x.Add(symbolModel.oHLCs[i + 1].DateTime.ToOADate());
                        algorithmModel.y.Add(symbolModel.oHLCs[i + 1].Close);

                        if (symbolModel.oHLCs[i + 1].Close < symbolModel.oHLCs[i + 1].Open)
                        {
                            // Short
                            if (symbolModel.oHLCs[i + 1].Close > symbolModel.oHLCs[i + 2].Close)
                            {


                                algorithmModel.Plus += 1;
                                algorithmModel.PlusPercent += Math.Round((symbolModel.oHLCs[i + 1].Close - symbolModel.oHLCs[i + 2].Close) / symbolModel.oHLCs[i + 2].Close * 10000);
                            }
                            else
                            {


                                algorithmModel.Minus += 1;
                                algorithmModel.MinusPercent += Math.Round((symbolModel.oHLCs[i + 2].Close - symbolModel.oHLCs[i + 1].Close) / symbolModel.oHLCs[i + 1].Close * 10000);
                            }
                        }
                        else
                        {
                            // Long
                            if (symbolModel.oHLCs[i + 1].Close < symbolModel.oHLCs[i + 2].Close)
                            {


                                algorithmModel.Plus += 1;
                                algorithmModel.PlusPercent += Math.Round((symbolModel.oHLCs[i + 2].Close - symbolModel.oHLCs[i + 1].Close) / symbolModel.oHLCs[i + 1].Close * 10000);
                            }
                            else
                            {


                                algorithmModel.Minus += 1;
                                algorithmModel.MinusPercent += Math.Round((symbolModel.oHLCs[i + 1].Close - symbolModel.oHLCs[i + 2].Close) / symbolModel.oHLCs[i + 2].Close * 10000);
                            }
                        }

                    }
                }
            }

            ListAlgorithms.Add(algorithmModel);
        }

        public void CalculateAlgorithmThree(SymbolModel symbolModel)
        {
            for (int i = 2; i < 20; i++)
            {
                for (int j = 0; j < 30; j++)
                {
                    AlgorithmThree(symbolModel, i, j);
                }
            }
        }
        // Алгоритм без стоп лосса
        private void AlgorithmThree(SymbolModel symbolModel, int mul, int kline)
        {
            AlgorithmModel algorithmModel = new AlgorithmModel();

            algorithmModel.Open = mul;
            algorithmModel.Close = kline;

            for (int i = 0; i < symbolModel.oHLCs.Count - 3 - kline; i++)
            {
                if (i > 30)
                {
                    double sum = 0;
                    for (int j = i; j > (i - 30); j--)
                    {
                        sum += (symbolModel.oHLCs[j].High - symbolModel.oHLCs[j].Low);
                    }
                    double average = (sum / 30);
                    if ((symbolModel.oHLCs[i + 1].High - symbolModel.oHLCs[i + 1].Low) > (average * mul))
                    {

                        if (symbolModel.oHLCs[i + 1].Close > symbolModel.oHLCs[i + 1].Open)
                        {
                            // Short
                            if (symbolModel.oHLCs[i + 1].Close > symbolModel.oHLCs[i + 2 + kline].Close)
                            {
                                algorithmModel.x.Add(symbolModel.oHLCs[i + 1].DateTime.ToOADate());
                                algorithmModel.y.Add(symbolModel.oHLCs[i + 1].Close);

                                double percent = Math.Round((symbolModel.oHLCs[i + 1].Close - symbolModel.oHLCs[i + 2 + kline].Close) / symbolModel.oHLCs[i + 2 + kline].Close * 100, 2);

                                PointModel pointModel = new();
                                pointModel.IsPositive = true;
                                pointModel.IsLong = false;
                                pointModel.Time = symbolModel.oHLCs[i + 1].DateTime.ToOADate();
                                pointModel.Percent = percent;
                                algorithmModel.Points.Add(pointModel);

                                algorithmModel.Plus += 1;
                                algorithmModel.PlusPercent += percent;
                                i = i + kline;
                            }
                            else
                            {
                                algorithmModel.x.Add(symbolModel.oHLCs[i + 1].DateTime.ToOADate());
                                algorithmModel.y.Add(symbolModel.oHLCs[i + 1].Close);

                                double percent = Math.Round((symbolModel.oHLCs[i + 2 + kline].Close - symbolModel.oHLCs[i + 1].Close) / symbolModel.oHLCs[i + 1].Close * 100, 2);

                                PointModel pointModel = new();
                                pointModel.IsPositive = false;
                                pointModel.IsLong = false;
                                pointModel.Time = symbolModel.oHLCs[i + 1].DateTime.ToOADate();
                                pointModel.Percent = percent;
                                algorithmModel.Points.Add(pointModel);

                                algorithmModel.Minus += 1;
                                algorithmModel.MinusPercent += percent;
                                i = i + kline;
                            }
                        }
                        //else
                        //{
                        //    // Long
                        //    if (symbolModel.oHLCs[i + 1].Close < symbolModel.oHLCs[i + 2 + kline].Close)
                        //    {
                        //        algorithmModel.x.Add(symbolModel.oHLCs[i + 1].DateTime.ToOADate());
                        //        algorithmModel.y.Add(symbolModel.oHLCs[i + 1].Close);

                        //        double percent = Math.Round((symbolModel.oHLCs[i + 2 + kline].Close - symbolModel.oHLCs[i + 1].Close) / symbolModel.oHLCs[i + 1].Close * 100, 2);

                        //        PointModel pointModel = new();
                        //        pointModel.IsPositive = true;
                        //        pointModel.IsLong = true;
                        //        pointModel.Time = symbolModel.oHLCs[i + 1].DateTime.ToOADate();
                        //        pointModel.Percent = percent;
                        //        algorithmModel.Points.Add(pointModel);

                        //        algorithmModel.Plus += 1;
                        //        algorithmModel.PlusPercent += percent;
                        //        i = i + kline;
                        //    }
                        //    else
                        //    {
                        //        algorithmModel.x.Add(symbolModel.oHLCs[i + 1].DateTime.ToOADate());
                        //        algorithmModel.y.Add(symbolModel.oHLCs[i + 1].Close);

                        //        double percent = Math.Round((symbolModel.oHLCs[i + 1].Close - symbolModel.oHLCs[i + 2 + kline].Close) / symbolModel.oHLCs[i + 2 + kline].Close * 100, 2);

                        //        PointModel pointModel = new();
                        //        pointModel.IsPositive = false;
                        //        pointModel.IsLong = true;
                        //        pointModel.Time = symbolModel.oHLCs[i + 1].DateTime.ToOADate();
                        //        pointModel.Percent = percent;
                        //        algorithmModel.Points.Add(pointModel);

                        //        algorithmModel.Minus += 1;
                        //        algorithmModel.MinusPercent += percent;
                        //        i = i + kline;
                        //    }
                        //}

                    }
                }
            }

            ListAlgorithms.Add(algorithmModel);
        }
        /// Алгоритм с стоп лосом
        double StopLoss = 2;
        int Interval = 15;
        public void CalculateAlgorithmFour(SymbolModel symbolModel)
        {
            for (int i = 2; i < 20; i++)
            {
                for (int j = 0; j < 30; j++)
                {
                    AlgorithmFour(symbolModel, i, j);
                }
            }
        }
        private void AlgorithmFour(SymbolModel symbolModel, int mul, int kline)
        {
            AlgorithmModel algorithmModel = new AlgorithmModel();

            algorithmModel.Open = mul;
            algorithmModel.Close = kline;

            for (int i = 0; i < symbolModel.oHLCs.Count - 3 - kline; i++)
            {
                if (i > 30)
                {
                    double sum = 0;
                    for (int j = i; j > (i - 30); j--)
                    {
                        sum += (symbolModel.oHLCs[j].High - symbolModel.oHLCs[j].Low);
                    }
                    double average = (sum / 30);
                    //Добавлена ещё 1 проверка
                    
                    if ((symbolModel.oHLCs[i + 1].High - symbolModel.oHLCs[i + 1].Low) > (average * mul))
                    {
                        //if ((symbolModel.oHLCs[i + 1].High - symbolModel.oHLCs[i + 1].Low) > (average * (mul + 1)))
                        //{
                        //    i = i + kline;
                        //    continue;
                        //}
                        if (symbolModel.oHLCs[i + 1].Close > symbolModel.oHLCs[i + 1].Open)
                        {
                            // Short
                            (bool check, int close) = CheckShort(symbolModel, algorithmModel, i + 1, i + 2 + kline);
                            if (check)
                            {
                                algorithmModel.x.Add(symbolModel.oHLCs[i + 1].DateTime.ToOADate());
                                algorithmModel.y.Add(symbolModel.oHLCs[i + 1].Close);

                                algorithmModel.xClose.Add(symbolModel.oHLCs[i + 2 + kline].DateTime.ToOADate());
                                algorithmModel.yClose.Add(symbolModel.oHLCs[i + 2 + kline].Close);


                                double percent = Math.Round((symbolModel.oHLCs[i + 1].Close - symbolModel.oHLCs[i + 2 + kline].Close) / symbolModel.oHLCs[i + 2 + kline].Close * 100, 2);

                                PointModel pointModel = new();
                                pointModel.IsPositive = true;
                                pointModel.IsLong = false;
                                pointModel.Time = symbolModel.oHLCs[i + 1].DateTime.ToOADate();
                                pointModel.Percent = percent;
                                algorithmModel.Points.Add(pointModel);

                                algorithmModel.Plus += 1;
                                algorithmModel.PlusPercent += percent;
                                i = i + kline;
                            }
                            else
                            {
                                algorithmModel.x.Add(symbolModel.oHLCs[i + 1].DateTime.ToOADate());
                                algorithmModel.y.Add(symbolModel.oHLCs[i + 1].Close);

                                double percent = StopLoss;

                                PointModel pointModel = new();
                                pointModel.IsPositive = false;
                                pointModel.IsLong = false;
                                pointModel.Time = symbolModel.oHLCs[i + 1].DateTime.ToOADate();
                                pointModel.Percent = percent;
                                algorithmModel.Points.Add(pointModel);

                                algorithmModel.Minus += 1;
                                algorithmModel.MinusPercent += percent;
                                i = i + kline;
                            }
                        }
                        //else
                        //{
                        //    // Long
                        //    if (CheckLong(symbolModel, algorithmModel, i + 1, i + 2 + kline))
                        //    {
                        //        algorithmModel.x.Add(symbolModel.oHLCs[i + 1].DateTime.ToOADate());
                        //        algorithmModel.y.Add(symbolModel.oHLCs[i + 1].Close);

                        //        algorithmModel.xClose.Add(symbolModel.oHLCs[i + 2 + kline].DateTime.ToOADate());
                        //        algorithmModel.yClose.Add(symbolModel.oHLCs[i + 2 + kline].Close);

                        //        double percent = Math.Round((symbolModel.oHLCs[i + 2 + kline].Close - symbolModel.oHLCs[i + 1].Close) / symbolModel.oHLCs[i + 1].Close * 100, 2);

                        //        PointModel pointModel = new();
                        //        pointModel.IsPositive = true;
                        //        pointModel.IsLong = true;
                        //        pointModel.Time = symbolModel.oHLCs[i + 1].DateTime.ToOADate();
                        //        pointModel.Percent = percent;
                        //        algorithmModel.Points.Add(pointModel);

                        //        algorithmModel.Plus += 1;
                        //        algorithmModel.PlusPercent += percent;
                        //        i = i + kline;
                        //    }
                        //    else
                        //    {
                        //        algorithmModel.x.Add(symbolModel.oHLCs[i + 1].DateTime.ToOADate());
                        //        algorithmModel.y.Add(symbolModel.oHLCs[i + 1].Close);

                        //        double percent = StopLoss;

                        //        PointModel pointModel = new();
                        //        pointModel.IsPositive = false;
                        //        pointModel.IsLong = true;
                        //        pointModel.Time = symbolModel.oHLCs[i + 1].DateTime.ToOADate();
                        //        pointModel.Percent = percent;
                        //        algorithmModel.Points.Add(pointModel);

                        //        algorithmModel.Minus += 1;
                        //        algorithmModel.MinusPercent += percent;
                        //        i = i + kline;
                        //    }
                        //}
                    }
                }
            }

            ListAlgorithms.Add(algorithmModel);
        }
        private (bool, int) CheckShort(SymbolModel symbolModel, AlgorithmModel algorithmModel, int start, int end)
        {
            for (int i = (start + 1); i <= end; i++)
            {
                if ((symbolModel.oHLCs[start].Close + (symbolModel.oHLCs[start].Close * (StopLoss / 100))) <= symbolModel.oHLCs[i].High)
                {
                    algorithmModel.xClose.Add(symbolModel.oHLCs[i].DateTime.ToOADate());
                    algorithmModel.yClose.Add(symbolModel.oHLCs[start].Close + (symbolModel.oHLCs[start].Close * (StopLoss / 100)));
                    return (false, i);
                }
            }
            return (true, 0);
        }
        private bool CheckLong(SymbolModel symbolModel, AlgorithmModel algorithmModel, int start, int end)
        {
            for (int i = (start + 1); i <= end; i++)
            {
                if ((symbolModel.oHLCs[start].Close - (symbolModel.oHLCs[start].Close * (StopLoss / 100))) >= symbolModel.oHLCs[i].Low)
                {
                    algorithmModel.xClose.Add(symbolModel.oHLCs[i].DateTime.ToOADate());
                    algorithmModel.yClose.Add(symbolModel.oHLCs[start].Close - (symbolModel.oHLCs[start].Close * (StopLoss / 100)));
                    return false;
                }
            }
            return true;
        }
        public void CalculateAlgorithmFive(SymbolModel symbolModel)
        {
            for (int i = 2; i < 20; i++)
            {
                for (int j = 0; j < 30; j++)
                {
                    AlgorithmFive(symbolModel, i, j);
                }
            }
        }
        private void AlgorithmFive(SymbolModel symbolModel, int mul, int kline)
        {
            AlgorithmModel algorithmModel = new AlgorithmModel();

            algorithmModel.Open = mul;
            algorithmModel.Close = kline;

            for (int i = 0; i < symbolModel.oHLCs.Count - 3 - kline; i++)
            {
                if (i > 30)
                {
                    double sum = 0;
                    for (int j = i; j > (i - 30); j--)
                    {
                        sum += (symbolModel.oHLCs[j].High - symbolModel.oHLCs[j].Low);
                    }
                    double average = (sum / 30);

                    if ((symbolModel.oHLCs[i + 1].High - symbolModel.oHLCs[i + 1].Low) > (average * mul))
                    {
                        if (symbolModel.oHLCs[i + 1].Close < symbolModel.oHLCs[i + 1].Open)
                        {
                            // Short
                            (bool check, int close) = CheckShort(symbolModel, algorithmModel, i + 1, i + 2 + kline);
                            if (check)
                            {
                                algorithmModel.x.Add(symbolModel.oHLCs[i + 1].DateTime.ToOADate());
                                algorithmModel.y.Add(symbolModel.oHLCs[i + 1].Close);

                                algorithmModel.xClose.Add(symbolModel.oHLCs[i + 2 + kline].DateTime.ToOADate());
                                algorithmModel.yClose.Add(symbolModel.oHLCs[i + 2 + kline].Close);


                                double percent = Math.Round((symbolModel.oHLCs[i + 1].Close - symbolModel.oHLCs[i + 2 + kline].Close) / symbolModel.oHLCs[i + 2 + kline].Close * 100, 2);

                                PointModel pointModel = new();
                                pointModel.IsPositive = true;
                                pointModel.IsLong = false;
                                pointModel.Time = symbolModel.oHLCs[i + 1].DateTime.ToOADate();
                                pointModel.Percent = percent;
                                algorithmModel.Points.Add(pointModel);

                                algorithmModel.Plus += 1;
                                algorithmModel.PlusPercent += percent;

                                BetModel betModel = new();
                                betModel.Symbol = symbolModel.Name;
                                betModel.IsPositive = true;
                                betModel.IsLong = false;
                                betModel.OpenPrice = symbolModel.oHLCs[i + 2].Open;
                                betModel.ClosePrice = symbolModel.oHLCs[i + 2 + kline].Close;
                                betModel.OpenTime = symbolModel.oHLCs[i + 2].DateTime;
                                betModel.CloseTime = symbolModel.oHLCs[i + 2 + kline].DateTime;
                                betModel.Profit = percent;
                                betModel.StopLoss = StopLoss;
                                betModel.Open = algorithmModel.Open;
                                betModel.Close = algorithmModel.Close;
                                betModel.Interval = Interval;
                                algorithmModel.BetModels.Add(betModel);

                                i = i + kline;
                            }
                            else
                            {
                                algorithmModel.x.Add(symbolModel.oHLCs[i + 1].DateTime.ToOADate());
                                algorithmModel.y.Add(symbolModel.oHLCs[i + 1].Close);

                                double percent = StopLoss;

                                PointModel pointModel = new();
                                pointModel.IsPositive = false;
                                pointModel.IsLong = false;
                                pointModel.Time = symbolModel.oHLCs[i + 1].DateTime.ToOADate();
                                pointModel.Percent = percent;
                                algorithmModel.Points.Add(pointModel);

                                algorithmModel.Minus += 1;
                                algorithmModel.MinusPercent += percent;


                                BetModel betModel = new();
                                betModel.Symbol = symbolModel.Name;
                                betModel.IsPositive = false;
                                betModel.IsLong = false;
                                betModel.OpenPrice = symbolModel.oHLCs[i + 2].Open;
                                betModel.ClosePrice = symbolModel.oHLCs[i + 2].Open + (symbolModel.oHLCs[i + 2].Open / 100 * StopLoss);
                                betModel.OpenTime = symbolModel.oHLCs[i + 2].DateTime;
                                betModel.CloseTime = symbolModel.oHLCs[close].DateTime;
                                betModel.Profit = -percent;
                                betModel.StopLoss = StopLoss;
                                betModel.Open = algorithmModel.Open;
                                betModel.Close = algorithmModel.Close;
                                betModel.Interval = Interval;
                                algorithmModel.BetModels.Add(betModel);

                                i = i + kline;
                            }
                        }
                        //else
                        //{
                        //    // Long
                        //    if (CheckLongFive(symbolModel, algorithmModel, i + 1, i + 2 + kline))
                        //    {
                        //        algorithmModel.x.Add(symbolModel.oHLCs[i + 1].DateTime.ToOADate());
                        //        algorithmModel.y.Add(symbolModel.oHLCs[i + 1].Close);

                        //        algorithmModel.xClose.Add(symbolModel.oHLCs[i + 2 + kline].DateTime.ToOADate());
                        //        algorithmModel.yClose.Add(symbolModel.oHLCs[i + 2 + kline].Close);

                        //        double percent = Math.Round((symbolModel.oHLCs[i + 2 + kline].Close - symbolModel.oHLCs[i + 1].Close) / symbolModel.oHLCs[i + 1].Close * 100, 2);

                        //        PointModel pointModel = new();
                        //        pointModel.IsPositive = true;
                        //        pointModel.IsLong = true;
                        //        pointModel.Time = symbolModel.oHLCs[i + 1].DateTime.ToOADate();
                        //        pointModel.Percent = percent;
                        //        algorithmModel.Points.Add(pointModel);

                        //        algorithmModel.Plus += 1;
                        //        algorithmModel.PlusPercent += percent;
                        //        i = i + kline;
                        //    }
                        //    else
                        //    {
                        //        algorithmModel.x.Add(symbolModel.oHLCs[i + 1].DateTime.ToOADate());
                        //        algorithmModel.y.Add(symbolModel.oHLCs[i + 1].Close);

                        //        double percent = StopLoss;

                        //        PointModel pointModel = new();
                        //        pointModel.IsPositive = false;
                        //        pointModel.IsLong = true;
                        //        pointModel.Time = symbolModel.oHLCs[i + 1].DateTime.ToOADate();
                        //        pointModel.Percent = percent;
                        //        algorithmModel.Points.Add(pointModel);

                        //        algorithmModel.Minus += 1;
                        //        algorithmModel.MinusPercent += percent;
                        //        i = i + kline;
                        //    }
                        //}
                    }
                }
            }

            ListAlgorithms.Add(algorithmModel);
        }

        public void CalculateAlgorithmSix(SymbolModel symbolModel)
        {
            AlgorithmSix(symbolModel);
        }
        private void AlgorithmSix(SymbolModel symbolModel)
        {
            AlgorithmModel algorithmModel = new AlgorithmModel();

            int i = 0;

            while (true)
            {
                i = Six(i, symbolModel, algorithmModel);
                if (i == -1) break;
                i++;
            }



            //for (int i = 0; i < symbolModel.oHLCs.Count - 1; i++)
            //{
            //    i = Six(i, symbolModel, algorithmModel);
            //}

            ListAlgorithms.Add(algorithmModel);
        }
        private int Six(int i, SymbolModel symbolModel, AlgorithmModel algorithmModel)
        {
            double open = symbolModel.oHLCs[i].Open;
            List<double> sum = new List<double>();
            sum.Add(open);
            algorithmModel.x.Add(symbolModel.oHLCs[i].DateTime.ToOADate());
            algorithmModel.y.Add(open);
            int k = 1;
            if (true)
            {
                // Short
                for (; i < symbolModel.oHLCs.Count - 1; i++)
                {

                    double average = sum.Sum() / sum.Count;

                    if(symbolModel.oHLCs[i].High > (open + (open / 100 * 1)))
                    {
                        while (true)
                        {
                            if(symbolModel.oHLCs[i].High > (open + (open / 100 * 1)))
                            {
                                k++;
                                // Закрытие стратегии в убыток
                                if (k == 6)
                                {
                                    algorithmModel.xClose.Add(symbolModel.oHLCs[i].DateTime.ToOADate());
                                    algorithmModel.yClose.Add(open + (open / 100 * 1));

                                    //algorithmModel.MinusPercent += (5 * 50 / 100 * 3);
                                    algorithmModel.MinusPercent += 7.5;
                                    algorithmModel.Minus += 1;
                                    return i;
                                }

                                open = open + (open / 100 * 1);
                                sum.Add(open);
                                algorithmModel.x.Add(symbolModel.oHLCs[i].DateTime.ToOADate());
                                algorithmModel.y.Add(open);
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                    else if (symbolModel.oHLCs[i].Low < (average - (average / 100 * 1)))
                    {
                        algorithmModel.xClose.Add(symbolModel.oHLCs[i].DateTime.ToOADate());
                        algorithmModel.yClose.Add(average - (average / 100 * 1));

                        if(k == 1) algorithmModel.PlusPercent += 0.5;
                        else if(k == 2) algorithmModel.PlusPercent += 1;
                        else if(k == 3) algorithmModel.PlusPercent += 1.5;
                        else if(k == 4) algorithmModel.PlusPercent += 2;
                        else if(k == 5) algorithmModel.PlusPercent += 2.5;

                        algorithmModel.Plus += 1;
                        return i;
                    }

                }

            }
            else
            {
                // Long
                for (; i < symbolModel.oHLCs.Count - 1; i++)
                {
                    

                    double average = sum.Sum() / sum.Count;

                    if (symbolModel.oHLCs[i].Low < (open - (open / 100 * 1)))
                    {
                        while (true)
                        {
                            if (symbolModel.oHLCs[i].Low < (open - (open / 100 * 1)))
                            {
                                k++;
                                // Закрытие стратегии в убыток
                                if (k == 6)
                                {
                                    algorithmModel.xClose.Add(symbolModel.oHLCs[i].DateTime.ToOADate());
                                    algorithmModel.yClose.Add(open - (open / 100 * 1));

                                    //algorithmModel.MinusPercent += (5 * 50 / 100 * 3);
                                    algorithmModel.MinusPercent += 7.5;
                                    algorithmModel.Minus += 1;
                                    return i;
                                }

                                open = open - (open / 100 * 1);
                                sum.Add(open);
                                algorithmModel.x.Add(symbolModel.oHLCs[i].DateTime.ToOADate());
                                algorithmModel.y.Add(open);
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                    else if (symbolModel.oHLCs[i].High > (average + (average / 100 * 1)))
                    {
                        algorithmModel.xClose.Add(symbolModel.oHLCs[i].DateTime.ToOADate());
                        algorithmModel.yClose.Add(average + (average / 100 * 1));

                        algorithmModel.PlusPercent += ((double)k * 50 / 100);
                        algorithmModel.Plus += 1;
                        return i;
                    }
                }
            }
            return -1;
        }
        private bool RandomPosition()
        {
            if(new Random().Next(0, 1) == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void CalculateAlgorithmSeven(SymbolModel symbolModel)
        {
            AlgorithmSeven(symbolModel);
        }
        private void AlgorithmSeven(SymbolModel symbolModel)
        {
            AlgorithmModel algorithmModel = new AlgorithmModel();

            int i = 0;

            while (true)
            {
                i = Seven(i, symbolModel, algorithmModel);
                if (i == -1) break;
                i++;
            }

            ListAlgorithms.Add(algorithmModel);
        }
        private int Seven(int i, SymbolModel symbolModel, AlgorithmModel algorithmModel)
        {
            double open = symbolModel.oHLCs[i].Open;
            List<double> sum = new List<double>();
            sum.Add(open);
            algorithmModel.x.Add(symbolModel.oHLCs[i].DateTime.ToOADate());
            algorithmModel.y.Add(open);
            int k = 1;
            if (true)
            {
                // Short
                for (; i < symbolModel.oHLCs.Count - 1; i++)
                {
                    if (k >= 12)
                    {
                        algorithmModel.xClose.Add(symbolModel.oHLCs[i].DateTime.ToOADate());
                        algorithmModel.yClose.Add(open + (open / 100 * 1));

                        algorithmModel.MinusPercent += 3070.5;
                        algorithmModel.Minus += 1;
                        return i;
                    }

                    if (symbolModel.oHLCs[i].High > (open + (open / 100 * 3)))
                    {
                        while (true)
                        {
                            if (symbolModel.oHLCs[i].High > (open + (open / 100 * 3)))
                            {
                                k++;
                                open = (open + (open / 100 * 3));
                                sum.Add(open);
                                algorithmModel.x.Add(symbolModel.oHLCs[i].DateTime.ToOADate());
                                algorithmModel.y.Add(open);
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                    else if (k == 1)
                    {
                        if (symbolModel.oHLCs[i].Low < (sum[0] - (sum[0] * 0.01)))
                        {
                            algorithmModel.xClose.Add(symbolModel.oHLCs[i].DateTime.ToOADate());
                            algorithmModel.yClose.Add(sum[0] - (sum[0] * 0.01));

                            algorithmModel.PlusPercent += 0.25;
                            algorithmModel.Plus += 1;
                            return i;
                        }
                    }
                    else if (k == 2)
                    {
                        if (symbolModel.oHLCs[i].Low < (sum[0] - (sum[0] * 0.02)))
                        {
                            algorithmModel.xClose.Add(symbolModel.oHLCs[i].DateTime.ToOADate());
                            algorithmModel.yClose.Add(sum[0] - (sum[0] * 0.02));

                            algorithmModel.PlusPercent += 0.75;
                            algorithmModel.Plus += 1;
                            return i;
                        }
                    }
                    else if (k == 3)
                    {
                        if (symbolModel.oHLCs[i].Low < (sum[0] - (sum[0] * 0.0429)))
                        {
                            algorithmModel.xClose.Add(symbolModel.oHLCs[i].DateTime.ToOADate());
                            algorithmModel.yClose.Add(sum[0] - (sum[0] * 0.0429));

                            algorithmModel.PlusPercent += 1.75;
                            algorithmModel.Plus += 1;
                            return i;
                        }
                    }
                    else if (k == 4)
                    {
                        if (symbolModel.oHLCs[i].Low < (sum[0] - (sum[0] * 0.068)))
                        {
                            algorithmModel.xClose.Add(symbolModel.oHLCs[i].DateTime.ToOADate());
                            algorithmModel.yClose.Add(sum[0] - (sum[0] * 0.068));

                            algorithmModel.PlusPercent += 3.75;
                            algorithmModel.Plus += 1;
                            return i;
                        }
                    }
                    else if (k == 5)
                    {
                        if (symbolModel.oHLCs[i].Low < (sum[0] - (sum[0] * 0.0948)))
                        {
                            algorithmModel.xClose.Add(symbolModel.oHLCs[i].DateTime.ToOADate());
                            algorithmModel.yClose.Add(sum[0] - (sum[0] * 0.0948));

                            algorithmModel.PlusPercent += 7.75;
                            algorithmModel.Plus += 1;
                            return i;
                        }
                    }
                    else if (k == 6)
                    {
                        if (symbolModel.oHLCs[i].Low < (sum[0] - (sum[0] * 0.1229)))
                        {
                            algorithmModel.xClose.Add(symbolModel.oHLCs[i].DateTime.ToOADate());
                            algorithmModel.yClose.Add(sum[0] - (sum[0] * 0.1229));

                            algorithmModel.PlusPercent += 15.75;
                            algorithmModel.Plus += 1;
                            return i;
                        }
                    }
                    else if (k == 7)
                    {
                        if (symbolModel.oHLCs[i].Low < (sum[0] - (sum[0] * 0.1517)))
                        {
                            algorithmModel.xClose.Add(symbolModel.oHLCs[i].DateTime.ToOADate());
                            algorithmModel.yClose.Add(sum[0] - (sum[0] * 0.1517));

                            algorithmModel.PlusPercent += 31.75;
                            algorithmModel.Plus += 1;
                            return i;
                        }
                    }
                    else if (k == 8)
                    {
                        if (symbolModel.oHLCs[i].Low < (sum[0] - (sum[0] * 0.1809)))
                        {
                            algorithmModel.xClose.Add(symbolModel.oHLCs[i].DateTime.ToOADate());
                            algorithmModel.yClose.Add(sum[0] - (sum[0] * 0.1809));

                            algorithmModel.PlusPercent += 63.75;
                            algorithmModel.Plus += 1;
                            return i;
                        }
                    }
                    else if (k == 9)
                    {
                        if (symbolModel.oHLCs[i].Low < (sum[0] - (sum[0] * 0.2105)))
                        {
                            algorithmModel.xClose.Add(symbolModel.oHLCs[i].DateTime.ToOADate());
                            algorithmModel.yClose.Add(sum[0] - (sum[0] * 0.2105));

                            algorithmModel.PlusPercent += 127.75;
                            algorithmModel.Plus += 1;
                            return i;
                        }
                    }
                    else if (k == 10)
                    {
                        if (symbolModel.oHLCs[i].Low < (sum[0] - (sum[0] * 0.2403)))
                        {
                            algorithmModel.xClose.Add(symbolModel.oHLCs[i].DateTime.ToOADate());
                            algorithmModel.yClose.Add(sum[0] - (sum[0] * 0.2403));

                            algorithmModel.PlusPercent += 255.75;
                            algorithmModel.Plus += 1;
                            return i;
                        }
                    }
                    else if (k == 11)
                    {
                        if (symbolModel.oHLCs[i].Low < (sum[0] - (sum[0] * 0.2702)))
                        {
                            algorithmModel.xClose.Add(symbolModel.oHLCs[i].DateTime.ToOADate());
                            algorithmModel.yClose.Add(sum[0] - (sum[0] * 0.2702));

                            algorithmModel.PlusPercent += 511.75;
                            algorithmModel.Plus += 1;
                            return i;
                        }
                    }

                }

            }
            else
            {
                // Long
                for (; i < symbolModel.oHLCs.Count - 1; i++)
                {
                    if (k >= 6)
                    {
                        algorithmModel.xClose.Add(symbolModel.oHLCs[i].DateTime.ToOADate());
                        algorithmModel.yClose.Add(open - (open / 100 * 1));

                        algorithmModel.MinusPercent += (6 * 50 / 100 * 4);
                        algorithmModel.Minus += 1;
                        return i;
                    }

                    double average = sum.Sum() / sum.Count;

                    if (symbolModel.oHLCs[i].Low < open - (open / 100 * 1))
                    {
                        while (true)
                        {
                            if (symbolModel.oHLCs[i].Low < open - (open / 100 * 1))
                            {
                                k++;
                                open = open - (open / 100 * 1);
                                sum.Add(open);
                                algorithmModel.x.Add(symbolModel.oHLCs[i].DateTime.ToOADate());
                                algorithmModel.y.Add(open);
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                    else if (symbolModel.oHLCs[i].High > average + (average / 100 * 1))
                    {
                        algorithmModel.xClose.Add(symbolModel.oHLCs[i].DateTime.ToOADate());
                        algorithmModel.yClose.Add(average + (average / 100 * 1));

                        algorithmModel.PlusPercent += ((double)k * 50 / 100);
                        algorithmModel.Plus += 1;
                        return i;
                    }
                }
            }
            return -1;
        }
        public void CalculateAlgorithmEight(SymbolModel symbolModel)
        {
            AlgorithmEight(symbolModel);
        }
        private void AlgorithmEight(SymbolModel symbolModel)
        {
            AlgorithmModel algorithmModel = new AlgorithmModel();

            int i = 0;

            while (true)
            {
                if (i < symbolModel.oHLCs.Count - 1) {
                    i = Eight(i, symbolModel, algorithmModel);
                }
                else break;
                i++;
            }

            ListAlgorithms.Add(algorithmModel);
        }
        private int Eight(int i, SymbolModel symbolModel, AlgorithmModel algorithmModel)
        {
            double open = symbolModel.oHLCs[i].Open;
            double openShort = open + (open * 0.01);
            double slShort = open + (open * 0.03);
            double tpShort = open + (open * 0.005);
            double openLong = open - (open * 0.01);
            double slLong = open - (open * 0.03);
            double tpLong = open - (open * 0.005);

            if (symbolModel.oHLCs[i].High > openShort)
            {

                algorithmModel.x.Add(symbolModel.oHLCs[i].DateTime.ToOADate());
                algorithmModel.y.Add(openShort);
                for (; i < symbolModel.oHLCs.Count - 1; i++)
                {
                    if (symbolModel.oHLCs[i].High > slShort || symbolModel.oHLCs[i + 1].High > slShort)
                    {
                        algorithmModel.xClose.Add(symbolModel.oHLCs[i].DateTime.ToOADate());
                        algorithmModel.yClose.Add(slShort);

                        algorithmModel.MinusPercent += 2;

                        algorithmModel.Minus += 1;
                        return i;
                    }
                    if (symbolModel.oHLCs[i + 1].Low < tpShort)
                    {
                        algorithmModel.xClose.Add(symbolModel.oHLCs[i].DateTime.ToOADate());
                        algorithmModel.yClose.Add(tpShort);

                        algorithmModel.PlusPercent += 0.5;

                        algorithmModel.Plus += 1;
                        return i;
                    }
                }
            }
            else
            if (symbolModel.oHLCs[i].Low < openLong)
            {
                algorithmModel.x.Add(symbolModel.oHLCs[i].DateTime.ToOADate());
                algorithmModel.y.Add(openLong);

                for (; i < symbolModel.oHLCs.Count - 1; i++)
                {
                    if (symbolModel.oHLCs[i].Low < slLong || symbolModel.oHLCs[i + 1].Low < slLong)
                    {
                        algorithmModel.xClose.Add(symbolModel.oHLCs[i].DateTime.ToOADate());
                        algorithmModel.yClose.Add(slLong);

                        algorithmModel.MinusPercent += 2;

                        algorithmModel.Minus += 1;
                        return i;
                    }
                    if (symbolModel.oHLCs[i + 1].High > tpLong)
                    {
                        algorithmModel.xClose.Add(symbolModel.oHLCs[i].DateTime.ToOADate());
                        algorithmModel.yClose.Add(tpLong);

                        algorithmModel.PlusPercent += 0.5;

                        algorithmModel.Plus += 1;
                        return i;
                    }
                }
            }

            return i;
        }
        public void CalculateAlgorithmNine(SymbolModel symbolModel)
        {
            AlgorithmNine(symbolModel);
        }
        private void AlgorithmNine(SymbolModel symbolModel)
        {
            AlgorithmModel algorithmModel = new AlgorithmModel();

            int i = 0;

            while (true)
            {
                if (i < symbolModel.oHLCs.Count - 1)
                {
                    i = Nine(i, symbolModel, algorithmModel);
                }
                else break;
                i++;
            }

            ListAlgorithms.Add(algorithmModel);
        }
        private int Nine(int i, SymbolModel symbolModel, AlgorithmModel algorithmModel)
        {
            double open = symbolModel.oHLCs[i].Open;
            double openShort = open - (open * 0.01);
            double slShort = open - (open * 0.005);
            double tpShort = open - (open * 0.03);
            double openLong = open + (open * 0.01);
            double slLong = open + (open * 0.005);
            double tpLong = open + (open * 0.03);

            if (symbolModel.oHLCs[i].High > openLong)
            {

                algorithmModel.x.Add(symbolModel.oHLCs[i].DateTime.ToOADate());
                algorithmModel.y.Add(openLong);
                for (; i < symbolModel.oHLCs.Count - 1; i++)
                {
                    if (symbolModel.oHLCs[i + 1].Low < slLong)
                    {
                        algorithmModel.xClose.Add(symbolModel.oHLCs[i].DateTime.ToOADate());
                        algorithmModel.yClose.Add(slLong);

                        algorithmModel.MinusPercent += 0.5;

                        algorithmModel.Minus += 1;
                        return i;
                    }
                    if (symbolModel.oHLCs[i + 1].High > tpLong)
                    {
                        algorithmModel.xClose.Add(symbolModel.oHLCs[i].DateTime.ToOADate());
                        algorithmModel.yClose.Add(tpLong);

                        algorithmModel.PlusPercent += 2;

                        algorithmModel.Plus += 1;
                        return i;
                    }
                }


            }

                return i;
        }
    }
}
