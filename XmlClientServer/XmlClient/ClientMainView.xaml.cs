using System.Windows;

namespace XmlClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class ClientMainView : Window
    {
        public ClientMainView()
        {
            InitializeComponent();
            DataContext = new ClientMainViewModel();
        }
    }
}
