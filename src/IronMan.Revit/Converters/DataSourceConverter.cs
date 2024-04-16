using IronMan.Revit.Enum;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace IronMan.Revit.Converters
{
    public class DataSourceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var result = string.Empty;
            Source source = (Source)value;
            switch (source)
            {
                case Source.Document:
                    result = "全局管理";
                    break;
                case Source.ActiveView:
                    result = "当前视图";
                    break;
                default:
                    result = "数据错误";
                    break;
            }
            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
