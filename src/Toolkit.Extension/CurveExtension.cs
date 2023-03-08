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

        public static CurveLoop CloseCurve(this CurveLoop curveLoop)
        {
            CurveLoop newCurveLoop = new CurveLoop();
            if (!curveLoop.IsOpen())
            {
                return curveLoop;
            }
            var curves = new List<Curve>();
            foreach (Curve curve in curveLoop)
            {
                curve.MakeUnbound();
                curves.Add(curve);
            }
            var points = new List<XYZ>();
            for (int i = 0; i < curves.Count; i++)
            {
                XYZ intersection = new XYZ();
                if (i + 1 != curves.Count)
                {
                    intersection = curves[i].GetIntersection(curves[i + 1]);
                }
                else
                {
                    intersection = curves[i].GetIntersection(curves[0]);
                }
                points.Add(intersection);
            }
            for (int i = 0; i < points.Count; i++)
            {
                if (i + 1 != points.Count)
                {
                    newCurveLoop.Append(Line.CreateBound(points[i], points[i + 1]));
                }
                else
                {
                    newCurveLoop.Append(Line.CreateBound(points[i + 1], points[0]));
                }
            }
            return newCurveLoop.IsOpen() ? null : newCurveLoop;
        }


        public static XYZ GetIntersection(this Curve curve, Curve curve1)
        {
            IntersectionResultArray resultArray = new IntersectionResultArray();
            SetComparisonResult comparisonResult = curve.Intersect(curve1, out resultArray);
            XYZ intersectPoint = new XYZ();
            //if there are intersections
            if (SetComparisonResult.Disjoint != comparisonResult)
            {
                if (!resultArray.IsEmpty)
                {
                    intersectPoint = resultArray.get_Item(0).XYZPoint;
                }
            }
            return intersectPoint;
        }
    }
}
