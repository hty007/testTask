using starter;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sterter.exercises
{
    public class Exercise1 : SubCmd ,ICmd 
    {
        

        public Exercise1()
        {
            Hеader = "Упражнение 1.7";
            AddSubCommand("ex1", "Пример 1", example1);
        }

        private void example1()
        {
            throw new NotImplementedException();
        }



        //public void Execute(params string[] pars)
        //{
        //    Console.WriteLine("\t===== Упражнения 1.7 =====");

        //    while (work)
        //    {
        //        Console.Write("Введите команду:");
        //        str = Console.ReadLine();
        //    }
        //}

        public string GetHelp()
        {
            return "Решение упражнения 1.7";
        }

        public string[] GetName()
        {
            return new[] { "ex1" };
        }
    }

}
