using System;
using System.Collections.Generic;
using System.Text;

namespace nums
{
    public class BinaryNumber
    {
        private int data;
        public int Number { get => data; set => data = value; }
        public bool this[int digit]
        {
            get
            {
                var x = GetList();
                return x[digit];
            }
        }
        public int Count { get{
                int digit = 0;int num = 1;
                while (Number > num)
                {
                    digit += 1;
                    num *= 2;
                }
                return digit;
            } }

        private int Pow(int digit)
        {
            return (int)Math.Round(Math.Pow(2, digit));
        }

        public override string ToString() => ToString(-1);

        public  string ToString(int v=-1)
        {            
            var buf = GetList(v);
            StringBuilder res = new StringBuilder();
            foreach (var b in buf)
            {
                res.Append(b ? 1 : 0);
            }
            return res.ToString();
        }

        public List<bool> GetList(int length = -1)
        {

            var buf = new List<bool>();
            int num = Number;
            do
            {
                int digit = num%2;
                num /= 2;
                buf.Add(digit == 1);
            } while (num!=0);
            if (length != -1)
                while(buf.Count != length)
                    buf.Add(false);
            buf.Reverse();return buf;
        }
    }
}
