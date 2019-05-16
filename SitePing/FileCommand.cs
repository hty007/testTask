using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace SitePing
{
    abstract class SiteCommand : ICommand
    {
        protected MainControler mc;
        public SiteCommand(MainControler cvm)
        {
            mc = cvm;
        }
        public event EventHandler CanExecuteChanged;
        public abstract bool CanExecute(object parameter);
        public abstract void Execute(object parameter);
    }
    class AddCommand : SiteCommand
    {
        public AddCommand(MainControler cvm) : base(cvm){}
        public override bool CanExecute(object parameter) => true;
        public override void Execute(object parameter)
        {
            mc.Data.Add(new Pair(mc.AddSite));
            mc.SaveData();            
        }
    }
    class FileCommand : SiteCommand
    {
        public FileCommand(MainControler cvm) : base(cvm) { }
        public override bool CanExecute(object parameter) => true;
        public override void Execute(object parameter)
        {
            System.Diagnostics.Process.Start("notepad.exe", "../../List.txt");
        }
    }
    class ReadMyCommand : SiteCommand
    {
        public ReadMyCommand(MainControler cvm) : base(cvm) { }
        public override bool CanExecute(object parameter) => true;
        public override void Execute(object parameter)
        {
            System.Diagnostics.Process.Start("notepad.exe", "../../ReadMy.txt");
        }
    }

    class TaskCommand : SiteCommand
    {
        public TaskCommand(MainControler cvm) : base(cvm) { }
        public override bool CanExecute(object parameter) => true;
        public override void Execute(object parameter)
        {
            MessageBox.Show(@"Необходимо разработать wpf приложение мониторинга доступности сайтов. Необходимо придерживаться паттерна MVVM
1. Главная окно отображает список сайтов и индикатор доступности.
2. Список сайтов для проверки находится в хранилище (бд, файл).
3. При старте приложения запускается фоновый поток в котором через настраивай промежуток времени проверяются сайты
4. В окне должна быть возможность добавление/удаление сайтов
5. Окно приложения не должно блокироваться во время работы
", "Техническое задание.");
        }
    }

}
