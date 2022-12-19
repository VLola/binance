using CryptoExchange.Net.CommonObjects;
using System;
using System.Collections.Generic;
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
        double StopLoss = 0.05;
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
                            if (CheckShort(symbolModel, algorithmModel, i + 1, i + 2 + kline))
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
        private bool CheckShort(SymbolModel symbolModel, AlgorithmModel algorithmModel, int start, int end)
        {
            for (int i = (start + 1); i <= end; i++)
            {
                if ((symbolModel.oHLCs[start].Close + (symbolModel.oHLCs[start].Close * (StopLoss / 100))) <= symbolModel.oHLCs[i].High)
                {
                    algorithmModel.xClose.Add(symbolModel.oHLCs[i].DateTime.ToOADate());
                    algorithmModel.yClose.Add(symbolModel.oHLCs[start].Close + (symbolModel.oHLCs[start].Close * (StopLoss / 100)));
                    return false;
                }
            }
            return true;
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
    }
}
