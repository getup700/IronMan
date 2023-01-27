using IronMan.Revit.Entity.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IronMan.Revit.Entity.Extensions
{
    public static class PushButtonDataExtension
    {
        public static string GetMethodNameByAttribute(this Type type, string name = "Execute")
        {
            var methodInfo = type.GetMethod(name);
            if (methodInfo != null && Attribute.IsDefined(methodInfo, typeof(ButtonNameAttribute)))
            {
                var attribute = Attribute.GetCustomAttribute(methodInfo, typeof(ButtonNameAttribute)) as ButtonNameAttribute;
                return attribute.Name;
            }
            return type.Name;
        }

        public static string GetMethodTooltipByAttribute(this Type type, string name = "Execute")
        {
            var methodToolTip = type.GetMethod(name);
            if (methodToolTip != null && Attribute.IsDefined(methodToolTip, typeof(ButtonNameAttribute)))
            {
                var attribute = Attribute.GetCustomAttribute(methodToolTip, typeof(ButtonNameAttribute)) as ButtonNameAttribute;
                return attribute.ToolTip;
            }
            return null;
        }

        public static string GetMethodNameByAttribute(this ButtonNameAttribute buttonNameAttribute,Type type, string name = "Execute")
        {
            var methodInfo = type.GetMethod(name);
            if (methodInfo != null && Attribute.IsDefined(methodInfo, buttonNameAttribute.GetType()))
            {
                var attribute = Attribute.GetCustomAttribute(methodInfo, typeof(ButtonNameAttribute)) as ButtonNameAttribute;
                return attribute.Name;
            }
            return type.Name;
        }
    }
}
