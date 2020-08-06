using System.Collections.Generic;
/*
https://e-maxx.ru/algo/circles_intersection
https://e-maxx.ru/algo/circle_line_intersection
*/
namespace GPSTask
{
    public class DataProcessing
    {
        private List<HPoint> Sourses;
        private List<HTime> Times;
        private List<HCircle> Circles;
        private List<HPoint> Trajectory;

        public void SetSourses(List<HPoint> points) => Sourses = points;

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
                Circles.Add(circle);
            }
            foreach (HTime time in Times)
            {
                #region Подсчет одной точки                
                List<HPoint> region = GetRegionPoint(time, Circles);
                Checking(region, Circles);
                HPoint newPoint = CenterOfMass.Averaging(region);
                Trajectory.Add(newPoint);
                #endregion
            }
        }

        public static void Checking(List<HPoint> region, List<HCircle> circles)
        {
            // Проверка и отсев 
            int index = region.Count;
            while (index >= 0)
            {
                index--;

                HPoint point = region[index];
                foreach (HCircle circle in circles)
                {
                    if (!circle.Contains(point))
                    {
                        region.Remove(point);
                        break;
                    }
                }
            }
        }

        public static List<HPoint> GetRegionPoint(HTime time, List<HCircle> circles)
        {
            List<HPoint> region = new List<HPoint>();// Буфер с возможными точками
            
            for (int i = 0; i < time.Count; i++)
            {
                HCircle c_i = circles[i];
                c_i.SetRadius(SIGNAL_SPEED * time.GetTime(i));

                for (int j = i + 1; j < time.Count; j++)
                {
                    HCircle c_j = circles[j];
                    // Устанавливаем радиусы в метрах (скорость на время)
                    c_j.SetRadius(SIGNAL_SPEED * time.GetTime(j));
                    region.AddRange(c_i.IntersectingPoint(c_j));
                }
            }

            return region;
        }
    }
}