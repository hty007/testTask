namespace GPSTask
{
    internal class HPoint
    {
        public double X { get; private set; }
        public double Y { get; private set; }

        public HPoint(double x, double y)
        {
            this.X = x;
            this.Y = y;
        }
    }
}