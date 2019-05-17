using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            ModelPersons persons = new ModelPersons(true);

            var sunames = new[] { "Абрамов", "Максимов", "Липатов", "Андрюшин", "Максименко", "Ведерноков", "Васильев" };
            var names = new[] { "Максим", "Альберт", "Вадим", "Семён", "Метрофан", "Арксений"};
            var posts = new[] { "уборшик", "кассир", "кладовщик", "директор", "зам. директор", "ген. дирекрор", "зав. производством", "упаковщик", "разнорабочий", "водитель" };
            int maxSalary = 1000;
            int minSalary = 100;

            Random random = new Random();
            for (int i = 0; i < sunames.Length; i++)
                for (int j = 0; j < names.Length; j++)
                {
                    var name = sunames[i] + " " + names[j];
                    double salary = random.Next(minSalary, maxSalary)* minSalary;
                    var post = posts[random.Next(posts.Length)];
                    var experienceYears = random.Next(500) * 0.1;
                    persons.AddPerson(new Person(name, post, salary, experienceYears));
                }

            Console.WriteLine("Создано {0} записей", persons.AllPersons.Count);
            Console.ReadKey();


        }
    }
}
