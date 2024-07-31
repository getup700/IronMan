using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IronMan.Revit.DTO
{
    public class DTO_Family:DTO_Base
    {
        public Level Level { get; set; }

        public double BaseOffset { get; set; }

        public  List<XYZ> LocationPoints { get; set; }
    }
}
