using System;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SitePing
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        
               
        public MainWindow()
        {
            InitializeComponent();           
                       
            DataContext = new MainControler();

            this.Closed += MainWindow_Closed;
                 
        }

        private void MainWindow_Closed(object sender, EventArgs e)
        {
            ((IDisposable)DataContext).Dispose();
        }
    }  

}
