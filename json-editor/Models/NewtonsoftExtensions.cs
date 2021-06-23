using Newtonsoft.Json.Linq;
using System;

namespace Teko.Test.Editor.Models
{
    public static class NewtonsoftExtensions
    {
        // Поддсмотрено на форуме 
        // https://stackoverflow.com/questions/47267542/rename-jproperty-in-json-net
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

       
    }
}
