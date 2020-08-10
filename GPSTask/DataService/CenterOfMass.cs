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
}