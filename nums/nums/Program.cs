using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace nums
{
    class Program
    {
        static double[] data;

        static int[] Enumeration(double target)
        {
            var buf = new List<int>();
			double sum = 0;
            while (sum != target)
			{
				if(sum<target)
				{
					
				}
				
			}
			return buf.ToArray();
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


        static void LoadData()
        {
            using (StreamReader sr = new StreamReader("nums.txt"))
            {
                var d =new  List<double>();
                while (sr.EndOfStream)
                {
                    d.Add(double.Parse(sr.ReadLine()));
                }
                data = d.ToArray();
            }
        }

        static void Main(string[] args)
        {

        }
    }
}
