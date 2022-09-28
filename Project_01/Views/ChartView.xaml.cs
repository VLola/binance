using Project_01.ViewModels;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;

namespace Project_01.Views
{
    public partial class ChartView : UserControl
    {
        public double x_old;
        public double y_old;
        public ChartView()
        {
            InitializeComponent();
            this.DataContext = new ChartViewModel();
            MouseWheel += WindowChart_MouseWheel;
            MouseMove += WindowChart_MouseMove;
        }
        #region - Chart Events -
        private void WindowChart_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                double x = 0;
                if (e.GetPosition(this).X - x_old > 0) x = 20;
                else if (e.GetPosition(this).X - x_old < 0) x = -20;
                if (x_old != 0)
                {
                    double width = Chart1.Width;
                    //if (Chart1.Width + x >= 1300 && Chart1.Width + x <= 3500)
                    Chart1.Width = width + x;

                }

                double y = 0;
                if (e.GetPosition(this).Y - y_old > 0) y = 20;
                else if (e.GetPosition(this).Y - y_old < 0) y = -20;
                if (y_old != 0)
                {
                    double margin_top = Chart1.Margin.Top + y;
                    double margin_bottom = Chart1.Margin.Bottom - y;
                    Chart1.Margin = new Thickness(0, margin_top, 0, margin_bottom);
                }
            }
            x_old = e.GetPosition(this).X;
            y_old = e.GetPosition(this).Y;
        }

        private void WindowChart_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (player1Scale.ScaleY + Convert.ToDouble(e.Delta) / 2000 > 0) player1Scale.ScaleY = player1Scale.ScaleY + Convert.ToDouble(e.Delta) / 2000;
        }
        #endregion
    }
}
