using GPSTask;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace GPSTaskTest
{
    [TestClass]
    public class DataProcessingTest
    {
        [TestMethod]
        public void TestMethod_GetRegionPoint_0()
        {
            List<HCircle> circles = new List<HCircle>();
            circles.Add(new HCircle(new HPoint(0, 10), 1));
            circles.Add(new HCircle(new HPoint(-5.4, -7.5), 1));
            circles.Add(new HCircle(new HPoint(6.21, -8), 1));

            HTime time = new HTime();
            time.AddTime(0.00001716);
            time.AddTime(0.00000583);
            time.AddTime(0.00001694);

            HPoint expected = new HPoint(-9.85427700, -3.30208200);

            var region = DataProcessing.GetRegionPoint(time, circles);
            DataProcessing.Checking(region, circles);
            HPoint actual = CenterOfMass.Averaging(region);
            
            Assert.AreEqual(expected.X, actual.X, 0.0000001, $"({actual.X},{actual.Y})");
            Assert.AreEqual(expected.Y, actual.Y, 0.0000001, $"({actual.X},{actual.Y})");
        }
    }
}
