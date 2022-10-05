using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace IronMan.Revit.Converters
{
    public class ColorConverter : IValueConverter
    {
        /// <summary>
        /// RevitColor转换为WindowsColor
        /// </summary>
        /// <param name="value">转换前的值</param>
        /// <param name="targetType">一个数据对应多个数据源时使用</param>
        /// <param name="parameter">无法直接绑定，</param>
        /// <param name="culture">如果中文/英文时考虑</param>
        /// <returns>转换后的值</returns>
        public  object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is Autodesk.Revit.DB.Color color)
            {
               return ColorTranslator.ToHtml(System.Drawing.Color.FromArgb(color.Red, color.Green, color.Blue));
            }
            else
                return null;

        }
        /// <summary>
        /// WindowsColor转换为RevitColor
        /// </summary>
        /// <param name="value">转换后的值</param>
        /// <param name="targetType">一个数据对应多个数据源时使用</param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns>转换前的值</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
