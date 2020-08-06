using GPSTask;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace GPSTaskTest
{
    [TestClass]
    public class VectorTest
    {
        [TestMethod]
        public void TestMethod_Rotation_0()
        {
            HVector v1 = new HVector(5, 0);
            double angle = 0;

            v1.Rotation(angle);
            //v1.Rotation(-angle);

            Assert.AreEqual(0, v1.Y, 0.001, $"({v1.X},{v1.Y})");
            Assert.AreEqual(5, v1.X, 0.001, $"({v1.X},{v1.Y})");
        }

        [TestMethod]
        public void TestMethod_Rotation_1()
        {
            HVector v1 = new HVector(0, 5);
            int gradus = 90;
            double angle = Math.PI * gradus / 180;
            v1.Rotation(angle);//60

            Assert.AreEqual(5, v1.X, 0.001);
            Assert.AreEqual(0, v1.Y, 0.001);
        }

        [TestMethod]
        public void TestMethod_Rotation_2()
        {
            HVector v1 = new HVector(0, 5);
            int gradus = 45;
            double angle = Math.PI * gradus / 180;
            v1.Rotation(angle);
            Assert.IsTrue(v1.X == v1.Y, "Числа не ровны!");

            //Assert.AreEqual(4, v1.X, 0.001, $"X={v1.X}");
            //Assert.AreEqual(3, v1.Y, 0.001, $"Y={v1.Y}");
        }

        [TestMethod]
        public void TestMethod_Rotation_3()
        {
            HVector v1 = new HVector(0, 5);
            double angle = Math.Acos(4.0/5.0);            
            
            v1.Rotation(angle);

            Assert.AreEqual(3, v1.X, 0.001, $"X={v1.X}");
            Assert.AreEqual(4, v1.Y, 0.001, $"Y={v1.Y}");
        }

        [TestMethod]
        public void TestMethod_Rotation_4()
        {
            HVector v1 = new HVector(0, 5);
            double angle = Math.Acos(3.0 / 5.0);

            v1.Rotation(angle);

            Assert.AreEqual(4, v1.X, 0.001, $"X={v1.X}");
            Assert.AreEqual(3, v1.Y, 0.001, $"Y={v1.Y}");
        }

        [TestMethod]
        public void TestMethod_Rotation_5()
        {
            HVector v1 = new HVector(0, 5);
            double angle = Math.Acos(3.0 / 5.0);

            v1.Rotation(-angle);

            Assert.AreEqual(-4, v1.X, 0.001, $"X={v1.X}");
            Assert.AreEqual(3, v1.Y, 0.001, $"Y={v1.Y}");
        }

        [TestMethod]
        public void TestMethod_Rotation_6()
        {
            HVector v1 = new HVector(0, 5);
            double angle = 3;

            v1.Rotation(angle);
            v1.Rotation(- angle);

            Assert.AreEqual(0, v1.X, 0.001, $"X={v1.X}");
            Assert.AreEqual(5, v1.Y, 0.001, $"Y={v1.Y}");
        }

        [TestMethod]
        public void TestMethod_Rotation_7()
        {
            HVector v1 = new HVector(5, 0);
            double angle = Math.Acos(0.8); 

            v1.Rotation(angle);
            //v1.Rotation(-angle);

            Assert.AreEqual(4, v1.X, 0.001, $"X={v1.X}");
            Assert.AreEqual(-3, v1.Y, 0.001, $"Y={v1.Y}");
        }

       

        [TestMethod]
        public void TestMethod_Rotation_8()
        {
            HVector v1 = new HVector(5, 0);
            double angle = - Math.Acos(0.8);

            v1.Rotation(angle);
            //v1.Rotation(-angle);

            Assert.AreEqual(4, v1.X, 0.001, $"({v1.X},{v1.Y})");
            Assert.AreEqual(3, v1.Y, 0.001, $"({v1.X},{v1.Y})");
        }


        #region Multiplication
        [TestMethod]
        public void TestMethod_Multiplication_1()
        {
            HVector v1 = new HVector(0, 5);

            v1.Multiplication(2);

            Assert.AreEqual(0, v1.X, 0.001, $"X={v1.X}");
            Assert.AreEqual(10, v1.Y, 0.001, $"Y={v1.Y}");
        }

        [TestMethod]
        public void TestMethod_Multiplication_2()
        {
            HVector v1 = new HVector(2, 0);

            v1.Multiplication(2);

            Assert.AreEqual(4, v1.X, 0.001, $"X={v1.X}");
            Assert.AreEqual(0, v1.Y, 0.001, $"Y={v1.Y}");
        }

        [TestMethod]
        public void TestMethod_Multiplication_3()
        {
            HVector v1 = new HVector(1, 1);

            v1.Multiplication(5);

            Assert.AreEqual(5, v1.X, 0.001, $"X={v1.X}");
            Assert.AreEqual(5, v1.Y, 0.001, $"Y={v1.Y}");
        } 
        #endregion
    }
}
