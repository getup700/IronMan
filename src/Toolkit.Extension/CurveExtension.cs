using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IronMan.Revit.Toolkit.Extension
{
    public static class CurveExtension
    {
        public static XYZ IntersectionPoint(this Curve curveSource,Curve curve)
        {
            IntersectionResultArray intersectionResultArray = new IntersectionResultArray();
            SetComparisonResult comparisonResult;
            comparisonResult = curveSource.Intersect(curve, out intersectionResultArray);
            XYZ intersectionResult = null;
            if(SetComparisonResult.Disjoint!=comparisonResult)
            {
                try
                {
                    if (!intersectionResultArray.IsEmpty)
                    {
                        intersectionResult = intersectionResultArray.get_Item(0).XYZPoint;
                    }
                }
                catch 
                {
                    return null;
                }
            }
            return intersectionResult;
        }
    }
}
