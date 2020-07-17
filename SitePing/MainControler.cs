using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SitePing
{
    public class MainControler : BaseViewModel, IDisposable
    {
        #region Поля и контрукторы
        Thread myThread;
        public ObservableCollection<Pair> Data { get; set; }
        string nameFile = "../../List.txt";
        private int errors;
        private string addSite;
        private Pair selectedPair;
        private bool tryAddSite;
        private int interval;
        private bool work;

        public Pair SelectedPair { get => selectedPair; set { selectedPair = value; OnPropertyChanged("SelectedPair"); } }

        public int Errors{get => errors; set{errors = value;OnPropertyChanged("Errors");}}
        public int Interval { get => interval; set { interval = value; OnPropertyChanged("Interval"); } } 
        public bool TryAddSite { get => tryAddSite; set{ tryAddSite = value;OnPropertyChanged("TryAddSite");}}

        public string AddSite{
            get => addSite;
            set
            {
                addSite = value;
                Pair site = Data.FirstOrDefault(x => x.Site == value);
                if (site == null) 
                    TryAddSite = true; 
                else 
                    TryAddSite = false;
                OnPropertyChanged("AddSite");
            }
        }

        public MainControler()
        {
            Data = new ObservableCollection<Pair>();
            LoadData();
            Interval = 1500;
            Add = new HCommand(AddMethod);
            File = new HCommand(FileMethod);
            ReadMy = new HCommand(ReadMyMethod);
            TestTask = new HCommand(TaskMethod);
            Delete = new HCommand(DeleteMethod);
            myThread = new Thread(new ThreadStart(ThreadCheck));
            myThread.Start();

        }

        ~MainControler()
        {
            work = false;
        }


        #endregion
        #region Команды        

        public HCommand Add { get; private set; }
        public HCommand File { get; private set; }
        public HCommand ReadMy { get; private set; }
        public HCommand TestTask { get; private set; }
        public HCommand Delete { get; private set; }

        private void DeleteMethod(object obj)
        {
            if (SelectedPair != null)
                Data.Remove(SelectedPair);
        }

        private void TaskMethod(object obj)
        {
            MessageBox.Show(@"Необходимо разработать wpf приложение мониторинга доступности сайтов. Необходимо придерживаться паттерна MVVM
1. Главная окно отображает список сайтов и индикатор доступности.
2. Список сайтов для проверки находится в хранилище (бд, файл).
3. При старте приложения запускается фоновый поток в котором через настраивай промежуток времени проверяются сайты
4. В окне должна быть возможность добавление/удаление сайтов
5. Окно приложения не должно блокироваться во время работы
", "Техническое задание.");
        }

        private void ReadMyMethod(object obj)
        {
            System.Diagnostics.Process.Start("notepad.exe", "../../ReadMy.txt");
        }

        private void FileMethod(object obj)
        {
            System.Diagnostics.Process.Start("notepad.exe", "../../List.txt");
        }

        private void AddMethod(object obj)
        {
            Data.Add(new Pair(AddSite));
            SaveData();
        }
        public void ThreadCheck()
        {
            Thread.Sleep(1000);
            work = true;
            while (work)
            {
                try
                {
                    Check();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    
                }
                Thread.Sleep(Interval);
            }
        }
        public void Check()
        {
            var r = new Random();
            try
            {
                lock (Data)
                { 
                    foreach (Pair p in Data)
                    {
                        //p.Status = r.Next(2)==1;
                        try
                        {
                            WebRequest request = WebRequest.Create("http://" + p.Site);
                            //WebRequest request = WebRequest.Create(p.Site);
                            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                            p.Status = (response != null);
                            response.Close();
                        }
                        catch
                        {
                            //p.Status = false;
                            Errors++;
                        }
                    }
                }
            }
            catch (InvalidOperationException)
            {

            }/**/
        }
        internal void DeleteSite(int index)
        {
            Data.RemoveAt(index);
            SaveData();
        }

        #endregion
        #region Работа с файлами и интерфейсы


        public void LoadData()
        {
            if (!System.IO.File.Exists(nameFile))
                throw new FileNotFoundException($"Файл отсутвует в файловой системе по указонному адресу.\n{nameFile}");
            using (StreamReader sr = new StreamReader(nameFile, System.Text.Encoding.Default))
            {
                while (!sr.EndOfStream)
                {
                    Data.Add(new Pair(sr.ReadLine()));
                }
            }
        }

        public void SaveData()
        {
            using (StreamWriter sw = new StreamWriter(nameFile, false, System.Text.Encoding.Default))
            {
                foreach (Pair p in Data)
                    sw.WriteLine(p.Site);
            }
        }

        public void Dispose()
        {
            work = false;
        }
        #endregion Работа с файлами
    }
}
