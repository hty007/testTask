using Protocol;
using System;
using System.IO;

namespace XmlServer
{
    internal class ModelFileHelper
    {
        internal static void Save(MailModel model, string fileName)
        {
            var xml = XMLHelper.GetXML(model);
            File.WriteAllText(fileName, xml);
        }

        internal static MailModel Load(string fileName)
        {
            var xml = File.ReadAllText(fileName);
            return XMLHelper.GetModel(xml);
        }
    }
}