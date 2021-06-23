using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Text;
using WPFStorage.Base;
using WPFStorage.Dialogs;

namespace Teko.Test.Editor.Models
{
    public class FileModel : ObservableObject
    {
        private const string START_TOKEN = "-----";
        private const string STOP_TOKEN = "/-----";
        private string fileName;

        public string FileName { get => fileName; set => SetProperty(ref fileName, value); }
        public ObservableCollection<Record> Children { get; set; } = new ObservableCollection<Record>();
        public bool IsOpen { get; internal set; }

        internal void Open(string fileName)
        {
            var allText = File.ReadAllText(fileName);
            var jsonStrings = allText.Split(new[] { START_TOKEN, STOP_TOKEN }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var item in jsonStrings)
            {
                if (string.IsNullOrWhiteSpace(item))
                    continue;
                JObject jObj = JsonConvert.DeserializeObject(item) as JObject;
                Record rec = new Record(jObj);
                Children.Add(rec);
            }
            IsOpen = true;
        }

        internal void Save(string fileName)
        {
            StringBuilder builder = new StringBuilder();
            foreach (var child in Children)
            {
                builder.AppendLine(START_TOKEN);
                builder.AppendLine(child.GetJson());
                builder.AppendLine(STOP_TOKEN);
            }

            File.WriteAllText(fileName, builder.ToString());
            Process.Start(fileName);
        }

        internal void Close()
        {
            Children.Clear();
            IsOpen = false;
        }

        internal void Create()
        {
            Children.Clear();
            var record = new Record(new JObject());
            Children.Add(record);
            IsOpen = true;
        }
    }
}
