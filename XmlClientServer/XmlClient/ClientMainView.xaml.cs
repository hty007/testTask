using System;
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
            Closed += ClientMainView_Closed;
        }

        private void ClientMainView_Closed(object sender, System.EventArgs e)
        {
            if (DataContext is IDisposable model)
            {
                model.Dispose();
            }
        }
    }
}
