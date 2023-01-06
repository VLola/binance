using Project_07.ViewModels;
using System.Windows;

namespace Project_07
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel();
        }
    }
}
