using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IronMan.Revit.DTO
{
    public class DTO_Material:DTO_Base
    {
        public Color Color { get; set; }
        public Color AppearanceColor { get; set; }
    }
}
