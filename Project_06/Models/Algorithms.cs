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

            double indicator = 0;
            algorithmModel.xIndicatorLong.Add(symbolModel.oHLCs[0].DateTime.ToOADate());
            algorithmModel.yIndicatorLong.Add(0);

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

                                    indicator += 1;
                                    algorithmModel.xIndicatorShort.Add(symbolModel.oHLCs[i + 1].DateTime.ToOADate());
                                    algorithmModel.yIndicatorShort.Add(indicator);

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

                                    indicator -= 1;
                                    algorithmModel.xIndicatorShort.Add(symbolModel.oHLCs[i + 1].DateTime.ToOADate());
                                    algorithmModel.yIndicatorShort.Add(indicator);

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

                                    indicator += 1;
                                    algorithmModel.xIndicatorLong.Add(symbolModel.oHLCs[i + 1].DateTime.ToOADate());
                                    algorithmModel.yIndicatorLong.Add(indicator);

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

                                    indicator -= 1;
                                    algorithmModel.xIndicatorLong.Add(symbolModel.oHLCs[i + 1].DateTime.ToOADate());
                                    algorithmModel.yIndicatorLong.Add(indicator);

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
                                    indicator += 1;
                                    algorithmModel.xIndicatorShort.Add(symbolModel.oHLCs[i + 1].DateTime.ToOADate());
                                    algorithmModel.yIndicatorShort.Add(indicator);

                                    symbolModel.Plus += 1;
                                    symbolModel.PlusPercent += Math.Round((symbolModel.oHLCs[i + 1].Close - symbolModel.oHLCs[i + 2].Close) / symbolModel.oHLCs[i + 2].Close * 10000);
                                }
                                else
                                {
                                    indicator -= 1;
                                    algorithmModel.xIndicatorShort.Add(symbolModel.oHLCs[i + 1].DateTime.ToOADate());
                                    algorithmModel.yIndicatorShort.Add(indicator);

                                    algorithmModel.Minus += 1;
                                    algorithmModel.MinusPercent += Math.Round((symbolModel.oHLCs[i + 2].Close - symbolModel.oHLCs[i + 1].Close) / symbolModel.oHLCs[i + 1].Close * 10000);
                                }
                            }
                            else
                            {
                                // Long
                                if (symbolModel.oHLCs[i + 1].Close < symbolModel.oHLCs[i + 2].Close)
                                {
                                    indicator += 1;
                                    algorithmModel.xIndicatorLong.Add(symbolModel.oHLCs[i + 1].DateTime.ToOADate());
                                    algorithmModel.yIndicatorLong.Add(indicator);

                                    algorithmModel.Plus += 1;
                                    algorithmModel.PlusPercent += Math.Round((symbolModel.oHLCs[i + 2].Close - symbolModel.oHLCs[i + 1].Close) / symbolModel.oHLCs[i + 1].Close * 10000);
                                }
                                else
                                {
                                    indicator -= 1;
                                    algorithmModel.xIndicatorLong.Add(symbolModel.oHLCs[i + 1].DateTime.ToOADate());
                                    algorithmModel.yIndicatorLong.Add(indicator);

                                    algorithmModel.Minus += 1;
                                    algorithmModel.MinusPercent += Math.Round((symbolModel.oHLCs[i + 1].Close - symbolModel.oHLCs[i + 2].Close) / symbolModel.oHLCs[i + 2].Close * 10000);
                                }
                            }
                        }

                    }
                }
            }
            algorithmModel.xIndicatorLong.Add(symbolModel.oHLCs[symbolModel.oHLCs.Count - 1].DateTime.ToOADate());
            algorithmModel.yIndicatorLong.Add(0);
            ListAlgorithms.Add(algorithmModel);
        }
        public void CalculateAlgorithmTwo(SymbolModel symbolModel)
        {

            AlgorithmModel algorithmModel = new AlgorithmModel();
            int mul = 4;

            double indicator = 0;
            algorithmModel.xIndicatorLong.Add(symbolModel.oHLCs[0].DateTime.ToOADate());
            algorithmModel.yIndicatorLong.Add(0);


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

                                indicator += 1;
                                algorithmModel.xIndicatorShort.Add(symbolModel.oHLCs[i + 1].DateTime.ToOADate());
                                algorithmModel.yIndicatorShort.Add(indicator);

                                algorithmModel.Plus += 1;
                                algorithmModel.PlusPercent += Math.Round((symbolModel.oHLCs[i + 1].Close - symbolModel.oHLCs[i + 2].Close) / symbolModel.oHLCs[i + 2].Close * 10000);
                            }
                            else
                            {

                                indicator -= 1;
                                algorithmModel.xIndicatorShort.Add(symbolModel.oHLCs[i + 1].DateTime.ToOADate());
                                algorithmModel.yIndicatorShort.Add(indicator);

                                algorithmModel.Minus += 1;
                                algorithmModel.MinusPercent += Math.Round((symbolModel.oHLCs[i + 2].Close - symbolModel.oHLCs[i + 1].Close) / symbolModel.oHLCs[i + 1].Close * 10000);
                            }
                        }
                        else
                        {
                            // Long
                            if (symbolModel.oHLCs[i + 1].Close < symbolModel.oHLCs[i + 2].Close)
                            {

                                indicator += 1;
                                algorithmModel.xIndicatorLong.Add(symbolModel.oHLCs[i + 1].DateTime.ToOADate());
                                algorithmModel.yIndicatorLong.Add(indicator);

                                algorithmModel.Plus += 1;
                                algorithmModel.PlusPercent += Math.Round((symbolModel.oHLCs[i + 2].Close - symbolModel.oHLCs[i + 1].Close) / symbolModel.oHLCs[i + 1].Close * 10000);
                            }
                            else
                            {

                                indicator -= 1;
                                algorithmModel.xIndicatorLong.Add(symbolModel.oHLCs[i + 1].DateTime.ToOADate());
                                algorithmModel.yIndicatorLong.Add(indicator);

                                algorithmModel.Minus += 1;
                                algorithmModel.MinusPercent += Math.Round((symbolModel.oHLCs[i + 1].Close - symbolModel.oHLCs[i + 2].Close) / symbolModel.oHLCs[i + 2].Close * 10000);
                            }
                        }

                    }
                }
            }
            algorithmModel.xIndicatorLong.Add(symbolModel.oHLCs[symbolModel.oHLCs.Count - 1].DateTime.ToOADate());
            algorithmModel.yIndicatorLong.Add(0);

            ListAlgorithms.Add(algorithmModel);
        }

        public void CalculateAlgorithmThree(SymbolModel symbolModel)
        {

        }
    }
}
