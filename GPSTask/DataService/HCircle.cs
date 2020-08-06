using System;
using System.Collections.Generic;

namespace GPSTask
{
    public class CenterOfMass
    {
        public static HPoint Averaging(List<HPoint> region)
        {
            double newX = 0, newY = 0;
            foreach (HPoint point in region)
            {
                newX += point.X;
                newY += point.Y;
            }
            return new HPoint(newX/region.Count, newY / region.Count);
        }
    }

    public class HCircle
    {
        public HPoint Center { get; internal set; }
        public double Radius1 { get; set; }// Из за погрешности два радиуса
        public double Radius2 { get; set; }// Из за погрешности два радиуса

        public HCircle(HPoint center, double radius1, double radius2 = 0)
        {
            Center = center;
            Radius1 = radius1;
            Radius2 = radius2;
        }

        public void SetRadius(double radius)
        {
            Radius1 = radius * (100 + 2.5) / 100;
            Radius2 = radius * (100 - 2.5) / 100;
        }

        /// <summary>
        /// Возвращает точки пересечения окружностей
        /// </summary>
        /// <param name="circle">вторая окрежность</param>
        /// <returns></returns>
        public HPoint[] IntersectingPoint(HCircle circle)
        {
            List<HPoint> result = new List<HPoint>();
            result.AddRange(IntersectingPoint(this.Center, Radius1, circle.Center, circle.Radius1));
            result.AddRange(IntersectingPoint(this.Center, Radius1, circle.Center, circle.Radius2));
            result.AddRange(IntersectingPoint(this.Center, Radius2, circle.Center, circle.Radius1));
            result.AddRange(IntersectingPoint(this.Center, Radius2, circle.Center, circle.Radius2));

            return result.ToArray();
        }

        internal bool Contains(HPoint point)
        {
            double d = Center.GetDistance(point);
            if (Math.Max(Radius1, Radius2) >= d
                && Math.Min(Radius1, Radius2) <= d)
                return true;

            return false;
        }


        /// <summary>
        /// Определяет точку пересечения между двумя окружностями
        /// </summary>
        /// <param name="center1"></param>
        /// <param name="radius11"></param>
        /// <param name="center2"></param>
        /// <param name="radius12"></param>
        /// <returns>Точки пересечения одну или две</returns>
        public static HPoint[] IntersectingPoint(HPoint center1, double radius1, HPoint center2, double radius2)
        {
            double d = center1.GetDistance(center2);
            if (d > radius1 + radius2) return new HPoint[] { };
            if (d == radius1 + radius2)  
            {
                double lambda = radius1 / radius2;
                double newX = (center1.X+lambda*center2.X) / (1+lambda);
                double newY = (center1.Y+lambda*center2.Y) / (1+lambda);

                return new[] { new HPoint(newX, newY) };
            }
            double cosA = (radius2 * radius2 - d * d - radius1 * radius1) / (2 * d * radius1);// Выведено из теоремы косинусаов
            
            double angle = (cosA<0)? 
                (-Math.Acos(Math.Abs(cosA))) 
                : Math.Acos(cosA);
            //double angle =  Math.Acos(cosA);
            
            
            //angle = angle + Math.PI;
            //angle = angle % Math.PI;

            HVector vector = new HVector(center2.X - center1.X, center2.Y - center1.Y);
            vector.Multiplication(radius1/d);
            vector.Rotation(angle);
            HPoint p1 = new HPoint(center1.X + vector.X, center1.Y + vector.Y);
            vector.Rotation(- 2*angle);
            HPoint p2 = new HPoint(center1.X + vector.X, center1.Y + vector.Y);

            //double newX = (radius1/d) * (center1.X + lambda * center2.X) / (1 + lambda);
            //double newY = (center1.Y + lambda * center2.Y) / (1 + lambda);

            return new[] { p1, p2 };
        }
    }
}