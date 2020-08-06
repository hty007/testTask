using System;

namespace GPSTask
{
    public class HVector
    {
        public HPoint Point { get; private set; }

        public double X { get => Point.X; }
        public double Y { get => Point.Y; }

        public HVector(double x, double y)
        {
            Point = new HPoint(x, y);

        }
        /// <summary>
        /// Вращение вектора
        /// https://vunivere.ru/work39751/page2
        /// </summary>
        /// <param name="angle"></param>
        public void Rotation(double angle)
        {
            //if (angle >= 0)
            //{
                double newX = X * Math.Cos(angle) + Y * Math.Sin(angle);
                double newY = X * (-Math.Sin(angle)) + Y * Math.Cos(angle);
                Point = new HPoint(newX, newY);
            //}
            //else
            //{
            //    double newX = X * Math.Cos(angle) - Y * Math.Sin(angle);
            //    double newY = X * ( Math.Sin(angle)) + Y * Math.Cos(angle);
            //    Point = new HPoint(newX, newY);
            //}


        }

        public void Multiplication(double quotient)
        {
            double newX = quotient * X;
            double newY = quotient * Y;

            Point = new HPoint(newX, newY);
        }

        public override string ToString()
        {
            return $"{X:f3},{Y:f3}";
        }
    }
}