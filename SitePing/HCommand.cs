using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace SitePing
{   

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
