using Project_06.ViewModels;
using System.Windows;

namespace Project_06
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = new MainViewModel();
        }
    }
}
