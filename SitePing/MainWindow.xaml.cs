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
        static MainControler controler;
        Thread myThread;
               
        public MainWindow()
        {
            InitializeComponent();
            controler = new MainControler();
            //controler.Build(stackSite);
            myThread = new Thread(new ThreadStart(ThreadCheck));            
            DataContext = controler;


            myThread.Start();

            /*
            Binding binding = new Binding();
            binding.ElementName = "controler";
            binding.Path = new PropertyPath("ErrorsStr");
            countError.SetBinding(Label.ContentProperty, binding);/**/
                 
        }
               

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (textBox.Text.Length > 5)
                addButton.IsEnabled = true;
        }


        static public void ThreadCheck()
        {
            Thread.Sleep(1000);            
            while (true)
            {
                controler.Check();
                Thread.Sleep(10000);
            }            
        }

        private void Label_MouseEnter(object sender, MouseEventArgs e)
        {
            //controler.Build(stackSite);
        }

        private void Random_Click(object sender, RoutedEventArgs e)
        {
            var r = new Random();
            int index = r.Next(controler.Data.Count);
            controler.Data[index].Status = !controler.Data[index].Status;
        }

        
        private void Label_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            int index = dataList.SelectedIndex;

            MessageBoxResult choice = MessageBox.Show(string.Format("Удалить элемент '{0}'",controler.Data[index].Site ), 
                "MouseEnter",
                MessageBoxButton.YesNo);
            if (choice == MessageBoxResult.Yes)
                controler.Delete(index);
        } 
    }  

}
