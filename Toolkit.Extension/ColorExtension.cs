using RevitColor = Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Toolkit.Extension
{
    public static class ColorExtension
    {
        public static RevitColor.Color ConvertToRevitColor(this Color color)
        {
             return new RevitColor.Color(color.R,color.G,color.B);
        }
    }
}
