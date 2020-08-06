using System;
using System.Collections.Generic;
using System.IO;

namespace GPSTask
{
    public class DataReader
    {
        private List<HPoint> Sourses = new List<HPoint>();
        private List<HTime> Times = new List<HTime>();

        public List<HPoint> GetSourses() => Sourses;
        public List<HTime> GetTimes() => Times;

        public string Message { get; private set; }

        internal bool Open(string fileName)
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
    }
}