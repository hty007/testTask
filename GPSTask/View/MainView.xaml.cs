using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GPSTask
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainView : Window
    {
        MainVeiwModel Model;
        public MainView()
        {
            Model = new MainVeiwModel(this);            
            InitializeComponent();
            DataContext = Model;
            Loaded += MainView_Loaded;
        }

        private void MainView_Loaded(object sender, RoutedEventArgs e)
        {
            Model.SetView(this);
        }

        private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Model.AutorClick();
        }
    }
}
