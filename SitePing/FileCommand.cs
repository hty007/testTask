using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SitePing
{
    abstract class cCommand : ICommand
    {
        protected MainControler _cvm;
        public cCommand(MainControler cvm)
        {
            _cvm = cvm;
        }
        public event EventHandler CanExecuteChanged;
        public abstract bool CanExecute(object parameter);
        public abstract void Execute(object parameter);
    }
    class AddCommand : cCommand
    {
        public AddCommand(MainControler cvm) : base(cvm)
        {
        }
        public override bool CanExecute(object parameter)
        {
            return true;
        }
        public override void Execute(object parameter)
        {
            Car car = new Car();
            _cvm.Cars.Insert(0, car);
            _cvm.SelectedCar = car;
        }
    }

}
