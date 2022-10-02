using Project_01.ViewModels;
using System.Windows.Controls;

namespace Project_01.Views
{
    public partial class MainView : UserControl
    {
        public MainView()
        {
            InitializeComponent();
            this.DataContext = new MainViewModel(this);
        }
    }
}
