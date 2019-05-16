using System;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace SitePing
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static MainControler controler;

        public MainWindow()
        {
            InitializeComponent();
            controler = new MainControler();
            //controler.Build(stackSite);
            Thread myThread = new Thread(new ThreadStart(ThreadCheck));            
            DataContext = controler;


            myThread.Start();

            /*
            Binding binding = new Binding();
            binding.ElementName = "controler";
            binding.Path = new PropertyPath("ErrorsStr");
            countError.SetBinding(Label.ContentProperty, binding);/**/
                 
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            controler.Add(textBox.Text);
            //controler.Build(stackSite);
            textBox.Clear();
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

        private void Label_MouseEnter_1(object sender, MouseEventArgs e)
        {
                    }

        private void TaskButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(@"
Необходимо разработать wpf приложение мониторинга доступности сайтов. Необходимо придерживаться паттерна MVVM
1. Главная окно отображает список сайтов и индикатор доступности.
2. Список сайтов для проверки находится в хранилище (бд, файл).
3. При старте приложения запускается фоновый поток в котором через настраивай промежуток времени проверяются сайты
4. В окне должна быть возможность добавление/удаление сайтов
5. Окно приложения не должно блокироваться во время работы
", "Техническое задание.");
        }

        private void ReadMyButton_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("notepad.exe", "../../ReadMy.txt");
        }

        private void FileButton_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("notepad.exe", "../../List.txt");
        }
    }  

}
