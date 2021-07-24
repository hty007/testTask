using System;
using System.Collections.Generic;
using System.IO;

namespace ConsoleStorage.INI
{
    public class IniData : IDisposable
    {
        private string _fileName;
        private List<Group> _groups;

        private IniData(string fileName)
        {
            _fileName = fileName;
            _groups = new List<Group>();
        }

        public IReadOnlyCollection<Group> Groups => _groups;

        public void Dispose()
        {
            _groups.Clear();
            _groups = null;
        }
        public Group GetGroup(string key)
        {
            Group group = _groups.Find(g => g.Name == key);
            if (group == null)
            {
                group = new Group() { Name = key };
                _groups.Add(group);
            }
            return group;
        }

        /// <summary>
        /// Загружает определённый файл в формате ini и возвращает класс по работе с ним
        /// </summary>
        /// <param name="fileName">путь к файлу</param>
        /// <returns></returns>
        public static IniData LoadFile(string fileName)
        {
            IniData data = new IniData(fileName);
            data.LoadFile();
            return data;
        }

        #region LoadFile
        protected void LoadFile()
        {
            if (!File.Exists(_fileName)) File.CreateText(_fileName).Close();

            string[] lines = File.ReadAllLines(_fileName);

            int index = 0;
            while (true)
            {
                index = LinesReaderHelper.ReadGroup(lines, index, out Group group);
                if (group == null) break;
                else _groups.Add(group);
            }

        }
        #endregion
        #region SaveFile
        public void SaveFile()
        {
            FileInfo file = new FileInfo(_fileName);
            using (var sw = file.CreateText())
            {
                foreach (var group in _groups)
                {
                    WrireGroup(sw, group);
                }
            }
        }

        private static void WrireGroup(StreamWriter sw, Group group)
        {
            if (group.Comment != null) WriteComment(sw, group.Comment);
            sw.WriteLine($"[{group.Name}]");
            foreach (IniElement element in group)
            {
                if (element.Comment != null) WriteComment(sw, element.Comment);
                sw.WriteLine(element.ToString());
            }
        }

        private static void WriteComment(StreamWriter sw, List<string> comment)
        {
            foreach (var item in comment)
            {
                sw.WriteLine($"# {item}");
            }
        }
        #endregion
    }
}
