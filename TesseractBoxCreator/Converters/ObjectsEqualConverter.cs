using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using TesseractBoxCreator.Model;

namespace TesseractBoxCreator.Converters
{
    public class ObjectsEqualConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            BoxItem itemA = values[0] as BoxItem;
            BoxItem itemB = values[1] as BoxItem;

            return itemA == itemB;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
