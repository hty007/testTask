using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;

namespace ConsoleStorage.INI
{
    public class Group : IEnumerable<IniElement>
    {
        private List<IniElement> _elements;
        #region property
        public List<string> Comment { get; set; }
        public string Name { get; set; }
        public object Count { get => _elements.Count; }
        #endregion
        #region constructor
        public Group()
        {
            _elements = new List<IniElement>();
        }
        #endregion
        #region bool
        public void SetBool(string key, bool value, params string[] comments)
        {
            IniElement element = _elements.Find(x => x.Name == key);
            if (element == null)
            {
                element = new IniElement() { Name = key };
                _elements.Add(element);
            }
            element.Value = value.ToString();
            if (comments.Length > 0)
            {
                element.Comment = new List<string>(comments);
            }
        }


        public bool GetBool(string key)
        {
            IniElement element = _elements.Find(e => e.Name == key);
            if (bool.TryParse(element.Value, out bool result))
                return result;
            else
            {
                throw new ArgumentException($"Ошибка парсинга элемента {element.Name}. Невозможно преобразовать '{element.Value}' в тип <bool>");
            }
        }
        #endregion
        #region Date
        public void SetDate(string key, DateTime value, params string[] comments)
        {
            IniElement element = _elements.Find(x => x.Name == key);
            if (element == null)
            {
                element = new IniElement() { Name = key };
                _elements.Add(element);
            }
            element.Value = value.ToString("yyyy-MM-dd HH:mm:ss");
            if (comments.Length > 0)
            {
                element.Comment = new List<string>(comments);
            }
        }
        public DateTime GetDate(string key)
        {
            IniElement element = _elements.Find(e => e.Name == key);
            if (DateTime.TryParse(element.Value, out DateTime result))
                return result;
            else
            {
                throw new ArgumentException($"Ошибка парсинга элемента {element.Name}. Невозможно преобразовать '{element.Value}' в тип <DateTime>");
            }
        }
        #endregion
        #region String
        public void SetString(string key, string value, params string[] comments)
        {
            IniElement element = _elements.Find(x => x.Name == key);
            if (element == null)
            {
                element = new IniElement() { Name = key };
                _elements.Add(element);
            }
            element.Value = value;
            if (comments.Length > 0)
            {
                element.Comment = new List<string>(comments);
            }
        }

        

        public string GetString(string key)
        {
            return _elements.Find(e => e.Name == key).Value;
        }
        #endregion
        #region Numeric
        public void SetNumeric(string key, long value, params string[] comments)
        {
            IniElement element = _elements.Find(x => x.Name == key);
            if (element == null)
            {
                element = new IniElement() { Name = key };
                _elements.Add(element);
            }
            element.Value = value.ToString();
            if (comments.Length > 0)
            {
                element.Comment = new List<string>(comments);
            }
        }


        public long GetNumeric(string key)
        {
            IniElement element = _elements.Find(e => e.Name == key);
            if (long.TryParse(element.Value, out long result))
                return result;
            else
            {
                throw new ArgumentException($"Ошибка парсинга элемента {element.Name}. Невозможно преобразовать '{element.Value}' в тип <long>");
            }
        }
        public int GetInteger(string key)
        {
            IniElement element = _elements.Find(e => e.Name == key);
            if (int.TryParse(element.Value, out int result))
                return result;
            else
            {
                throw new ArgumentException($"Ошибка парсинга элемента {element.Name}. Невозможно преобразовать '{element.Value}' в тип <int>");
            }
        }
        #endregion
        #region Real
        public void SetReal(string key, double value, params string[] comments)
        {
            IniElement element = _elements.Find(x => x.Name == key);
            if (element == null)
            {
                element = new IniElement() { Name = key };
                _elements.Add(element);
            }
            element.Value = value.ToString();
            if (comments.Length > 0)
            {
                element.Comment = new List<string>(comments);
            }
        }
        public double GetReal(string key)
        {
            IniElement element = _elements.Find(e => e.Name == key);
            if (double.TryParse(element.Value, NumberStyles.Any, CultureInfo.InvariantCulture, out double result))
                return result;
            else
            {
                throw new ArgumentException($"Ошибка парсинга элемента {element.Name}. Невозможно преобразовать '{element.Value}' в тип <double>");
            }
        }

        public float GetFloat(string key)
        {
            IniElement element = _elements.Find(e => e.Name == key);
            if (float.TryParse(element.Value, NumberStyles.Any, CultureInfo.InvariantCulture, out float result))
                return result;
            else
            {
                throw new ArgumentException($"Ошибка парсинга элемента {element.Name}. Невозможно преобразовать '{element.Value}' в тип <double>");
            }
        }
        #endregion
        #region other method
        internal void AddElement(IniElement element)
        {
            _elements.Add(element);
        }

        public override string ToString()
        {
            return $"{Name}[{Count}]";
        }

        public IEnumerator<IniElement> GetEnumerator()
        {
            return ((IEnumerable<IniElement>)_elements).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)_elements).GetEnumerator();
        }
        #endregion
    }
}
