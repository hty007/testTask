using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleStorage.Utility
{
    public static class ConsoleHelper
    {
        public static void WriteItemInfoCollect<T>(IEnumerable<T> collect, Func<T, string> format, string header = null, bool number = false)
        {
            if (header != null)
                Console.WriteLine($"{header}: ");
            if (collect != null)
            {
                if (number)
                {
                    int index = 0;

                    foreach (T item in collect)
                    {
                        Console.WriteLine($"{index++}: {format(item)}");
                    }
                }
                else
                {
                    foreach (var item in collect)
                    {
                        Console.WriteLine(format(item));
                    }
                }
            }
            else
            {
                Console.WriteLine("Коллекция отсутвует!");
            }

            Console.WriteLine(" -----");
        }

        public static bool QueryInt(string message, out int num)
        {
            Console.Write(message);
            var line = Console.ReadLine();
            if (int.TryParse(line, out num))
                return true;

            num = -1;
            return false;
        }

        public static string Query(string message)
        {
            Console.Write(message);
            return Console.ReadLine();
        }

        public static void WriteProperty(string name, object value)
        {
            Console.WriteLine($"{name}: {value}");
        }
    }
}
