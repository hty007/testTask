using System;
using System.Windows;

namespace GPSTask
{
    public class CoordinateHelper
    {
        /// <summary>
        /// Начало координат
        /// </summary>
        public static HPoint Zero { get; set; }
        /// <summary>
        /// Цена деления (Масштаб)
        /// </summary>
        public static double Scale { get; internal set; }

        internal static Point Convert(double x, double y)
        {
            return Zero.Clone(x * Scale, -y * Scale).ToPoint();
        }
        internal static Point Convert(HPoint point)
        {
            return Zero.Clone(point.X * Scale, -point.Y * Scale).ToPoint();
        }

        internal static HPoint BackConvert(Point p)
        {
            HPoint point = new HPoint((p.X-Zero.X)/Scale, (Zero.Y-p.Y)/Scale);
            return point;
        }

        internal static HPoint BackConvert(double x, double y)
        {
            HPoint point = new HPoint((x - Zero.X) / Scale, (Zero.Y - y) / Scale);
            return point;
        }
    }
}