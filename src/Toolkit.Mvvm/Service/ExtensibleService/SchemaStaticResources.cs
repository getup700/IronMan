using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IronMan.Revit.Toolkit.Mvvm.Service.ExtensibleService
{
    public class SchemaStaticResources
    {
        public static bool IsAllowedType(Type type) => _types.Any(t => t == type);

        private static List<Type> _types { get; set; } = new List<Type>()
        {
            typeof(byte),
            typeof(int),
            typeof(double),
            typeof(float),
            typeof(short),
            typeof(bool),
            typeof(string),
            typeof(Guid),
            typeof(ElementId),
            typeof(XYZ),
            typeof(UV),
        };
    }
}
