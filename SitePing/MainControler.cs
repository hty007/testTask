using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Runtime.CompilerServices;
using System.Windows.Controls;

namespace SitePing
{
    public class MainControler : INotifyPropertyChanged
    {
        #region Поля и контрукторы
        public ObservableCollection<Pair> Data { get; set; }
        string nameFile = "../../List.txt";
        private int errors;
        public int Errors { get => errors; set
            {
                errors = value;
                OnPropertyChanged("Errors");
            } }
        public string ErrorsStr
        {
            get => errors.ToString();
        }

        public MainControler()
        {
            Data = new ObservableCollection<Pair>();
            LoadData();
        }

        
        #endregion
        #region Функционал


        public void Add(string site)
        {
            Data.Add(new Pair(site));
            SaveData();
        }

        public void Check()
        {
            var r = new Random();
             try
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
            catch(InvalidOperationException)
            {

            }/**/
        }        

        #endregion
        #region Работа с файлами и интерфейсы
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        public void LoadData()
        {            
            if (!File.Exists(nameFile))
                throw new FileNotFoundException($"Файл отсутвует в файловой системе по указонному адресу.\n{nameFile}");
            using (StreamReader sr = new StreamReader(nameFile, System.Text.Encoding.Default))
            {
                while (!sr.EndOfStream)
                {
                    Data.Add(new Pair(sr.ReadLine()));
                }
            }
        }

        internal void Delete(int index)
        {
            Data.RemoveAt(index);
            SaveData();
        }

        public void SaveData()
        {
            using (StreamWriter sw = new StreamWriter(nameFile, false, System.Text.Encoding.Default))
            {
                foreach (Pair p in Data)
                    sw.WriteLine(p.Site);
            }
        }
        #endregion Работа с файлами
    }
}
