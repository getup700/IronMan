using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IronMan.Revit.Toolkit.Extension
{
    public static class ElementIdExtension
    {
        public static Element ConvertElement(this ElementId id, Document document)
        {
            try
            {
                return document.GetElement(id);
            }
            catch (Exception ex)
            {
                throw new Exception($"无法通过RevitDocument将ElementId转换为Element\t{ex.Source}\t{ex.Message}");
            }
        }
    }
}
