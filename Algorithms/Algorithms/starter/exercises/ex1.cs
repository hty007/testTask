using starter;
using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace sterter.exercises
{
    public class Exercise1 : SubCmd, ICmd
    {
        #region Интерфейст ICmd
        public string GetHelp()
        {
            return "Решение упражнения 1.7";
        }

        public string[] GetName()
        {
            return new[] { "examp1","e1" };
        }
        #endregion
        public Exercise1()
        {
            Hеader = "Упражнение 1.7";
            AddSubCommand("ex1", "Пример 1. Докажите что значение <a+b> может быть больше, чем значение min(a,b).", example1);
            AddSubCommand("ex2", "Пример 2. Докажите что значение <a*b> может быть больше, чем значение min(a,b).", example2);
        }

        private void example1(string obj)
        {
            #region Заглавие и параметры
            Console.WriteLine("Упражнение 1.7 Пример 1");
            Console.WriteLine("Докажите что значение <a+b> может быть больше, чем значение min(a,b).");

            int min = -10;
            int max = 100;

            string[] a_str = obj.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries); 
            #endregion
            #region Изменение min и max 
            if (a_str.Length == 2)
            {
                if (int.TryParse(a_str[0], out int a) && int.TryParse(a_str[1], out int b))
                {
                    min = Math.Min(a, b);
                    max = Math.Max(a, b);
                }
            } 
            #endregion
            #region Проверка двух чисел
            if (a_str.Length == 3)
            {
                if (int.TryParse(a_str[1], out int a) && int.TryParse(a_str[2], out int b))
                {
                    Console.Write($"Проверяю <{a}> и <{b}>");
                    if (a + b < Math.Min(a, b))
                    {
                        Console.WriteLine($"--- подходят под условия");
                    }
                    else
                    {
                        Console.WriteLine($"--- не подходят под условия");
                    }
                    return;
                }
            }
            #endregion
            #region Выполнение поиска
            Console.WriteLine("Выполняю...");
            int find = 0;

            for (int a = min; a <= max; a++)
            {
                for (int b = min; b <= max; b++)
                {
                    if (a + b < Math.Min(a, b))
                    {
                        find++;
                        Console.WriteLine($"---Найдено <{a}> и <{b}>");
                    }
                }
            }
            if (find == 0) Console.WriteLine("Ничего не найдено"); 
            #endregion
        }
        private void example2(string obj)
        {
            #region Заглавие и параметры
            Console.WriteLine("Упражнение 1.7 Пример 2");
            Console.WriteLine("Докажите что значение <a*b> может быть больше, чем значение min(a,b).");

            int min = -5;
            int max = 10;

            string[] a_str = obj.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            #endregion
            #region Изменение min и max 
            if (a_str.Length == 2)
            {
                if (int.TryParse(a_str[0], out int a) && int.TryParse(a_str[1], out int b))
                {
                    min = Math.Min(a, b);
                    max = Math.Max(a, b);
                }
            }
            #endregion
            #region Проверка двух чисел
            if (a_str.Length == 3)
            {
                if (int.TryParse(a_str[1], out int a) && int.TryParse(a_str[2], out int b))
                {
                    Console.Write($"Проверяю <{a}> и <{b}>");
                    Thread.Sleep(2000);
                    if (Check_ex2(a, b))
                    {
                        Console.WriteLine($"--- подходят под условия");
                    }
                    else
                    {
                        Console.WriteLine($"--- не подходят под условия");
                    }
                    return;
                }

            }
            #endregion
            #region Выполнение поиска
            Console.WriteLine("Выполняю...");
            int find = 0;

            for (int a = min; a <= max; a++)
            {
                for (int b = min; b <= max; b++)
                {
                    if (Check_ex2(a, b))
                    {
                        find++;
                        Console.WriteLine($"---Найдено <{a}> и <{b}>");
                    }
                }
            }
            if (find == 0) Console.WriteLine("Ничего не найдено");
            #endregion
        }

        private static bool Check_ex2(int a, int b)
        {
            return a * b < Math.Min(a, b);
        }
    }

}
