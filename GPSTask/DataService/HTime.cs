using System.Collections.Generic;

namespace GPSTask
{
    internal class HTime
    {
        List<double> times = new List<double>();

        public int Count { get => times.Count; }

        public double GetTime(int index) => times[index];

        public void AddTime(double time)
        {
            times.Add(time);
        }
    }
}