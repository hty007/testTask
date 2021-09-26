using System;
using System.IO;

namespace Protocol
{
    public class StreamHelper
    {
        public static MemoryStream ModelToStream(MailModel model)
        {
            MemoryStream ms = new MemoryStream();
            using (BinaryWriter bw = new BinaryWriter(ms))
            {
                bw.Write(model.FormatVersion);
                bw.Write(model.Id);
                bw.Write(model.To);
                bw.Write(model.From);
                bw.Write(model.Text);
                bw.Write(model.Color);
                bw.Write(model.Image.Length);
                bw.Write(model.Image);
                return new MemoryStream(ms.ToArray());
            }

        }

        public static MailModel StreamToModel(MemoryStream ms, int start = 4)
        {
            ms.Position = start;
            var model = new MailModel();
            using (BinaryReader br = new BinaryReader(ms))
            {
                model.FormatVersion = br.ReadInt32();
                model.Id = br.ReadUInt32();
                model.To = br.ReadString();
                model.From = br.ReadString();
                model.Text = br.ReadString();
                model.Color = br.ReadString();
                int countImage = br.ReadInt32();
                model.Image = br.ReadBytes(countImage);
            }

            return model;
        }
    }
}
