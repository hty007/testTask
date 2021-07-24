using System.Collections.Generic;

namespace ConsoleStorage.INI
{
    public static class LinesReaderHelper
    {
        public static int ReadGroup(string[] lines, int position, out Group group)
        {
            group = null;
            int index = ReadComment(lines, position, out List<string> comment);

            if (lines.Length <= index) return index;
            string line = lines[index].Trim();
            var _group = new Group();
            _group.Name = line.TrimStart('[').TrimEnd(']');
            _group.Comment = comment;
            index++;
            while (index < lines.Length)
            {
                index = ReadElement(lines, index, out IniElement element);
                if (element != null)
                    _group.AddElement(element);
                else
                    break;
            }
            group = _group;
            return index;
        }

        public static int ReadElement(string[] lines, int position, out IniElement element)
        {
            element = null;
            int index = ReadComment(lines, position, out List<string> comment);
            if (index >= lines.Length) return index;
            while (index < lines.Length)
            {
                string line = lines[index];
                if (!string.IsNullOrWhiteSpace(line))
                {
                    line = line.Trim();
                    if (line.StartsWith("[")) break;

                    string[] param = line.Split('=');

                    if (param.Length == 2)
                    {
                        element = new IniElement()
                        {
                            Name = param[0],
                            Value = param[1],
                            Comment = comment
                        };
                        index++;
                        break;
                    }
                }
                index++;
            }
            return index;
        }

        public static int ReadComment(string[] lines, int position, out List<string> comment)
        {
            int i = position;
            List<string> _comment = null;
            for (; i < lines.Length; i++)
            {
                string line = lines[i];

                if (string.IsNullOrWhiteSpace(line)) continue;
                line = line.Trim();

                if (line.StartsWith("#"))
                    AddComment(line.TrimStart('#', ' '), ref _comment);
                else
                    break;
            }
            comment = _comment;
            return i;
        }

        private static void AddComment(string line, ref List<string> comment)
        {
            if (comment == null) comment = new List<string>();
            comment.Add(line);
        }
    }
}
