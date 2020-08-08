using System;
using System.Collections.Generic;
using System.IO;

namespace GPSTask
{
    public class DataFileHelper
    {
        private List<HPoint> Sourses = new List<HPoint>();
        private List<HTime> Times = new List<HTime>();
        private List<HPoint> Trajectory;

        public List<HPoint> GetSourses() => Sourses;
        public List<HTime> GetTimes() => Times;

        public string Message { get; private set; }

        internal bool ReadInputFile(string fileName)
        {
            try
            {
                StreamReader sr = new StreamReader(fileName);
                string line = sr.ReadLine();
                if (!TryParseSources(line))
                {
                    Message = "Формат координат приемников не распознан!\n"+Message;
                    return false;
                }

                while (! sr.EndOfStream)
                {
                    line = sr.ReadLine();
                    if (!TryParseTime(line))
                    {
                        Message = "Формат времени сигнала не распознан!\n" + Message;
                        return false;
                    }
                }

                return true;

            }
            catch (Exception ex)
            {
                Message = "Во время выполнения чтения из файла, возникла исключение! \n"+ex.Message;
                return false;               
            }            
        }

        internal void SetTimes(List<HTime> times)
        {
            Times = times;
        }

        internal bool FileInputWrite(string fileName)
        {
            try
            {
                if (Times == null) { Message = "Траектория пуста, программист прими меры!"; return false; }

                StreamWriter sw = new StreamWriter(fileName);
                bool firstSourse = true;
                foreach (HPoint sourse in Sourses)
                {
                    if (firstSourse)
                    {
                        sw.Write(sourse.ToString());
                        firstSourse = false;
                    }
                    else
                    {
                        sw.Write(", " + sourse.ToString());
                    }
                }
                sw.WriteLine();
                foreach (HTime time in Times)
                {                    
                    sw.WriteLine(time.ToString());
                }
                sw.Close();
                return true;
            }
            catch (Exception ex)
            {
                Message = "Во время записи файла, возникло исключение! \n" + ex.Message;
                return false;
            }
        }

        internal void SetSourses(List<HPoint> sourses)
        {
            Sourses = sourses;
        }

        private bool TryParseTime(string line)
        {// Строчка со времением, название переименовать!
            string[] timeLine = line.Split(new[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries);
            HTime time = new HTime();
            for (int i = 0; i < timeLine.Length; i++)
            {
                if (double.TryParse(timeLine[i].Replace('.', ','), out double t))
                    time.AddTime(t);
                else
                {
                    Message = $"Ошибка парсинга времени {i} источника. (t='{timeLine[i]}')";
                    return false;
                }
            }
            Times.Add(time);
            return true;
        }

        private bool TryParseSources(string line)
        {// Строчка с координатами, название не удачное надо переименовать
            string[] coordinatesLine = line.Split(new[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries);
            if (coordinatesLine.Length % 2 != 0) { Message = "Число координат нечетное!\n" + line; return false; }

            if (Sourses == null) Sourses = new List<HPoint>();

            for (int i = 0; i < coordinatesLine.Length; i+=2)
            {
                if (double.TryParse(coordinatesLine[i].Replace('.', ','), out double x) && double.TryParse(coordinatesLine[i + 1].Replace('.', ','), out double y))
                    Sourses.Add(new HPoint(x, y));
                else
                {
                    Message = $"Ошибка парсинга координат {i / 2 + 1} источника. (x='{coordinatesLine[i]}',y='{coordinatesLine[i + 1]}')";
                    return false;
                }
            }

            return true;            
        }

        internal bool FileOutputWrite(string fileName)
        {
            try
            {
                if (Trajectory == null) { Message = "Траектория пуста, программист прими меры!"; return false; }

                StreamWriter sw = new StreamWriter(fileName);
                
                sw.WriteLine();
                foreach (HPoint point in Trajectory)
                {
                    sw.WriteLine(point.ToString());    
                }
                sw.Close();
                return true;
            }
            catch (Exception ex)
            {
                Message = "Во время записи файла, возникло исключение! \n" + ex.Message;
                return false;
            }
        }

        internal void SetTrajectory(List<HPoint> trajectory)
        {
            Trajectory = trajectory;
        }
    }
}