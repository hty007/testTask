using Protocol;
using System;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using WPFStorage.Base;

namespace XmlServer
{
    internal class EditorModel: ObservableObject
    {
        private int formatVersion;
        private uint id;
        private string to;
        private string from;
        private string text;
        private Color color;
        private BitmapImage image;
        private EditorModelWindow window;

        public int FormatVersion { get => formatVersion; set => SetProperty(ref formatVersion, value); }
        public uint Id { get => id; set => SetProperty(ref id, value); }
        public string To { get => to; set => SetProperty(ref to, value); }
        public string From { get => from; set => SetProperty(ref from, value); }
        public string Text { get => text; set => SetProperty(ref text, value); }
        public Color Color { get => color; set => SetProperty(ref color, value); }
        public BitmapImage Image { get => image; set => SetProperty(ref image, value); }

        public EditorModel(ProtocolModel model)
        {
            FormatVersion = model.FormatVersion;
            Id = model.Id;
            To = model.To;
            From = model.From;
            Text = model.Text;
            Color = ToColor(model.Color);
            Image = ToImage(model.Image);
        }

        //  https://stackoverflow.com/questions/14337071/convert-array-of-bytes-to-bitmapimage
        public BitmapImage ToImage(byte[] array)
        {
            using (var ms = new MemoryStream(array))
            {
                var image = new BitmapImage();
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad; // here
                image.StreamSource = ms;
                image.EndInit();
                return image;
            }
        }


        public byte[] ToBytes(BitmapImage bitmapImage)
        {
            byte[] data;
            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bitmapImage));
            using (MemoryStream ms = new MemoryStream())
            {
                encoder.Save(ms);
                data = ms.ToArray();
            }
            return data;
        }

        private Color ToColor(byte[] bytes)
        {
            if (bytes.Length >= 4)
            {
                return Color.FromArgb(bytes[0],bytes[1], bytes[2], bytes[3]);
            }
            if (bytes.Length == 3)
            {
                return Color.FromRgb(bytes[0], bytes[1], bytes[2]);
            }
            return default;
        }

        internal bool? OpenDialog()
        {
            window = new EditorModelWindow();
            window.DataContext = this;
            window.ShowDialog();

            return true;
        }

        internal ProtocolModel GetModel()
        {
            throw new NotImplementedException();
        }
    }
}