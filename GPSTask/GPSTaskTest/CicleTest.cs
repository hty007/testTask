using GPSTask;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace GPSTaskTest
{
    [TestClass]
    public class CircleTest
    {
        [TestMethod]
        [DataRow(9, 11,   10, 11,   5)]
        [DataRow(9, 11,   9,  11,   4)]
        [DataRow(9, 10,   9,  11,   3)]
        [DataRow(9, 10.5, 9,  10.5, 2)]
        [DataRow(9, 10,   9,  10,   1)]
        [DataRow(9, 9.5 , 9,  9.5,  0)]
        public void TestMethod_IntersectingPointInaccuracy_2(double r11, double r12, double r21, double r22, int expected)
        {
            HPoint centr1 = new HPoint(5, 10);
            HCircle circle1 = new HCircle(centr1, 1);
            circle1.Radius1 = r11;
            circle1.Radius2 = r12;
            HPoint centr2 = new HPoint(25, 10);
            HCircle circle2 = new HCircle(centr2, 1);
            circle2.Radius1 = r21;
            circle2.Radius2 = r22;           

            HPoint[] actual = circle1.IntersectingPoint(circle2);

            Assert.AreEqual(expected, actual.Length, message: "Количества не совпадает!");            
        }


        [TestMethod]
        public void TestMethod_IntersectingPointInaccuracy_1()
        {
            HPoint centr1 = new HPoint(5, 10);
            HCircle circle1 = new HCircle(centr1, 11);
            circle1.Radius1 = 9;
            circle1.Radius2 = 11;
            HPoint centr2 = new HPoint(25, 10);
            HCircle circle2 = new HCircle(centr2, 11);
            circle2.Radius1 = 8; 
            circle2.Radius2 = 9;

            HPoint[] expected = new[] {
                //new HPoint(2, 7),
                new HPoint(16, 10)
            };


            //HPoint[] actual = HCircle.IntersectingPoint(centr1, radius1, centr2, radius2);
            HPoint[] actual = circle1.IntersectingPoint(circle2);

            Assert.AreEqual(expected.Length, actual.Length, message: "Количества не совпадает!");
            foreach (HPoint exp in expected)
            {
                bool find = false;
                foreach (HPoint act in actual)
                {
                    if (act.Equals(exp, 0.001))
                    {
                        find = true;
                    }
                }
                Assert.IsTrue(find, $"Точка [{exp.ToString()}] не найдена!");
            }
        }

        [TestMethod]
        public void TestMethod_IntersectingPoint_8()
        {
            // вертикально
            HPoint centr1 = new HPoint(5, 10);
            HPoint centr2 = new HPoint(25, 10);
            HPoint[] expected = new[] {
                //new HPoint(2, 7),
                new HPoint(16, 10)
            };
            double radius1 = 11; //Math.Sqrt(3 * 3 + 3 * 3);
            double radius2 = 9; //Math.Sqrt(5 * 5 + 3 * 3);

            HPoint[] actual = HCircle.IntersectingPoint(centr1, radius1, centr2, radius2);

            Assert.AreEqual(expected.Length, actual.Length, message: "Количества не совпадает!");
            foreach (HPoint exp in expected)
            {
                bool find = false;
                foreach (HPoint act in actual)
                {
                    if (act.Equals(exp, 0.001))
                    {
                        find = true;
                    }
                }
                Assert.IsTrue(find, $"Точка [{exp.ToString()}] не найдена!");
            }
        }

        [TestMethod]
        public void TestMethod_IntersectingPoint_9()
        {
            // вертикально
            HPoint centr1 = new HPoint(5, 10);
            HPoint centr2 = new HPoint(25, 10);
            HPoint[] expected = new[] {
                //new HPoint(2, 7),
                new HPoint(14, 10)
            };
            double radius1 = 9; //Math.Sqrt(3 * 3 + 3 * 3);
            double radius2 = 11; //Math.Sqrt(5 * 5 + 3 * 3);

            HPoint[] actual = HCircle.IntersectingPoint(centr1, radius1, centr2, radius2);

            Assert.AreEqual(expected.Length, actual.Length, message: "Количества не совпадает!");
            foreach (HPoint exp in expected)
            {
                bool find = false;
                foreach (HPoint act in actual)
                {
                    if (act.Equals(exp, 0.001))
                    {
                        find = true;
                    }
                }
                Assert.IsTrue(find, $"Точка [{exp.ToString()}] не найдена!");
            }
        }


        [TestMethod]
        public void TestMethod_IntersectingPoint_One()
        {
            // Одна точка пересечения
            HPoint centr1 = new HPoint(5, 5);
            HPoint centr2 = new HPoint(13, 5);

            HPoint[] result = HCircle.IntersectingPoint(centr1, 4, centr2, 4);

            Assert.AreEqual(1, result.Length);
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

            Assert.AreEqual(2, actual.Length, message: "Количества не совпадает!");

            foreach (HPoint exp in expected)
            {
                bool find = false;
                foreach (HPoint act in actual)
                {
                    if (act.Equals(exp, 0.001))
                    {
                        find = true;
                    }
                }
                Assert.IsTrue(find, $"Точка [{exp.ToString()}] не найдена!");
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
            foreach (HPoint exp in expected)
            {
                bool find = false;
                foreach (HPoint act in actual)
                {
                    if (act.Equals(exp, 0.001))
                    {
                        find = true;
                    }
                }
                Assert.IsTrue(find, $"Точка [{exp.ToString()}] не найдена!");
            }
        }

        [TestMethod]
        public void TestMethod_IntersectingPoint_3()
        {
            // вертикально
            HPoint centr1 = new HPoint(2, 2);
            HPoint centr2 = new HPoint(7, 7);
            HPoint[] expected = new[] {
                new HPoint(2, 7),
                new HPoint(7, 2)
            };
            double radius1 = 5;
            double radius2 = 5;
            //double radius1 = Math.Sqrt(3 * 3 + 3 * 3);
            //double radius2 = Math.Sqrt(5 * 5 + 3 * 3);

            HPoint[] actual = HCircle.IntersectingPoint(centr1, radius1, centr2, radius2);

            Assert.AreEqual(2, actual.Length, message: "Количества не совпадает!");
            foreach (HPoint exp in expected)
            {
                bool find = false;
                foreach (HPoint act in actual)
                {
                    if (act.Equals(exp, 0.001))
                    {
                        find = true;
                    }
                }
                Assert.IsTrue(find, $"Точка [{exp.ToString()}] не найдена!");
            }
        }

        [TestMethod]
        public void TestMethod_IntersectingPoint_4()
        {
            // вертикально
            HPoint centr1 = new HPoint(7, 7);
            HPoint centr2 = new HPoint(2, 2);
            HPoint[] expected = new[] {
                new HPoint(2, 7),
                new HPoint(7, 2)
            };
            double radius1 = 5;
            double radius2 = 5;
            //double radius1 = Math.Sqrt(3 * 3 + 3 * 3);
            //double radius2 = Math.Sqrt(5 * 5 + 3 * 3);

            HPoint[] actual = HCircle.IntersectingPoint(centr1, radius1, centr2, radius2);

            Assert.AreEqual(2, actual.Length, message: "Количества не совпадает!");
            foreach (HPoint exp in expected)
            {
                bool find = false;
                foreach (HPoint act in actual)
                {
                    if (act.Equals(exp, 0.001))
                    {
                        find = true;
                    }
                }
                Assert.IsTrue(find, $"Точка [{exp.ToString()}] не найдена!");
            }
        }

        [TestMethod]
        public void TestMethod_IntersectingPoint_5()
        {
            // вертикально
            HPoint centr1 = new HPoint(2, 7);
            HPoint centr2 = new HPoint(7, 2);
            HPoint[] expected = new[] {
                new HPoint(2, 2),
                new HPoint(7, 7)
            };
            double radius1 = 5;
            double radius2 = 5;
            //double radius1 = Math.Sqrt(3 * 3 + 3 * 3);
            //double radius2 = Math.Sqrt(5 * 5 + 3 * 3);

            HPoint[] actual = HCircle.IntersectingPoint(centr1, radius1, centr2, radius2);

            Assert.AreEqual(2, actual.Length, message: "Количества не совпадает!");
            foreach (HPoint exp in expected)
            {
                bool find = false;
                foreach (HPoint act in actual)
                {
                    if (act.Equals(exp, 0.001))
                    {
                        find = true;
                    }
                }
                Assert.IsTrue(find, $"Точка [{exp.ToString()}] не найдена!");
            }
        }

        [TestMethod]
        public void TestMethod_IntersectingPoint_6()
        {
            // вертикально
            HPoint centr1 = new HPoint(7, 2);
            HPoint centr2 = new HPoint(2, 7);
            HPoint[] expected = new[] {
                new HPoint(2, 2),
                new HPoint(7, 7)
            };
            double radius1 = 5;
            double radius2 = 5;
            //double radius1 = Math.Sqrt(3 * 3 + 3 * 3);
            //double radius2 = Math.Sqrt(5 * 5 + 3 * 3);

            HPoint[] actual = HCircle.IntersectingPoint(centr1, radius1, centr2, radius2);

            Assert.AreEqual(2, actual.Length, message: "Количества не совпадает!");
            foreach (HPoint exp in expected)
            {
                bool find = false;
                foreach (HPoint act in actual)
                {
                    if (act.Equals(exp, 0.001))
                    {
                        find = true;
                    }
                }
                Assert.IsTrue(find, $"Точка [{exp.ToString()}] не найдена!");
            }
        }

        [TestMethod]
        public void TestMethod_IntersectingPoint_7()
        {
            // вертикально
            HPoint centr1 = new HPoint(4, 6);
            HPoint centr2 = new HPoint(13, 6);
            HPoint[] expected = new[] {
                new HPoint(9, 3),
                new HPoint(9, 9)
            };
            //double radius1 = 5;
            //double radius2 = 5;
            double radius1 = Math.Sqrt(5 * 5 + 3 * 3);
            double radius2 = Math.Sqrt(4 * 4 + 3 * 3);

            HPoint[] actual = HCircle.IntersectingPoint(centr1, radius1, centr2, radius2);

            Assert.AreEqual(2, actual.Length, message: "Количества не совпадает!");
            foreach (HPoint exp in expected)
            {
                bool find = false;
                foreach (HPoint act in actual)
                {
                    if (act.Equals(exp, 0.001))
                    {
                        find = true;
                    }
                }
                Assert.IsTrue(find, $"Точка [{exp.ToString()}] не найдена!");
            }
        }
    }
}
