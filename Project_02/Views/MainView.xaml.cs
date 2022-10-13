using Project_02.ViewModels;
using System.Windows.Controls;

namespace Project_02.Views
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
