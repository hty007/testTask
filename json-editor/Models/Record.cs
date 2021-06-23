﻿using Newtonsoft.Json.Linq;
using System;
using System.Collections.ObjectModel;
using WPFStorage.Base;

namespace Teko.Test.Editor.Models
{
    /// <summary>
    /// Асоциирует с JToken и его потомками
    /// </summary>
    public class Record : ObservableObject
    {
        #region members
        private JToken token;
        private Record parent;
        private bool editMode;
        #endregion
        #region ctor
        /// <summary>
        /// Создает root-object
        /// </summary>
        /// <param name="jsonObj"></param>
        public Record(JObject jsonObj)
        {
            this.token = jsonObj;
            AddChildren(jsonObj);
        }

        /// <summary>
        /// Создает запись имешую родителя 
        /// </summary>
        /// <param name="item"></param>
        /// <param name="parent"></param>
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
        /// <summary>
        /// Получает имя запись и если это возможно изменяет
        /// </summary>
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
        /// <summary>
        /// Получает значение записи (если оно не комплексное) и если это возможно изменяет
        /// </summary>
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


        /// <summary>
        /// Тип записи
        /// </summary>
        public string Type
        {
            get
            {
                if (token != null)
                    return token.Type.ToString();
                return "null";
            }
        }

        /// <summary>
        /// Если запись контейнер, количество дочерних элементов 
        /// </summary>
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

        /// <summary>
        /// Запись не является комплексной
        /// </summary>
        public bool IsLeaf
        {
            get
            {
                if (token != null)
                {
                    return token is JValue 
                        || (token is JProperty property && property.Value is JValue);
                }
                return false;
            }
        }

        /// <summary>
        /// Запись является свойством
        /// </summary>
        public bool IsProperty => token is JProperty;

        /// <summary>
        /// Запись является контейнером
        /// </summary>
        public bool IsContainer => token is JContainer;
        /// <summary>
        /// Запись является массивом
        /// </summary>
        public bool IsArray => token is JArray;
        /// <summary>
        /// Запись является значением
        /// </summary>
        public bool IsValue => token is JValue;
        /// <summary>
        /// Запись является объектом ()
        /// </summary>
        public bool IsObject => token is JObject;
        /// <summary>
        /// Запись является root-object
        /// </summary>
        public bool IsRoot => parent == null;
        /// <summary>
        /// Запись является свойством мо значением Array
        /// </summary>
        public bool IsPropertyArray => token is JProperty property && property.Value is JArray;
        /// <summary>
        /// Запись является свойством мо значением Object
        /// </summary>
        public bool IsPropertyObject => token is JProperty property && property.Value is JObject;
        /// <summary>
        /// колекция дочерних объетов записи
        /// </summary>
        public ObservableCollection<Record> Children { get; } = new ObservableCollection<Record>();

        /// <summary>
        /// Режим редактирования
        /// </summary>
        public bool EditMode
        {
            get => editMode;
            set
            {
                SetProperty(ref editMode, value);
                RaisePropertyChanged(nameof(CanEditName));
                RaisePropertyChanged(nameof(CanEditValue));
            }
        }

        /// <summary>
        /// Возможность редактировать название <see cref="Name"/>
        /// </summary>
        public bool CanEditName => EditMode && IsProperty;
        /// <summary>
        /// Возможность редактировать значение <see cref="Value"/>
        /// </summary>
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

        public Record AddObjectInside()
        {
            var newToken = new JObject();
            return AddNewTokenInside("object", newToken);
        }

        public Record AddArrayInside(string key)
        {
            var newToken = new JArray();

            return AddNewTokenInside(key, newToken);
        }

        public Record AddObjectInside(string key)
        {
            var newToken = new JObject();

            return AddNewTokenInside(key, newToken);
        }

        public Record AddPropertyInside(string key, string value)
        {
            var newToken = new JValue(value);
            return AddNewTokenInside(key, newToken);
        }

        internal Record AddPropertyAfter(string key, string value)
        {
            var property = new JProperty(key, value);
            token.AddAfterSelf(property);
            var rec = new Record(property, parent);
            parent.Children.Add(rec);
            return rec;
        }

        private Record AddNewTokenInside(string key, JToken value)
        {
            if (token is JObject jObj)
            {
                jObj.Add(key, value);
                //token.Last.AddAfterSelf(newToken);
                var child = new Record(jObj.Last, this);
                Children.Add(child);
                return child;
            }
            else if (token is JProperty property && property.Value is JObject obj)
            {
                obj.Add(key, value);
                var child = new Record(obj.Last, this);
                Children.Add(child);
                return child;
            }
            else if (token is JProperty property1 && property1.Value is JArray array)
            {
                //if (newToken is JProperty property2)
                //    newToken = property2.Value;
                array.Add(value);
                var child = new Record(array.Last, this);
                Children.Add(child);
                return child;
            }
            else
            {
                return null;
            }
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
