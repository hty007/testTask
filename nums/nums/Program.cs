using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Collections;
using System.Text;
using System.Diagnostics;

namespace nums
{
    class Program
    {
        static double[] data = new double[] {100, 1,4,66, 30 , 7.6, 500, 8.55,3,4 };       


        static void Main(string[] args)
        {
            BinaryNumber bn = new BinaryNumber();
            LoadData();
            var l_data = new List<double>(data);
            AlgoritmA algoritm = new AlgoritmA(l_data);
            //double target = 500.2;
            //double target = 4674.32;
            double target = 10360.42;
            Stopwatch watch = new Stopwatch();

            // Выполнение алгоритма
            watch.Start();
            double[] items = algoritm.Run(target);
            watch.Stop();
            
            
            if (items == null)
            {
                Console.WriteLine("Нет подходящей комбинации");
            }
            else
            {
                Console.WriteLine("Вывожу ответ:");
                foreach (var item in items)
                {
                    int index = l_data.IndexOf(item);
                    Console.Write($"{index}({item})  ");
                }
            }
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.Write(target.ToString("000.00"));
            Console.Write(":\t");
            Console.Write(Sum(items));

            Console.WriteLine();
            Console.WriteLine($"Время выполнения алгоритма: {watch.ElapsedMilliseconds} мс");
            Console.WriteLine($"Время выполнения алгоритма: {watch.ElapsedTicks} тиках");
            Console.ReadLine();
        }

        private static double Sum(double[] items)
        {
            double sum = 0;
            for (int i = 0; i < items.Length; i++)
            {                
                sum += items[i];
            }
            return sum;
        }

        static void LoadData()
        {
            using (StreamReader sr = new StreamReader("nums.txt"))
            {
                var d =new  List<double>();
                while (!sr.EndOfStream)
                {
                    var b = sr.ReadLine();
                    d.Add(double.Parse(b, System.Globalization.CultureInfo.InvariantCulture));
                }
                data = d.ToArray();
            }
        }
        static double Summ( IEnumerable<int> nums)
        {
            double sum = 0;
            foreach (int index in nums)
            {
                sum += data[index];
            }
            return sum;
        }
        static double Summ(bool[] thy)
        {
            double sum = 0;
            for (int i = 0; i < thy.Length; i++)
            {
                if (thy[i])
                    sum += data[i];
            }
            return sum;
        }
        static string Answer(BinaryNumber bn)
        {
            var res = new StringBuilder();
            var l = bn.GetList();
            for (int i = 0; i < l.Count; i++)
                if (l[i])
                {
                    if (res.Length!=0)
                        res.Append(' ');
                    res.Append(i.ToString());
                }
            return res.ToString();
        }
    }
}
