using Protocol;
using System;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using WPFStorage.Base;

namespace XmlClient
{
    // TODO: Абстрагировать Editor и Viewer 
    public class ViewerViewModel: ObservableObject
    {
        #region fields
        private int formatVersion;
        private uint id;
        private string to;
        private string from;
        private string text;
        private Color color;
        private BitmapImage image;
        private string fileName;
        private DateTime time;
        #endregion
        #region constuctor

        public ViewerViewModel()
        {
            
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
        
        public DateTime Time { get => time; set => SetProperty(ref time, value); }
        public string Text { get => text; set => SetProperty(ref text, value); }
        public Color Color { get => color; set => SetProperty(ref color, value); }
        public BitmapImage Image { get => image; set => SetProperty(ref image, value); }
        public RelayCommand LoadImageCommand { get; }
        public RelayCommand CancelCommand { get; }
        public RelayCommand OkCommand { get; }
        public bool? PressOk { get; private set; }

        public void SetModel(MailModel model)
        {
            FormatVersion = model.FormatVersion;
            Id = model.Id;
            To = model.To;
            From = model.From;
            Text = model.Text;
            Color = ToColor(model.Color);
            Image = ToImage(model.Image);
        }
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

        public static Color ToColor(string stringColor)
        {
            var color = (Color)ColorConverter.ConvertFromString(stringColor);
            return color;
        }
        #endregion
        #region public
        

        //public bool? OpenDialog()
        //{
        //    window = new ViewerView();
        //    window.DataContext = this;
        //    window.ShowDialog();

        //    return PressOk;
        //}
        #endregion
        #region private
        //private void Ok()
        //{
        //    window.DialogResult = true;
        //    PressOk = true;
        //    window.Close();
        //}
        #endregion
    }
}