using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace HCalculator
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string prop)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }

    public class BoolToVisuble : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool bvalue)
            {
                if (bvalue)
                    return Visibility.Visible;
            }
            return Visibility.Collapsed;

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class HCommand : ICommand
    {
        private Action<object> _method;

        public HCommand(Action<object> method)
        {
            _method = method;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return _method != null;
        }

        public void Execute(object parameter)
        {
            try
            {
                _method.Invoke(parameter);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, $"Ошибка {this.GetType().Name}");
            }
        }
    }
}
