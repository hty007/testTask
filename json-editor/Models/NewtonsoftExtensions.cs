using Newtonsoft.Json.Linq;
using System;

namespace Teko.Test.Editor.Models
{
    public static class NewtonsoftExtensions
    {
        public static JProperty Rename(this JProperty property, string newName)
        {
            if (property == null)
                throw new ArgumentNullException("property", "Cannot rename a null token");

            if (property.Parent == null)
                throw new InvalidOperationException("Cannot rename a property with no parent");

            var existingValue = property.Value;
            property.Value = null;
            var newProperty = new JProperty(newName, existingValue);
            property.Replace(newProperty);
            return newProperty;
        }

        //public static void ReplaceValue(this JValue value, string newValue)
        //{
        //    if (value == null)
        //        throw new ArgumentNullException("value", "Cannot rename a null token");

        //    if (value.Parent == null)
        //        throw new InvalidOperationException("Cannot rename a value with no parent");

        //    value.Value
        //}
    }
}
