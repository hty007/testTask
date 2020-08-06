using System;

namespace GPSTask
{
    public class HPoint
    {
        public double X { get; private set; }
        public double Y { get; private set; }

        public HPoint(double x, double y)
        {
            this.X = x;
            this.Y = y;
        }

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

        public override string ToString()
        {
            return $"({X:f}, {Y:f})";
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
                if (Math.Abs(point.X - X)<delta && Math.Abs(point.Y - Y) < delta)
                    return true;
            }
            return false;
        }
    }
}