using System;
using System.Windows;

namespace GPSTask
{
    public class HPoint
    {
        #region Поля и свойства
        public double X { get; private set; }
        public double Y { get; private set; }
        #endregion
        #region Определение расстояние между точками
        public double GetDistance(HPoint point)
        {
            return Distance(this, point);
        }

        public static double Distance(HPoint a, HPoint b)
        {
            var dx = b.X - a.X;
            var dy = b.Y - a.Y;
            return Math.Sqrt(dx * dx + dy * dy);
        }
        #endregion
        #region ovveride
        public override string ToString()
        {
            return string.Format("{0}, {1}", X.ToString("0.##########").Replace(',', '.'), Y.ToString("0.##########").Replace(',', '.'));
        }

        public override bool Equals(object obj)
        {// Добавил для тестов
            if (obj is HPoint point)
            {
                if (point.X == X && point.Y == Y)
                    return true;
            }
            return false;
        }


        public bool Equals(object obj, double delta)
        {// Добавил для тестов
            if (obj is HPoint point)
            {
                if (Math.Abs(point.X - X) < delta && Math.Abs(point.Y - Y) < delta)
                    return true;
            }
            return false;
        }
        #endregion
        #region Конструкторы и другие форматы
        internal Point ToPoint()
        {
            return new Point(X, Y);
        }
        public HPoint Clone(double dx, double dy)
        {
            HPoint point = new HPoint(X + dx, Y + dy);
            return point;
        }

        public HPoint(double x, double y)
        {
            this.X = x;
            this.Y = y;
        } 
        #endregion
    }
}