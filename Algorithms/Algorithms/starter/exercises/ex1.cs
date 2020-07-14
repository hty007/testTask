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
        private const string EXAMPLE1_TEXT = "Пример 1.\nДокажите что значение <a+b> может быть больше, чем значение min(a,b).";
        private const string EXAMPLE2_TEXT = "Пример 2.\nДокажите что значение <a*b> может быть больше, чем значение min(a,b).";
        private const string EXAMPLE3_TEXT = "Пример 3.\nНачертите сеть дорог с двумя точками а и b, такими, что маршрут между ними, преодолеваемый за кратчайшее время, не является самым коротким.";
        private const string EXAMPLE4_TEXT = "Пример 4.\nНачертите сеть дорог с двумя точками а и b, самый короткий маршрут между которыми не является маршрутом с наименьшим количеством поворотов.";
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
            AddSubCommand("ex1", EXAMPLE1_TEXT, example1);
            AddSubCommand("ex2", EXAMPLE2_TEXT, example2);
            AddSubCommand("ex3", EXAMPLE3_TEXT, example3);
            AddSubCommand("ex4", EXAMPLE4_TEXT, example4);
        }

        private void example1(string obj)
        {
            #region Заглавие и параметры
            Console.WriteLine("Упражнение 1.7 "+ EXAMPLE1_TEXT);

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
            Console.WriteLine("Упражнение 1.7 " + EXAMPLE2_TEXT);

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
        private void example3(string obj)
        {
            //Начертите сеть дорог с двумя точками а и Ь, такими, что маршрут между ними, 
            //преодолеваемый за кратчайшее время, не является самым коротким.
            #region Заглавие и параметры
            Console.InputEncoding = Encoding.UTF8;
            Console.WriteLine("Упражнение 1.7 " + EXAMPLE3_TEXT);
            #endregion
            #region Ответ
            string answer = @"Данную сеть дорог можно реализовать в взвешанном однонаправленом графе:
A→→
↑ ↓
↑ ↓
B←←
Так в приведённом примере путь из 'А' в 'В' составить 6 условных единиц, а из 'В' в 'А' всего 2.
";
            Console.WriteLine(answer);            
            #endregion
        }
        private void example4(string obj)
        {
            //Начертите сеть дорог с двумя точками а и Ь, самый короткий маршрут между 
            //которыми не является маршрутом с наименьшим количеством поворотов.
            #region Заглавие и параметры
            Console.InputEncoding = Encoding.UTF8;
            Console.WriteLine("Упражнение 1.7 " + EXAMPLE4_TEXT);
            #endregion
            #region Ответ
            string answer = @"Данную сеть дорог можно реализовать в взвешанном однонаправленом графе:
  1   2  
A→→→→→→
↑ ↓→↓ ↓
↑   ↓ ↓
↑ ↓←↓ ↓
B←←←←←←
Так в приведённом примере путь из 'А' в 'В' при повороте на дорогу 1 составить 11 условных единиц и встретится 6 поворотов, 
а при повороте на дорогу 2 будет потрачено 15 условных едениц и встретится всего 2 поворота.
";
            Console.WriteLine(answer);
            #endregion
        }
    }

}
