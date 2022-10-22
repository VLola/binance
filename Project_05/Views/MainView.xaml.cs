using Project_05.ViewModels;
using System.Windows.Controls;

namespace Project_05.Views
{
    public partial class MainView : UserControl
    {
        public MainView()
        {
            InitializeComponent();
            this.DataContext = new MainViewModel();
        }
    }
}
