using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization;
using System.IO;

namespace WpfApp1
{

    public 
        class ModelPersons
    {
        public ObservableCollection<Person> FilterPersons { get; private set; }
        [DataMember]
        public List<Person> AllPersons { get; private set; }
        public ObservableCollection<string> Posts { get; private set; }
        public bool SortUp { get; set; }
        public string PostFilter { get; set; }
        public double SalaryFilter { get; set; }

        public ModelPersons(bool q)
        {
            if (q)
            {
                AllPersons = new List<Person>();
                Posts = new ObservableCollection<string>();
            }
        }

        public ModelPersons(List<Person> persons)
        {
            AllPersons = persons;
            PostUpdate();
        }

        public ModelPersons()
        {
            PostFilter = "";
            SalaryFilter = 0;
            SortUp = false;
            Posts = new ObservableCollection<string>();
            FilterPersons = new ObservableCollection<Person>();
            DataContractJsonSerializer jsonFormatter = new DataContractJsonSerializer(typeof(List<Person>));
            using (FileStream fs = new FileStream("AllPersons.json", FileMode.Open))
            {
                AllPersons = (List<Person>)jsonFormatter.ReadObject(fs);
            }
            PostUpdate();

            /*
            AllPersons = new List<Person>();
            AddPerson(new Person("Иван Максимов", "Кассир", 15000, 2.5));
            AddPerson(new Person("Митрофан Боголюбов", "Менеджер", 35000, 3.6));
            AddPerson(new Person("Виталий Костенко", "Кассир", 16000, 1.9));
            AddPerson(new Person("Максим Романов", "Кассир", 20000, 5.4));//*/
            ApplyFilter();
        }

        private void PostUpdate()
        {
            foreach (var p in AllPersons)
            {
                if (Posts.IndexOf(p.Post) < 0)
                    Posts.Add(p.Post);
            }
        }

        ~ModelPersons()
        {
            DataContractJsonSerializer jsonFormatter = new DataContractJsonSerializer(typeof(List<Person>));
            using (FileStream fs = new FileStream("AllPersons.json", FileMode.Create))
            {
                jsonFormatter.WriteObject(fs, AllPersons);
            }
        }


        public void AddPerson(Person person)
        {
            if (AllPersons.IndexOf(person) < 0)
                AllPersons.Add(person);
            if (Posts.IndexOf(person.Post) < 0)
                Posts.Add(person.Post);
            
            AllPersons.Sort(ComparePerson);
        }

        private int ComparePerson(Person A, Person B)
        {
            var resutl = A.Salary.CompareTo(B.Salary);
            return (SortUp)? resutl : resutl*(-1);

        }

        // События
        public void ApplyFilter()
        {
            AllPersons.Sort(ComparePerson);
            var salary = SalaryFilter;
            if (salary == 0)
                salary = double.MinValue;
            FilterPersons.Clear();
            if (PostFilter == ""|| PostFilter == null)
            {
                foreach (var p in AllPersons)
                {
                    if (salary <= p.Salary)
                        FilterPersons.Add(p);
                }
            }
            else
            {

                foreach (var p in AllPersons)
                {
                    if (p.Post == PostFilter && salary <= p.Salary)
                        FilterPersons.Add(p);
                }
            }            
        }



    }
}
