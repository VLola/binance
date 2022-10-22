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
        public ChartView(string name)
        {
            InitializeComponent();
            double[] dataX = new double[] { 1, 2, 3, 4, 5 };
            double[] dataY = new double[] { 1, 4, 9, 16, 25 };
            plt.Plot.AddScatter(dataX, dataY);
            this.Title = name;
        }
    }
}
