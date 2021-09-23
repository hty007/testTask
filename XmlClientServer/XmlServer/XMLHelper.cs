using Protocol;
using System;
using System.Xml.Linq;

namespace XmlServer
{
    internal class XMLHelper
    {
        internal static ProtocolModel GetModel(string xml)
        {
            var doc = XDocument.Parse(xml);
            var xModel = doc.Element("model");
            ProtocolModel model = new ProtocolModel();
            model.Id = (uint)xModel.Attribute(nameof(model.Id));
            model.FormatVersion = (int)xModel.Attribute(nameof(model.FormatVersion));

            model.To = (string)xModel.Element(nameof(model.To));
            model.From = (string)xModel.Element(nameof(model.From));
            model.Text = (string)xModel.Element(nameof(model.Text));

            var strColor = (string)xModel.Element(nameof(model.Color));
            uint num = uint.Parse(strColor, System.Globalization.NumberStyles.AllowHexSpecifier);
            model.Color = BitConverter.GetBytes(num);

            var strImage = (string)xModel.Element(nameof(model.image));
            model.image = Convert.FromBase64String(strImage);

            return model;
        }

        internal static string GetXML(ProtocolModel model)
        {
            var doc = new XDocument();
            var xModel = new XElement("model");
            xModel.Add(new XAttribute(nameof(model.Id), model.Id));
            xModel.Add(new XAttribute(nameof(model.FormatVersion), model.FormatVersion));
            xModel.Add(new XElement(nameof(model.To), model.To));
            xModel.Add(new XElement(nameof(model.From), model.From));
            xModel.Add(new XElement(nameof(model.Text), model.Text));
            xModel.Add(new XElement(nameof(model.Color), BitConverter.ToString(model.Color)));
            xModel.Add(new XElement(nameof(model.image), Convert.ToBase64String(model.image)));
            doc.Add(xModel);

            return doc.ToString();
        }
    }
}