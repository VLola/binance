using Project_04.Models;
using ScottPlot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Project_04.Views
{
    /// <summary>
    /// Логика взаимодействия для ChartView.xaml
    /// </summary>
    public partial class ChartView : Window
    {
        public ChartView(string name, ChartModel chartModel)
        {
            InitializeComponent();
            this.Title = name;
            plt.Plot.AddCandlesticks(chartModel.OHLCs.ToArray());
            foreach (var item in chartModel.Averages)
            {
                plt.Plot.AddScatter(item.dataX, item.dataY);
            }
            plt.Plot.AddScatter(chartModel.OpenOrders.OpenTimes.ToArray(), chartModel.OpenOrders.OpenPrices.ToArray(), color: System.Drawing.Color.Green, lineWidth: 0, markerSize: 8);
            plt.Plot.AddScatter(chartModel.CloseOrders.OpenTimes.ToArray(), chartModel.CloseOrders.OpenPrices.ToArray(), color: System.Drawing.Color.DarkRed, lineWidth: 0, markerSize: 8);
            plt.Refresh();
        }
    }
}
