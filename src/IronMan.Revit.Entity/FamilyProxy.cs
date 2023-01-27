using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IronMan.Revit.Entity
{
    public class FamilyProxy:ElementProxy
    {
        private FamilyInstance _floor;
        public FamilyProxy(FamilyInstance element)
        {
            _floor = element;
            BaseOffset = _floor.get_Parameter(BuiltInParameter.INSTANCE_FREE_HOST_OFFSET_PARAM).AsDouble();
        }

        public FamilyProxy()
        {

        }

        public IEnumerable<XYZ> LocationPoints { get; set; } = new List<XYZ>();

        public Level Level { get; set; }

        public double BaseOffset { get; set; }
    }
}
