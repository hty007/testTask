using System;
using System.Collections.Generic;

namespace nums
{
    internal class AlgoritmA
    {        
        private List<double> Data;
        

        public AlgoritmA(List<double> data)
        {
            this.Data = data;
        }

        internal double[] Run(double target)
        {
            Data.Sort();
            Data.Reverse();
            int count = Data.Count;
            Queue<Variation> variations = new Queue<Variation>();
            variations.Enqueue(new Variation(count, target));
            while (true)
            {
                if (variations.Count == 0) 
                    return null;
                Variation item = variations.Peek();
                if (item.Index < count-1)
                {
                    Variation clone = item.Clone();
                    clone.Index++;
                    variations.Enqueue(clone);
                }
                //if (item.Mask[32])
                //{
                //    Console.WriteLine($"32:{item.Index}({item.Remnant})");
                //}
                //if (item.Mask[32] && item.Mask[54])
                //{
                //    Console.WriteLine($"32,54:{item.Index}({item.Remnant})");
                //}

                item.Remnant -= Data[item.Index];
                item.Mask[item.Index] = true;

                if (Math.Abs(item.Remnant) < 0.000001)// Почти ноль это ноль
                {
                    return Answer(item.Mask);
                }
                else if (item.Remnant > 0)
                { 
                    if (item.Index == count-1)
                        variations.Dequeue();
                    else
                        item.Index++;
                }
                else if (item.Remnant < 0)
                {
                    variations.Dequeue();
                }
            }
            
        }

        private double[] Answer(List<bool> mask)
        {
            List<double> result = new List<double>();
            for (int i = 0; i < mask.Count; i++)
            {
                if (mask[i])
                {
                    result.Add(Data[i]);
                } 
            }
            return result.ToArray();
        }

        class Variation 
        {
            public List<bool> Mask;
            public int Index;
            public double Remnant;

            public Variation()
            {
            }

            public Variation(int count, double target)
            {
                Mask = new List<bool>(new bool[count]);

                Index = 0;
                Remnant = target;
            }

            public Variation Clone()
            {
                Variation variation = new Variation(Mask.Count, Remnant);
                variation.Index = Index; 
                for (int i = 0; i < Mask.Count; i++)
                {
                    if (i > Index) break;
                    variation.Mask[i] = Mask[i]; 
                }
                return variation;
            }
        }
    }
}