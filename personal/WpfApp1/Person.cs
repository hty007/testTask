using System;
using System.Runtime.Serialization;

namespace WpfApp1
{
    [DataContract]
    public 
        class Person : IComparable
    {
        private string name;
        private string post;
        private double salary;
        private double experienceYears;

        [DataMember]
        public string Name { get => name; set => name = value; }
        [DataMember]
        public string Post { get => post; set => post = value; }
        [DataMember]
        public double Salary { get => salary; set => salary = value; }
        [DataMember]
        public double ExperienceYears { get => experienceYears; set => experienceYears = value; }

        public Person(string name, string post, double salary, double experienceYears)
        {
            Name = name;
            Post = post;
            Salary = Math.Round(salary,2);
            ExperienceYears = Math.Round(experienceYears,1);
        }

        private string ToTable(object o, int c)
        {
            string result = o.ToString();
            for (int i = result.Length; i < c; i++)
                result += " ";
            return result;
        }

        public override string ToString()
        {
            //int sep = 30;
            return ToTable(Name, 30) + ToTable(Post, 30) + ToTable(Math.Round(Salary,1), 10) + ToTable(Math.Round( ExperienceYears,2), 10);
        }



        #region Реализация интерфейсов
        
        public int CompareTo(object obj)
        {
            var person = (Person)obj;
            return this.CompareTo(person);
        }//*/
        public int CompareTo(Person A)
        {
            return salary.CompareTo(A.Salary);
        }

        #endregion
    }

}
/*
 Post должность
 Salary зарплата
 ExperienceYears Стаж
*/
