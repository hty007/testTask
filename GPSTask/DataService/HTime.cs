using System.Collections.Generic;

namespace GPSTask
{
    public class HTime
    {
        List<double> times = new List<double>();

        public int Count { get => times.Count; }

        public double GetTime(int index) => times[index];

        public void AddTime(double time)
        {
            times.Add(time);
        }

        public override string ToString()
        {
            string result = "";
            bool first = true;
            foreach (double t in times)
            {
                if (first)
                {
                    result += t.ToString("0.##########").Replace(',','.');
                    first = false;
                }
                else
                {
                    result +=", " + t.ToString("0.##########").Replace(',', '.');
                }
            }
            return result;
        }
    }
}