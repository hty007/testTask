using GPSTask;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
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

            double delta = 0.00001716 * DataProcessing.SIGNAL_SPEED * 5 / 100;

            HPoint expected = new HPoint(-9.85427700, -3.30208200);

            var region = DataProcessing.GetRegionPoint(time, circles);
            DataProcessing.Checking(region, circles, 5);
            HPoint actual = CenterOfMass.Averaging(region);


            double error = actual.GetDistance(expected);

            Assert.IsTrue(error < delta, $"\nОшибка больше указанной погрешности!\n error ={error}");

            //Assert.AreEqual(expected.X, actual.X, 0.0000001, $"({actual.X},{actual.Y})");
            //Assert.AreEqual(expected.Y, actual.Y, 0.0000001, $"({actual.X},{actual.Y})");
        }

        [TestMethod]
        [DataRow(0.00001716, 0.00000583, 0.00001694, -9.85427700, -3.30208200)]
        [DataRow(0.00001539, 0.00000517, 0.00001558, -7.92068200, -2.74886900)]
        [DataRow(0.00001324, 0.00000524, 0.00001373, -5.78066500, -2.35031000)]
        [DataRow(0.00001320, 0.00000570, 0.00001243, -3.81985900, -1.98844000)]
        [DataRow(0.00001143, 0.00000682, 0.00001091, -1.94217300, -1.73413300)]
        [DataRow(0.00001063, 0.00000854, 0.00000889, -0.15681350, -0.89424750)]
        [DataRow(0.00001085, 0.00001054, 0.00000880, 2.13924600, -0.62065110)]
        [DataRow(0.00001093, 0.00001199, 0.00000863, 4.13878300, -0.12695850)]
        [DataRow(0.00001179, 0.00001335, 0.00000860, 5.81872400, 0.53746240)]
        [DataRow(0.00001214, 0.00001660, 0.00000938, 8.04749200, 1.01432800)]        
        public void TestMethod_GetRegionPoint_1(double time1, double time2, double time3,double x, double y)
        {
            List<HCircle> circles = new List<HCircle>();
            circles.Add(new HCircle(new HPoint(0, 10), 1));
            circles.Add(new HCircle(new HPoint(-5.4, -7.5), 1));
            circles.Add(new HCircle(new HPoint(6.21, -8), 1));

            HTime time = new HTime();
            time.AddTime(time1);
            time.AddTime(time2);
            time.AddTime(time3);

            double delta = Math.Max(Math.Max(time1,time2),time3) * DataProcessing.SIGNAL_SPEED * 5 / 100;
            //double delta = 1; // 1 метр

            HPoint expected = new HPoint(x, y);

            List<HPoint> region = DataProcessing.GetRegionPoint(time, circles, 0);
            DataProcessing.Checking(region, circles, 2.5);
            if (region.Count == 0 || region.Count == 1)
            {
                region = DataProcessing.GetRegionPoint(time, circles,5);
                DataProcessing.Checking(region, circles, 5);
            }
            

            //if (region.Count == 0)
            //{
            //    region = DataProcessing.GetRegionPoint(time, circles,5);
            //    //DataProcessing.Checking(region, circles);
            //}

            Assert.IsTrue(region.Count > 0, "\nВ регионе не осталось точек");
            HPoint actual = CenterOfMass.Averaging(region);
            double dx = actual.X - expected.X;
            double dy = actual.Y - expected.Y;
            double error = actual.GetDistance(expected);

            Assert.IsTrue(error < delta, $"Ошибка больше указанной погрешности!\ndelta={delta}\nerror={error}\ndx={dx}\ndy={dy}\nN={region.Count}");            
        }
    }
}
