using Microsoft.Win32;
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
        #region fields
        private int formatVersion;
        private uint id;
        private string to;
        private string from;
        private string text;
        private Color color;
        private BitmapImage image;
        private EditorModelWindow window;
        private string fileName;
        #endregion
        #region constuctor

        public EditorModel(MailModel model)
        {
            FormatVersion = model.FormatVersion;
            Id = model.Id;
            To = model.To;
            From = model.From;
            Text = model.Text;
            //Color = Colors.Red;
            Color = ToColor(model.Color);
            Image = ToImage(model.Image);

            LoadImageCommand = new RelayCommand(LoadImage);
            CancelCommand = new RelayCommand(Cancel);
            SaveCommand = new RelayCommand(Save);
        } 
        #endregion
        #region Properties
        #region Color
        public byte Red
        {
            get => Color.R;
            set
            {
                color.R = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(Color));
            }
        }
        public byte Blue
        {
            get => Color.B;
            set
            {
                color.B = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(Color));
            }
        }
        public byte Green
        {
            get => Color.G;
            set
            {
                color.G = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(Color));
            }
        }
        #endregion
        public string FileName { get => fileName; set => SetProperty(ref fileName, value); }
        public int FormatVersion { get => formatVersion; set => SetProperty(ref formatVersion, value); }
        public uint Id { get => id; set => SetProperty(ref id, value); }
        public string To { get => to; set => SetProperty(ref to, value); }
        public string From { get => from; set => SetProperty(ref from, value); }
        public string Text { get => text; set => SetProperty(ref text, value); }
        public Color Color { get => color; set => SetProperty(ref color, value); }
        public BitmapImage Image { get => image; set => SetProperty(ref image, value); }
        public RelayCommand LoadImageCommand { get; }
        public RelayCommand CancelCommand { get; }
        public RelayCommand SaveCommand { get; }
        public bool? PressSave { get; private set; } 
        #endregion
        #region static methods
        //  https://stackoverflow.com/questions/14337071/convert-array-of-bytes-to-bitmapimage
        public static BitmapImage ToImage(byte[] array)
        {
            if (array == null)
                return null;

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


        public static byte[] ToBytes(BitmapImage bitmapImage)
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

        public static Color ToColor(string stringColor)
        {
            var color = (Color)ColorConverter.ConvertFromString(stringColor);
            return color;
            //if (bytes != null)
            //{
            //    if (bytes.Length >= 4)
            //    {
            //        return Color.FromArgb(bytes[0], bytes[1], bytes[2], bytes[3]);
            //    }
            //    if (bytes.Length == 3)
            //    {
            //        return Color.FromRgb(bytes[0], bytes[1], bytes[2]);
            //    }
            //}
            //return default;
        }

        public static byte[] ToBytes(Color color)
        {
            return new[] { color.A, color.R, color.G, color.B };
        }
        #endregion
        #region public
        public MailModel GetModel()
        {
            MailModel model = new MailModel();
            model.FormatVersion = FormatVersion;
            model.Id = Id;
            model.To = To;
            model.From = From;
            model.Text = Text;
            model.Color = Color.ToString();
            model.Image = ToBytes(Image);
            return model;
        }

        public bool? OpenDialog()
        {
            window = new EditorModelWindow();
            window.DataContext = this;
            window.ShowDialog();

            return PressSave;
        }
        #endregion
        #region private
        private void Save()
        {
            // ToDo: Проверить существует ли файл и если что переименовать

            window.DialogResult = true;
            PressSave = true;
            window.Close();
        }

        private void Cancel()
        {
            window.DialogResult = false;
            PressSave = false;
            window.Close();
        }


        private void LoadImage()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "image files (*.jpg)|*.txt|All files (*.*)|*.*";

            ofd.CheckFileExists = true;
            ofd.AddExtension = true;
            var res = ofd.ShowDialog();
            if (res == true)
            {
                Image = new BitmapImage(new Uri(ofd.FileName, UriKind.Absolute));
            }
        } 
        #endregion
    }
}