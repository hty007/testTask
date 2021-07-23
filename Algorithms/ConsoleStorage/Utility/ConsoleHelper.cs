using System;
using System.Collections.Generic;
using System.Globalization;
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
            var line = ReadDigitsFromConsole();
            if (int.TryParse(line, out num))
                return true;

            num = -1;
            return false;
        }

        public static string ReadDigitsFromConsole()
        {
            string result = "";
            while (true)
            {
                var k = Console.ReadKey(true);
                switch (k.Key)
                {
                    case ConsoleKey.Backspace:
                        if (result.Length > 0)
                        {
                            result = result.Remove(startIndex: result.Length - 1, count: 1);
                            Console.Write(value: $"{k.KeyChar} {k.KeyChar}");
                        }
                        break;
                    case ConsoleKey.Enter:
                        Console.WriteLine();
                        return result;
                    case ConsoleKey.Decimal:
                        Console.Write(value: k.KeyChar);
                        result += k.KeyChar;
                        break;
                    case ConsoleKey.OemMinus:
                        Console.Write(value: k.KeyChar);
                        result += k.KeyChar;
                        break;
                    default:
                        if (char.IsDigit(c: k.KeyChar))
                        {
                            Console.Write(value: k.KeyChar);
                            result += k.KeyChar;
                        }
                        break;
                }
            }
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

        public static void WriteLineS(int length, string text)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Пишет сообщение красными буквами на белом фоне
        /// </summary>
        /// <param name="message">сообщение</param>
        public static void Error(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.BackgroundColor = ConsoleColor.White;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        public static bool QueryFloat(string message, out float half)
        {
            Console.Write(message);
            var line = ReadDigitsFromConsole();
            if (float.TryParse(line, NumberStyles.Any, CultureInfo.InvariantCulture, out half))
                return true;

            half = 0;
            return false;
        }
    }
}
