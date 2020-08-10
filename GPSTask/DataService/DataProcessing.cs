using System;
using System.Collections.Generic;
/*
https://e-maxx.ru/algo/circles_intersection
https://e-maxx.ru/algo/circle_line_intersection
*/
namespace GPSTask
{
    public class DataProcessing
    {
        // Я всегда думал что скорость распространения радио сигнала 3*10^8(м/c), может эта задачи из паралельной вселенной? 
        public const double SIGNAL_SPEED = 1000000;/* м/с */
        //public const bool USE_INACCURACY = true; /* м/с */

        #region Поля и свойства
        private List<HPoint> Sourses;
        private List<HTime> Times;
        private List<HCircle> Circles;
        private List<HPoint> Trajectory;

        public List<HPoint> GetTrajectory() => Trajectory;
        public List<HPoint> GetSourses() => Sourses;
        public void SetSourses(List<HPoint> points) => Sourses = points;
        public void SetTrajectory(List<HPoint> trajectory) => Trajectory = trajectory;
        internal List<HTime> GetTimes() => Times;
        #endregion
        #region Публичные методы для работы
        public void Processing()
        {
            Circles = new List<HCircle>();
            // Создаем единичные окружности 
            foreach (HPoint point in Sourses)
            {
                HCircle circle = new HCircle(point, 1);
                Circles.Add(circle);
            }

            Trajectory = new List<HPoint>();
            foreach (HTime time in Times)
            {
                #region Подсчет одной точки   
                List<HPoint> region = GetRegionPoint(time, Circles, 0);
                Checking(region, Circles, 2.5);
                if (region.Count == 0 || region.Count == 1)
                {
                    region = GetRegionPoint(time, Circles, 4);// Увеличиваем Погрешность до 4%
                    Checking(region, Circles, 5); // Увеличиваем погрешность до 5%
                }
                HPoint newPoint = CenterOfMass.Averaging(region);// Просто находим центр масс (среднее короче)
                Trajectory.Add(newPoint);
                #endregion
            }
        }
        public void CalculateTimes()
        {
            Times = new List<HTime>();
            foreach (HPoint point in Trajectory)
            {
                HTime timePoint = new HTime();
                foreach (var sourse in Sourses)
                {
                    double d = point.GetDistance(sourse);// Дистанция
                    double time = d / SIGNAL_SPEED; // Время путь на время (с)/Секунды/
                    timePoint.AddTime(time);
                }
                Times.Add(timePoint);
            }
        } 
        #endregion
        #region Внутренние методы, с модификатором public из-за участия в тестах
        public static void Checking(List<HPoint> region, List<HCircle> circles, double inaccuracy = 0)
        {
            // Проверка и отсев 
            int index = region.Count;
            while (index >= 0)
            {
                if (index == region.Count) index = region.Count - 1;

                HPoint point = region[index];
                if (double.IsNaN(point.X) || double.IsNaN(point.Y))
                {
                    region.RemoveAt(index);
                    continue;
                }


                foreach (HCircle circle in circles)
                {
                    if (!circle.Contains(point, inaccuracy))
                    {
                        region.RemoveAt(index);
                        //index++;
                        break;
                    }
                }
                index--;
            }
        }

        public static List<HPoint> GetRegionPoint(HTime time, List<HCircle> circles, double inaccuracy = 2.5)
        {
            List<HPoint> region = new List<HPoint>();// Буфер с возможными точками

            for (int i = 0; i < time.Count; i++)
            {
                HCircle c_i = circles[i];
                c_i.SetRadius(SIGNAL_SPEED * time.GetTime(i), inaccuracy);

                for (int j = i + 1; j < time.Count; j++)
                {
                    HCircle c_j = circles[j];
                    // Устанавливаем радиусы в метрах (скорость на время)
                    //if (USE_INACCURACY)
                    c_j.SetRadius(SIGNAL_SPEED * time.GetTime(j), inaccuracy);
                    //else
                    //    c_j.SetRadius(SIGNAL_SPEED * time.GetTime(j), 0);
                    region.AddRange(c_i.IntersectingPoint(c_j));
                }
            }

            return region;
        } 
        #endregion

        public DataProcessing(DataFileHelper dataReader)
        {
            Sourses = dataReader.GetSourses();
            Times = dataReader.GetTimes();
        }

        public DataProcessing()
        {
        }
    }
}