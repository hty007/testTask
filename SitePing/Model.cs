using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SitePing
{
    public class Pair : INotifyPropertyChanged
    {
        private string site;
        private bool status;
        

        public Pair(string site)
            {
                Site = site;
            }

        public string Site { get => site; set { site = value; OnPropertyChanged("Site"); } }
        public bool Status { get => status; set { status = value; OnPropertyChanged("Status"); OnPropertyChanged("Brush"); } }
        public System.Windows.Media.SolidColorBrush Brush { get => (status) ? System.Windows.Media.Brushes.Green : System.Windows.Media.Brushes.Red; }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        public override string ToString()
        {
            return site;
        }
    }

    public class Model : IEnumerable
    {
        List<Pair> data;

        public Pair this[int index] { get => data[index]; set => data[index] = value; }
        public List<Pair> List { get => data; }

        public Model() => data = new List<Pair>();

        public void Add(string site)
        {
            var p = new Pair(site);
            data.Add(p);
        }

        public IEnumerator GetEnumerator()
        {
            return ((IEnumerable)data).GetEnumerator();
        }        
    }
}
