using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;

namespace WPFStorage.Base
{
    public class ObservableObject : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Установить значение хранящего поля свойства и вызвать событие <see cref="PropertyChanged"/> при его обновлении.
        /// </summary>
        /// <typeparam name="T">Тип свойства.</typeparam>
        /// <param name="backField">Поле, хранящее значение свойства.</param>
        /// <param name="value">Новое значение свойства.</param>
        /// <param name="propertyName">Имя свойства.</param>
        /// <returns>true, если значение действительно было установлено.</returns>
        protected virtual bool SetProperty<T>(ref T backField, T value, /*[CallerMemberName]*/ string propertyName = null)
        {
            // Из-за ограничения .net 4.0 задании атрибут [CallerMemberName] не функционирует,
            // по этому пришлось испльзовать конструкцию представленную ниже
            if (propertyName == null)
            {
                StackTrace stackTrace = new StackTrace();
                StackFrame frame = stackTrace.GetFrame(1);
                MethodBase method = frame.GetMethod();
                propertyName = method.Name.Replace("set_", "");
            }

            if (EqualityComparer<T>.Default.Equals(backField, value))
                return false;
            backField = value;
            RaisePropertyChanged(propertyName);
            return true;
        }

        protected virtual void RaisePropertyChanged(/*[CallerMemberName]*/ string propertyName = null)
        {
            // Из-за ограничения .net 4.0 задании атрибут [CallerMemberName] не функционирует,
            // по этому пришлось испльзовать конструкцию представленную ниже
            if (propertyName == null)
            { // TODO: устранить копирование кода
                StackTrace stackTrace = new StackTrace();
                StackFrame frame = stackTrace.GetFrame(1);
                MethodBase method = frame.GetMethod();
                propertyName = method.Name.Replace("set_", "");
            }
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
