using System;

namespace Project_06.Models
{
    public class Algorithms
    {
        public AlgorithmModel AlgorithmOne { get; set; } = new();
        public AlgorithmModel AlgorithmTwo { get; set; } = new();
        public void CalculateAlgorithmOne(SymbolModel symbolModel)
        {
            int mul = 4;

            AlgorithmOne.Plus = 0;
            AlgorithmOne.PlusPercent = 0;
            AlgorithmOne.Minus = 0;
            AlgorithmOne.MinusPercent = 0;
            AlgorithmOne.x.Clear();
            AlgorithmOne.y.Clear();
            AlgorithmOne.xIndicatorLong.Clear();
            AlgorithmOne.yIndicatorLong.Clear();
            AlgorithmOne.xIndicatorShort.Clear();
            AlgorithmOne.yIndicatorShort.Clear();
            double indicator = 0;
            AlgorithmOne.xIndicatorLong.Add(symbolModel.oHLCs[0].DateTime.ToOADate());
            AlgorithmOne.yIndicatorLong.Add(0);

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
                        AlgorithmOne.x.Add(symbolModel.oHLCs[i + 1].DateTime.ToOADate());
                        AlgorithmOne.y.Add(symbolModel.oHLCs[i + 1].Close);

                        //if (oHLCs[i + 1].Close > oHLCs[i - 30].Close)
                        if (!reverse)
                        {
                            if (symbolModel.oHLCs[i + 1].Close < symbolModel.oHLCs[i + 1].Open)
                            {
                                // Short
                                if (symbolModel.oHLCs[i + 1].Close > symbolModel.oHLCs[i + 2].Close)
                                {
                                    minus = false;

                                    indicator += 1;
                                    AlgorithmOne.xIndicatorShort.Add(symbolModel.oHLCs[i + 1].DateTime.ToOADate());
                                    AlgorithmOne.yIndicatorShort.Add(indicator);

                                    AlgorithmOne.Plus += 1;
                                    AlgorithmOne.PlusPercent += Math.Round((symbolModel.oHLCs[i + 1].Close - symbolModel.oHLCs[i + 2].Close) / symbolModel.oHLCs[i + 2].Close * 10000);
                                }
                                else
                                {
                                    if (minus)
                                    {
                                        if (reverse) reverse = false;
                                        else reverse = true;
                                    }
                                    minus = true;

                                    indicator -= 1;
                                    AlgorithmOne.xIndicatorShort.Add(symbolModel.oHLCs[i + 1].DateTime.ToOADate());
                                    AlgorithmOne.yIndicatorShort.Add(indicator);

                                    AlgorithmOne.Minus += 1;
                                    AlgorithmOne.MinusPercent += Math.Round((symbolModel.oHLCs[i + 2].Close - symbolModel.oHLCs[i + 1].Close) / symbolModel.oHLCs[i + 1].Close * 10000);
                                }
                            }
                            else
                            {
                                // Long
                                if (symbolModel.oHLCs[i + 1].Close < symbolModel.oHLCs[i + 2].Close)
                                {
                                    minus = false;

                                    indicator += 1;
                                    AlgorithmOne.xIndicatorLong.Add(symbolModel.oHLCs[i + 1].DateTime.ToOADate());
                                    AlgorithmOne.yIndicatorLong.Add(indicator);

                                    AlgorithmOne.Plus += 1;
                                    AlgorithmOne.PlusPercent += Math.Round((symbolModel.oHLCs[i + 2].Close - symbolModel.oHLCs[i + 1].Close) / symbolModel.oHLCs[i + 1].Close * 10000);
                                }
                                else
                                {
                                    if (minus)
                                    {
                                        if (reverse) reverse = false;
                                        else reverse = true;
                                    }
                                    minus = true;

                                    indicator -= 1;
                                    AlgorithmOne.xIndicatorLong.Add(symbolModel.oHLCs[i + 1].DateTime.ToOADate());
                                    AlgorithmOne.yIndicatorLong.Add(indicator);

                                    AlgorithmOne.Minus += 1;
                                    AlgorithmOne.MinusPercent += Math.Round((symbolModel.oHLCs[i + 1].Close - symbolModel.oHLCs[i + 2].Close) / symbolModel.oHLCs[i + 2].Close * 10000);
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
                                    indicator += 1;
                                    AlgorithmOne.xIndicatorShort.Add(symbolModel.oHLCs[i + 1].DateTime.ToOADate());
                                    AlgorithmOne.yIndicatorShort.Add(indicator);

                                    symbolModel.Plus += 1;
                                    symbolModel.PlusPercent += Math.Round((symbolModel.oHLCs[i + 1].Close - symbolModel.oHLCs[i + 2].Close) / symbolModel.oHLCs[i + 2].Close * 10000);
                                }
                                else
                                {
                                    indicator -= 1;
                                    AlgorithmOne.xIndicatorShort.Add(symbolModel.oHLCs[i + 1].DateTime.ToOADate());
                                    AlgorithmOne.yIndicatorShort.Add(indicator);

                                    AlgorithmOne.Minus += 1;
                                    AlgorithmOne.MinusPercent += Math.Round((symbolModel.oHLCs[i + 2].Close - symbolModel.oHLCs[i + 1].Close) / symbolModel.oHLCs[i + 1].Close * 10000);
                                }
                            }
                            else
                            {
                                // Long
                                if (symbolModel.oHLCs[i + 1].Close < symbolModel.oHLCs[i + 2].Close)
                                {
                                    indicator += 1;
                                    AlgorithmOne.xIndicatorLong.Add(symbolModel.oHLCs[i + 1].DateTime.ToOADate());
                                    AlgorithmOne.yIndicatorLong.Add(indicator);

                                    AlgorithmOne.Plus += 1;
                                    AlgorithmOne.PlusPercent += Math.Round((symbolModel.oHLCs[i + 2].Close - symbolModel.oHLCs[i + 1].Close) / symbolModel.oHLCs[i + 1].Close * 10000);
                                }
                                else
                                {
                                    indicator -= 1;
                                    AlgorithmOne.xIndicatorLong.Add(symbolModel.oHLCs[i + 1].DateTime.ToOADate());
                                    AlgorithmOne.yIndicatorLong.Add(indicator);

                                    AlgorithmOne.Minus += 1;
                                    AlgorithmOne.MinusPercent += Math.Round((symbolModel.oHLCs[i + 1].Close - symbolModel.oHLCs[i + 2].Close) / symbolModel.oHLCs[i + 2].Close * 10000);
                                }
                            }
                        }

                    }
                }
            }
            AlgorithmOne.xIndicatorLong.Add(symbolModel.oHLCs[symbolModel.oHLCs.Count - 1].DateTime.ToOADate());
            AlgorithmOne.yIndicatorLong.Add(0);
        }
        public void CalculateAlgorithmTwo(SymbolModel symbolModel)
        {
            int mul = 4;

            AlgorithmTwo.Plus = 0;
            AlgorithmTwo.PlusPercent = 0;
            AlgorithmTwo.Minus = 0;
            AlgorithmTwo.MinusPercent = 0;
            AlgorithmTwo.x.Clear();
            AlgorithmTwo.y.Clear();
            AlgorithmTwo.xIndicatorLong.Clear();
            AlgorithmTwo.yIndicatorLong.Clear();
            AlgorithmTwo.xIndicatorShort.Clear();
            AlgorithmTwo.yIndicatorShort.Clear();
            double indicator = 0;
            AlgorithmTwo.xIndicatorLong.Add(symbolModel.oHLCs[0].DateTime.ToOADate());
            AlgorithmTwo.yIndicatorLong.Add(0);


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
                        AlgorithmTwo.x.Add(symbolModel.oHLCs[i + 1].DateTime.ToOADate());
                        AlgorithmTwo.y.Add(symbolModel.oHLCs[i + 1].Close);

                        if (symbolModel.oHLCs[i + 1].Close < symbolModel.oHLCs[i + 1].Open)
                        {
                            // Short
                            if (symbolModel.oHLCs[i + 1].Close > symbolModel.oHLCs[i + 2].Close)
                            {

                                indicator += 1;
                                AlgorithmTwo.xIndicatorShort.Add(symbolModel.oHLCs[i + 1].DateTime.ToOADate());
                                AlgorithmTwo.yIndicatorShort.Add(indicator);

                                AlgorithmTwo.Plus += 1;
                                AlgorithmTwo.PlusPercent += Math.Round((symbolModel.oHLCs[i + 1].Close - symbolModel.oHLCs[i + 2].Close) / symbolModel.oHLCs[i + 2].Close * 10000);
                            }
                            else
                            {

                                indicator -= 1;
                                AlgorithmTwo.xIndicatorShort.Add(symbolModel.oHLCs[i + 1].DateTime.ToOADate());
                                AlgorithmTwo.yIndicatorShort.Add(indicator);

                                AlgorithmTwo.Minus += 1;
                                AlgorithmTwo.MinusPercent += Math.Round((symbolModel.oHLCs[i + 2].Close - symbolModel.oHLCs[i + 1].Close) / symbolModel.oHLCs[i + 1].Close * 10000);
                            }
                        }
                        else
                        {
                            // Long
                            if (symbolModel.oHLCs[i + 1].Close < symbolModel.oHLCs[i + 2].Close)
                            {

                                indicator += 1;
                                AlgorithmTwo.xIndicatorLong.Add(symbolModel.oHLCs[i + 1].DateTime.ToOADate());
                                AlgorithmTwo.yIndicatorLong.Add(indicator);

                                AlgorithmTwo.Plus += 1;
                                AlgorithmTwo.PlusPercent += Math.Round((symbolModel.oHLCs[i + 2].Close - symbolModel.oHLCs[i + 1].Close) / symbolModel.oHLCs[i + 1].Close * 10000);
                            }
                            else
                            {

                                indicator -= 1;
                                AlgorithmTwo.xIndicatorLong.Add(symbolModel.oHLCs[i + 1].DateTime.ToOADate());
                                AlgorithmTwo.yIndicatorLong.Add(indicator);

                                AlgorithmTwo.Minus += 1;
                                AlgorithmTwo.MinusPercent += Math.Round((symbolModel.oHLCs[i + 1].Close - symbolModel.oHLCs[i + 2].Close) / symbolModel.oHLCs[i + 2].Close * 10000);
                            }
                        }

                    }
                }
            }
            AlgorithmTwo.xIndicatorLong.Add(symbolModel.oHLCs[symbolModel.oHLCs.Count - 1].DateTime.ToOADate());
            AlgorithmTwo.yIndicatorLong.Add(0);
        }
    }
}
