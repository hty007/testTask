using Protocol;
using System;
using System.Linq;
using System.Xml.Linq;

namespace XmlServer
{
    internal class XMLHelper
    {
        internal static MailModel GetModel(string xml)
        {
            var doc = XDocument.Parse(xml);
            var xModel = doc.Element("model");
            MailModel model = new MailModel();
            model.Id = (uint)xModel.Attribute(nameof(model.Id));
            model.FormatVersion = (int)xModel.Attribute(nameof(model.FormatVersion));

            model.To = (string)xModel.Element(nameof(model.To));
            model.From = (string)xModel.Element(nameof(model.From));
            model.Text = (string)xModel.Element(nameof(model.Text));

            model.Color = (string)xModel.Element(nameof(model.Color));

            var strImage = (string)xModel.Element(nameof(model.Image));
            model.Image = Convert.FromBase64String(strImage);

            return model;
        }

        internal static string GetXML(MailModel model)
        {
            var doc = new XDocument();
            var xModel = new XElement("model");
            xModel.Add(new XAttribute(nameof(model.Id), model.Id));
            xModel.Add(new XAttribute(nameof(model.FormatVersion), model.FormatVersion));
            xModel.Add(new XElement(nameof(model.To), model.To));
            xModel.Add(new XElement(nameof(model.From), model.From));
            xModel.Add(new XElement(nameof(model.Text), model.Text));
            xModel.Add(new XElement(nameof(model.Color), model.Color));
            xModel.Add(new XElement(nameof(model.Image), Convert.ToBase64String(model.Image)));
            doc.Add(xModel);

            return doc.ToString();
        }

        public static byte[] StringToByteArray(string hex)
        {
            return Enumerable.Range(0, hex.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                             .ToArray();
        }
    }
}