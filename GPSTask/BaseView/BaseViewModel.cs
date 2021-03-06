﻿using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Runtime.CompilerServices;

namespace GPSTask
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = null)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
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

    public class HCommand<T> : ICommand
    {
        private Action<T> _method;

        public HCommand(Action<T> method)
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
                if (parameter is T parameterT) _method.Invoke(parameterT);
                else if (parameter is string s_parameter)
                {
                    System.Reflection.ParameterInfo[] pars = _method.Method.GetParameters();
                    if (pars.Length != 1) throw new ArgumentException("Количество параметров больше 1!");
                    var par = pars[0];
                    if (par.ParameterType == typeof(int)
                        && int.TryParse(s_parameter, out int i_parameter)
                        && i_parameter is T par_t)
                        _method.Invoke(par_t);
                    else if (par.ParameterType == typeof(double)
                        && double.TryParse(s_parameter.Replace(',', '.'), out double d_parameter)
                        && d_parameter is T par_t2)
                        _method.Invoke(par_t2);
                    else
                    {
                        MessageBox.Show($"не удалось привести параметр {parameter} к типу { (typeof(T)).FullName }");
                    }
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка метода '{_method.Method.Name}', класса '{_method.Target.GetType().Name}'\nСообщение: {ex.Message}");
            }
        }
    }
}
