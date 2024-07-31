using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace IronMan.Revit.Converters
{
    public class SizeConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var result = double.NaN;
            try
            {
                if (values == null || !values.Any()) return null;
                var length = (double)values[0];
                var width = (double)values[1];

                var variableLength = (double)values[2];
                if (variableLength == 0) return null;

                result = length * width / variableLength;

            }
            catch (Exception)
            {
                throw;
            }
            return result;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
