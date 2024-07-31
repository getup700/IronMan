using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace IronMan.Revit.Converters
{
    public class VisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return null;
            if (value is not bool) return null;
            bool temp = (bool)value;
            Visibility result;
            if(temp)
            {
                result=Visibility.Visible;
            }
            else
            {
                result=Visibility.Collapsed;
            }
            return result;
            //bool boolean;
            //bool flag;
            //if (value is bool)
            //{
            //    boolean = (bool)value;
            //    flag = true;
            //}
            //else
            //{
            //    flag = false;
            //}
            //bool flag2 = flag;
            //object result;
            //if (flag2)
            //{
            //    result = (boolean ? Visibility.Visible : Visibility.Collapsed);
            //}
            //else
            //{
            //    DialogModel model;
            //    bool flag3;
            //    if (value is DialogModel)
            //    {
            //        model = (DialogModel)value;
            //        flag3 = true;
            //    }
            //    else
            //    {
            //        flag3 = false;
            //    }
            //    bool flag4 = flag3;
            //    if (flag4)
            //    {
            //        result = ((model == DialogModel.Creator) ? Visibility.Visible : Visibility.Collapsed);
            //    }
            //    else
            //    {
            //        result = Visibility.Visible;
            //    }
            //}
            //return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
