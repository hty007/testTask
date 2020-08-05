using System.Collections.Generic;
/*
https://e-maxx.ru/algo/circles_intersection
https://e-maxx.ru/algo/circle_line_intersection
*/
namespace GPSTask
{
    internal class DataProcessing
    {
        private List<HPoint> Sourses;
        private List<HTime> Times;
        private List<HCircle> circles;
        private List<HPoint> Trajectory;

        public const double SIGNAL_SPEED = 1000000;/* м/с */

        public DataProcessing(DataReader dataReader)
        {
            Sourses = dataReader.GetSourses();
            Times = dataReader.GetTimes();
        }

        public void Processing() 
        {
            // Создаем единичные окружности 
            foreach (HPoint point in Sourses)
            {
                HCircle circle = new HCircle(point, 1);
                circles.Add(circle);
            }

            List<HPoint> region = new List<HPoint>();//
            HTime time = Times[0];
            for (int i = 0; i < time.Count; i++)
            {
                HCircle c_i = circles[i];
                c_i.SetRadius(SIGNAL_SPEED * time.GetTime(i));
               
                for (int j = i+1; j < time.Count; j++)
                {
                    HCircle c_j = circles[j];
                    c_j.SetRadius(SIGNAL_SPEED * time.GetTime(j));
                    region.AddRange(c_i.IntersectingPoint(c_j));
                }
            }

        }
    }
}