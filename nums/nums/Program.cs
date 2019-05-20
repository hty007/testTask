using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Collections;
using System.Text;

namespace nums
{
    class Program
    {
        static double[] data = new double[] {1,4,66,7.6, 500, 8.55,3,4 };
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
        


        static void Main(string[] args)
        {            
            BinaryNumber bn = new BinaryNumber();
            LoadData();
            /*
            for (int i = 250; i < 260; i++)
            {
                bn.Number = i;
                Console.WriteLine("{0}-{1}",
                    bn.ToString(),i);
            }/**/

            ///*
            /*
            Console.Write("Введите имкомую сумму:");
            double target = double.Parse(Console.ReadLine());/**/
            double target = 998.66;


            bn.Number = 0;
            double sum = 0;
            int max = (int)Math.Pow(2, data.Length)-1;
            while (sum != target&&bn.Number<max)
            {
                bn.Number++;
                sum = Summ(bn.GetList(data.Length).ToArray());
                Console.Write("Расмотрено {0}\r", bn.Number);
            }/**/


            if (bn.Number == max)
                Console.WriteLine("Подходящих вариантов не обнаружено!");
            else
            {
                Console.WriteLine("сумма {0} найдена!", target);
                Console.WriteLine("{0}", Answer(bn));

                var l = bn.GetList();
                for (int i = 0; i < l.Count; i++)
                    if (l[i])
                    {
                        Console.Write(data[i] + " ");
                    }
            }

            Console.WriteLine();
            Console.ReadLine();
        }
    }
}
