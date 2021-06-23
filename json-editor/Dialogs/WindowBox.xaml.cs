using System.Windows;

namespace WPFStorage.Dialogs
{
    /// <summary>
    /// Логика взаимодействия для Window1.xaml
    /// </summary>
    public partial class WindowBox : Window
    {
        public WindowBox(InputBoxModel model)
        {
            InitializeComponent();
            model.Success = () => { DialogResult = true; Close(); };
            model.Close = () => { DialogResult = false; Close(); };
            DataContext = model;
            Loaded += (a, b) => { model.Initialize(); };
        }
    }
}
