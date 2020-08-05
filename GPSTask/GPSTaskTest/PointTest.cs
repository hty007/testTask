using GPSTask;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GPSTaskTest
{


    [TestClass]
    public class PointTest
    {
        [DataRow(3, 0, 0, 0, 3)]
        [DataRow(0, 4, 0, 0, 4)]
        [DataRow(0, 0, 2, 0, 2)]
        [DataRow(0, 0, 0, 5, 5)]
        [DataRow(3, 0, 4, 0, 1)]
        [DataRow(0, 3, 4, 0, 5)]
        [DataRow(0, 3, 0, 4, 1)]
        [DataRow(3, 0, 0, 4, 5)]
        [TestMethod]
        public void TestMethod_Distance_0_1(double x1, double y1, double x2, double y2, double expected)
        {            
            HPoint p1 = new HPoint(x1, y1);
            HPoint p2 = new HPoint(x2, y2);

            double result = p1.GetDistance(p2);

            Assert.AreEqual(expected, result);
        }


        [TestMethod]
        public void TestMethod_Distance()
        {
            // Одна точка пересечения
            HPoint p1 = new HPoint(0, 3);
            HPoint p2 = new HPoint(4, 0);

            double result = p1.GetDistance(p2);

            Assert.AreEqual(5, result);            
        }
    }
}
