using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IronMan.Revit.Toolkit.Extension
{
    public static class ElementExtension
    {
        public static Parameter LookupParameter(this Element element, ElementId parameterId)
        {
            foreach (Parameter parameter in element.Parameters)
            {
                if (parameter.Id == parameterId)
                {
                    return parameter;
                }
            }
            return null;
        }
    }
}
