using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UIFramework;

namespace IronMan.Revit.Toolkit.Extension
{
    public static class LineExtension
    {
        public static XYZ Perpendicular(this Line line,XYZ xyz)
        {
            XYZ unitVector = line.Direction.Normalize();
            //线段法线
            XYZ cross = unitVector.CrossProduct(XYZ.BasisZ);
            Line parallelLine = Line.CreateUnbound((xyz + cross.Multiply(10)), xyz);
            XYZ resultDirection = parallelLine.Direction.Normalize();
            XYZ intersectPoint = parallelLine.IntersectionPoint(line);
            return resultDirection;
        }
    }
}
