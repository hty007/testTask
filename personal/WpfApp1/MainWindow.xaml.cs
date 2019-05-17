using System;
using System.Windows;
using System.Windows.Controls;

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
            DataContext = new ModelPersons();
            var o = (ModelPersons)DataContext;
            ListPerson.ItemsSource = o.FilterPersons;
            PostsList.ItemsSource = o.Posts;

        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void FilterGo_Click(object sender, RoutedEventArgs e)
        {
            var o = (ModelPersons)DataContext;
            o.ApplyFilter();
            
            //ListPerson.ItemsSource = o.FilterPersons;
        }

        private void Filter_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                var o = (ModelPersons)DataContext;
                if (Filter.Text == "")
                {
                    o.SalaryFilter = 0;
                    return;
                }
                o.SalaryFilter = double.Parse(Filter.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    string.Format("Что-то пошло не так! \n{0}", ex.Message),
                    "Exception",
                    MessageBoxButton.OK);
            }
        }

        private void SortUpCheck_Checked(object sender, RoutedEventArgs e)
        {
            var o = (ModelPersons)DataContext;
            o.SortUp = (bool)SortUpCheck.IsChecked;
        }

        private void SortUpCheck_Unchecked(object sender, RoutedEventArgs e)
        {
            var o = (ModelPersons)DataContext;
            o.SortUp = (bool)SortUpCheck.IsChecked;
        }

        private void ClearFilter_Click(object sender, RoutedEventArgs e)
        {
            PostsList.SelectedIndex = -1;
            Filter.Text = "";
            SortUpCheck.IsChecked = false;
            //var o = (ModelPersons)DataContext;
            FilterGo_Click(sender, e);
        }

        private void PostsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //var o = (ModelPersons)DataContext;
            //o.PostFilter = PostsList.SelectedValue;
        }

        private void About_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(
                       string.Format(@"Приложение должно иметь:
1.Список сотрудников, выводит все данные сотрудника.Автоматически упорядочивается по уменьшению зарплаты
2.Выпадающий список, которые содержит все имеющиеся должности
3.Поле ввода, служит для фильтрации сотрудников по зарплате(больше или равно)
4.Кнопка применения фильтра.При нажатии на кнопку, список сотрудников отфильтровывается по выбранной должности из выпадающего списка(2) и зарплате из поля(3)
5.Кнопка сброса фильтра"),
                       "Тестовое задание 1.0",
                       MessageBoxButton.OK);

        }
    }
}
