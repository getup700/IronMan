using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IronMan.Revit.Toolkit.Extension
{
    public static class ParameterFilterElementExtension
    {
        public static ElementFilter GetElementFilter(this ParameterFilterElement element) =>
           element.GetElementFilter();
    }
}
