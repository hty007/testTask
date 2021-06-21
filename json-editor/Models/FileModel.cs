using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using WPFStorage.Base;
using WPFStorage.Dialogs;

namespace Teko.Test.Editor.Models
{


    public class FileModel : ObservableObject
    {
        private string fileName;

        public string FileName { get => fileName; set => SetProperty(ref fileName, value); }
        public ObservableCollection<Record> Children { get; set; } = new ObservableCollection<Record>();
        public bool IsOpen { get; internal set; }

        internal void Open(string fileName)
        {
            var allText = File.ReadAllText(fileName);
            var jsonStrings = allText.Split(new[] { "-----", "/-----" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var item in jsonStrings)
            {
                if (string.IsNullOrWhiteSpace(item))
                    continue;
                JObject jObj = JsonConvert.DeserializeObject(item) as JObject;
                Record rec = new Record(jObj);
                Children.Add(rec);

            }
        }

        internal void Save(string fileName)
        {
            WinBox.ShowMessage($"Сохранение файл '{fileName}'");
        }
    }

    public class Record : ObservableObject
    {
        //private JObject jsonObj;
        //private JProperty jsonProperty;
        private JToken token;

        public Record(JObject jsonObj)
        {
            this.token = jsonObj;
            foreach (var tok in jsonObj.Children())
            {
                Record record = new Record(tok);
                Children.Add(record);
            }
            //foreach (var obj in jsonObj.Properties())
            //{
            //    Record record = new Record(obj);
            //    Children.Add(record);
            //}

        }

        //public Record(JProperty prorerty)
        //{
        //    this.jsonProperty = prorerty;
        //    foreach (var item in jsonProperty.Value.Children())
        //    {
        //        if (item.HasValues)
        //        {
        //            Children.Add(new Record(item));

        //        }    
        //        //item.Type;
        //    }

        //}

        public Record(JToken item)
        {
            this.token = item;
            if (token is JProperty property)
            {
                AddChildren(property.Value);
            }
            else if (token.HasValues)
            {
                AddChildren(token);
            }
        }

        private void AddChildren(JToken parent)
        {
            if (parent.HasValues)
            {
                foreach (var child in parent.Children())
                {
                    Children.Add(new Record(child));
                }
            }
        }

        public string Name
        {
            get
            {
                if (token != null)
                {
                    if (token is JProperty property)
                        return property.Name;
                    if (token is JObject)
                        return "obj";
                }
                return $"none({token.GetType().Name})";
            }
        }
        public string Type
        {
            get
            {
                if (token != null)
                    return token.Type.ToString();
                return "null";
            }
        }

        public int Count
        {
            get
            {
                if (token != null)
                {
                    if (token is JContainer container)
                        return container.Count;
                }
                return Children.Count;
            }
        }

        public bool IsLeaf
        {
            get
            {
                if (token != null)
                {
                    return token.First is JValue;
                }
                return false;
            }
        }

        public string Value
        {
            get
            {
                if (token != null)
                {
                    if (token is JProperty property)
                    {
                        if (property.Value is JValue p_value)
                            return p_value.Value.ToString();
                    }
                    if (token is JValue value)
                    {
                        return value.Value.ToString();
                    }
                }
                return string.Empty;
            }
        }

        public ObservableCollection<Record> Children { get; } = new ObservableCollection<Record>();
    }
}
