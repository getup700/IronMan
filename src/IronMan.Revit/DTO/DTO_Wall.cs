using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IronMan.Revit.DTO
{
    public class DTO_Wall:DTO_Base
    {
        public string FamilyName { get; set; }

        public string TypeName { get; set; }

        public double Thickness { get; set; }

        public Curve LocationCurve { get; set; }

        public double Height { get; set; }

        public ElementId BaseLevelId { get; set; }

        public double BaseOffset { get; set; }

        public ElementId TopLevelId { get; set; }

        public double TopOffset { get; set; }

        public XYZ Direction { get; set; }

        public bool IsFlip { get; set; }

        public bool IsStructural { get; set; }

        public int ReferenceId { get; set; }

    }
}
