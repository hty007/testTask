using Newtonsoft.Json.Linq;
using System;
using System.Collections.ObjectModel;
using WPFStorage.Base;

namespace Teko.Test.Editor.Models
{
    public class Record : ObservableObject
    {
        #region members
        private JToken token;
        private Record parent;
        private bool editMode;
        #endregion
        #region ctor
        public Record(JObject jsonObj)
        {
            this.token = jsonObj;
            AddChildren(jsonObj);
        }

        public Record(JToken item, Record parent)
        {
            this.token = item;
            this.parent = parent;
            if (token is JProperty property)
            {
                AddChildren(property.Value);
            }
            else if (token.HasValues)
            {
                AddChildren(token);
            }
        }
        #endregion
        #region binding
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

            set
            {
                if (token != null)
                {
                    if (token is JProperty property)
                    {
                        token = property.Rename(value);
                        RaisePropertyChanged();
                    };
                }
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
                    else if (token is JValue value)
                    {
                        return value.Value.ToString();
                    }
                }
                return string.Empty;
            }
            set
            {
                if (token != null)
                {
                    if (token is JProperty property)
                    {
                        if (property.Value is JValue p_value)
                            p_value.Value = value;
                    }
                    else if (token is JValue m_value)
                    {
                        m_value.Value = value;
                    }
                }
                RaisePropertyChanged();
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

        public bool IsProperty => token is JProperty;
        public bool IsContainer => token is JContainer;
        public bool IsArray => token is JArray;
        public bool IsObject => token is JObject;
        public bool IsRoot => parent == null;
        public bool IsPropertyArray => token is JProperty property && property.Value is JArray;
        public bool IsPropertyObject => token is JProperty property && property.Value is JObject;

        public ObservableCollection<Record> Children { get; } = new ObservableCollection<Record>();

        public bool EditMode
        { 
            get => editMode; 
            set {
                SetProperty(ref editMode, value);
                RaisePropertyChanged(nameof(CanEditName));
                RaisePropertyChanged(nameof(CanEditValue));
            } 
        }

        internal Record AddPropertyAfter(string key, string value)
        {
            var property = new JProperty(key, value);
            token.AddAfterSelf(property);
            var rec = new Record(property, parent);
            parent.Children.Add(rec);
            return rec;
        }

        internal Record CreateProperty(string key, string value)
        {
            if (IsObject)
            {
                var property = new JProperty(key, value);
                token.Last.AddAfterSelf(property);
                //token.AddAnnotation(property);
                var child = new Record(property, this);
                Children.Add(child);
                return child;
            }
            else if (IsPropertyObject )
            {
                var property = new JProperty(key, value);
                token.First.First.AddAfterSelf(property);
                //token.AddAnnotation(property);
                var child = new Record(property, this);
                Children.Add(child);
                return child;
            }
            else if (IsPropertyArray )
            {
                var property = new JProperty(key, value);
                token.First.First.First.AddAfterSelf(property);
                //token.AddAnnotation(property);
                var child = new Record(property, this);
                Children.Add(child);
                return child;
            }
            else
            {
                return null;
            }
        }

        public bool CanEditName => EditMode && IsProperty;
        public bool CanEditValue => EditMode && IsLeaf;

        #endregion
        #region methods
        public void Detete()
        {
            if (!IsRoot)
            {
                parent.Children.Remove(this);
                token.Remove();
            }
        }

        public string GetJson()
        {
            return token.ToString();
        }
        private void AddChildren(JToken parent)
        {
            if (parent.HasValues)
            {
                foreach (var child in parent.Children())
                {
                    Children.Add(new Record(child, this));
                }
            }
        } 
        #endregion
    }
}
