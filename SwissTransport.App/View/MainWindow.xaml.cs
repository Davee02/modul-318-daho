using MahApps.Metro.Controls;
using SwissTransport.App.ViewModel;

namespace SwissTransport.App.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        private readonly MainViewModel m_ViewModel = new MainViewModel();
        public MainWindow()
        {
            this.DataContext = m_ViewModel;

            InitializeComponent();
        }
    }
}
