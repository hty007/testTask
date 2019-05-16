using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace SitePing
{
    //[ValueConversion(typeof(bool), typeof(System.Windows.Media.Brushes))]
    public class ColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value.GetType() == typeof(bool)) return GetBrush((bool)value);
            if (value.GetType() == typeof(Pair))
            {
                Pair p = (Pair)value;
                return GetBrush(p.Status);
            }
            throw new ArgumentException();

        }
        private System.Windows.Media.SolidColorBrush GetBrush(bool b)=> (b) ? System.Windows.Media.Brushes.Green : System.Windows.Media.Brushes.Red;


        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
