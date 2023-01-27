using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Mechanical;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IronMan.Revit.Toolkit.Extension
{
    public static class XYZExtension
    {
        public static bool Equal(this XYZ point, XYZ p)
        {
            if (p == null) return false;
            if (point.DistanceTo(p) > 1000) return false;
            return true;

        }

        public static XYZ MetricXYZ(this XYZ xyz)
        {
            return new XYZ(xyz.X.ConvertToMilliMeters(), xyz.Y.ConvertToMilliMeters(), xyz.Z.ConvertToMilliMeters());
        }
    }
}
