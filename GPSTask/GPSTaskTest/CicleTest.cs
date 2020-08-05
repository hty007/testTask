using GPSTask;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace GPSTaskTest
{
    [TestClass]
    public class CircleTest
    {
        [TestMethod]
        public void TestMethod_IntersectingPoint_One()
        {
            // Одна точка пересечения
            HPoint centr1 = new HPoint(5, 5);
            HPoint centr2 = new HPoint(13, 5);

            HPoint[] result = HCircle.IntersectingPoint(centr1, 4, centr2, 4);

            Assert.AreEqual(1,result.Length);
            var point1 = result[0];
            Assert.AreEqual(9, point1.X);
            Assert.AreEqual(5, point1.Y);
        }

        [TestMethod]
        public void TestMethod_IntersectingPoint_Zero()
        {
            // Одна точка пересечения
            HPoint centr1 = new HPoint(5, 5);
            HPoint centr2 = new HPoint(13, 5);

            HPoint[] result = HCircle.IntersectingPoint(centr1, 3, centr2, 3);

            Assert.AreEqual(0, result.Length);
            //var point1 = result[0];
            //Assert.AreEqual(9, point1.X);
            //Assert.AreEqual(5, point1.Y);
        }

        [TestMethod]
        public void TestMethod_IntersectingPoint_1()
        {
            // две точка пересечения
            HPoint centr1 = new HPoint(5, 5);
            HPoint centr2 = new HPoint(13, 5);
            HPoint[] expected = new[] { 
                new HPoint(9, 8), 
                new HPoint(9, 2) 
            };

            HPoint[] actual = HCircle.IntersectingPoint(centr1, 5, centr2, 5);

            Assert.AreEqual(2, actual.Length, message:"Количества не совпадает!");
            foreach (HPoint act in actual)
            {
                bool find = false;
                foreach (HPoint exp in expected)
                {
                    if (act.Equals(exp))
                    {
                        find = true;
                    }
                }
                Assert.IsTrue(find);
            }            
        }

        [TestMethod]
        public void TestMethod_IntersectingPoint_2()
        {
            // вертикально
            HPoint centr1 = new HPoint(5, 10);
            HPoint centr2 = new HPoint(5, 2);
            HPoint[] expected = new[] {
                new HPoint(2, 7),
                new HPoint(8, 7)
            };
            double radius1 = Math.Sqrt(3 * 3 + 3 * 3);
            double radius2 = Math.Sqrt(5 * 5 + 3 * 3);

            HPoint[] actual = HCircle.IntersectingPoint(centr1, radius1, centr2, radius2);

            Assert.AreEqual(2, actual.Length, message: "Количества не совпадает!");
            foreach (HPoint act in actual)
            {
                bool find = false;
                foreach (HPoint exp in expected)
                {
                    if (act.Equals(exp))
                    {
                        find = true;
                    }
                }
                Assert.IsTrue(find);
            }
        }
    }
}
