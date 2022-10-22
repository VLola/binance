using System;

namespace Project_04.Models
{
    public class AverageModel
    {
        public double[] dataX { get; set; }
        public double[] dataY { get; set; }
        public AverageModel(DateTime dateTime, decimal AverageFifteenMinutes, decimal AverageOneHour, decimal AverageFourHour, decimal AverageEightHour, decimal AverageTwelveHour, decimal AverageOneDay, decimal AverageTwoDay)
        {
            dataX = new double[] {
                dateTime.AddHours(-24).ToOADate(),
                dateTime.AddHours(-12).ToOADate(),
                dateTime.AddHours(-6).ToOADate(),
                dateTime.AddHours(-4).ToOADate(),
                dateTime.AddHours(-2).ToOADate(),
                dateTime.AddMinutes(-30).ToOADate(),
                dateTime.AddMinutes(-7.5).ToOADate()
            };
            dataY = new double[] {
                Decimal.ToDouble(AverageTwoDay),
                Decimal.ToDouble(AverageOneDay),
                Decimal.ToDouble(AverageTwelveHour),
                Decimal.ToDouble(AverageEightHour),
                Decimal.ToDouble(AverageFourHour),
                Decimal.ToDouble(AverageOneHour),
                Decimal.ToDouble(AverageFifteenMinutes)
            };
        }
    }
}
