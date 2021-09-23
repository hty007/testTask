using System.Windows;

namespace XmlServer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class ServerMainView : Window
    {
        private ServerMainViewModel model;

        public ServerMainView()
        {
            InitializeComponent();
            model = new ServerMainViewModel();
            DataContext = model;
            Closed += model.Closed;
        }
    }
}
