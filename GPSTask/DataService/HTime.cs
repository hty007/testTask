using System.Collections.Generic;

namespace GPSTask
{
    internal class HTime
    {
        List<double> times = new List<double>();

        public void AddTime(double time)
        {
            times.Add(time);
        }
    }
}