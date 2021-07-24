using System.Collections.Generic;

namespace ConsoleStorage.INI
{
    public class IniElement
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public List<string> Comment { get; set; }

        public override string ToString()
        {
            return $"{Name}={Value}";
        }
    }
}
