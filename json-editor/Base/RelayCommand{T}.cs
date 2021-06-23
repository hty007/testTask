using System;
using System.Windows.Input;

namespace WPFStorage.Base
{
    public class RelayCommand<T> : ICommand
    {
        private readonly Action<T> execute;
        private readonly Predicate<T> canExecute;

        public RelayCommand(Action<T> execute, Predicate<T> canExecute = null)
        {
            if (execute == null)
                throw new ArgumentNullException(nameof(execute));

            this.execute = execute;
            this.canExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            if (canExecute == null)
                return true;

            if (parameter is T param)
                return canExecute(param);
            return false;
        }

        public void Execute(object parameter)
        {
            if (parameter is T parameterT)
                execute.Invoke(parameterT);
        }

        public void RaiseCanExecuteChanged()
            => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
}
