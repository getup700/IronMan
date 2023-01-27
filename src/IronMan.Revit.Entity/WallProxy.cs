using Autodesk.Revit.DB;
using IronMan.Revit.Toolkit.Extension;
using NPOI.OpenXmlFormats.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;

namespace IronMan.Revit.Entity
{
    public class WallProxy : ElementProxy
    {
        public Wall _wall;

        public WallProxy(Wall element) : base(element)
        {
            _wall = element;
        }

        public WallProxy()
        {

        }

        #region Properties
        //private double _typeName;
        //private double _thickness;
        //private Curve _locationCurve;
        //private ElementId _baseLevelId;
        //private double _baseOffset;
        //private ElementId _topLevelId;
        //private double _topOffset;

        public WallType TypeName
        {
            get => _wall.WallType;
            set { }
        }

        public double Thickness
        {
            get => _wall.WallType.Width.ConvertToMilliMeters();
            set => _wall.get_Parameter(BuiltInParameter.WALL_ATTR_WIDTH_PARAM).Set((double)value.ConvertToFeet());
        }

        public Curve LocationCurve
        {
            get => (_wall.Location as LocationCurve)?.Curve;
            set => LocationCurve = value;
        }

        public ElementId BaseLevelId
        {
            get => _wall.LevelId ?? ElementId.InvalidElementId;
            set=>_wall.get_Parameter(BuiltInParameter.WALL_BASE_CONSTRAINT).Set(value);
        }

        public double BaseOffset
        {
            get => _wall.get_Parameter(BuiltInParameter.WALL_BASE_OFFSET).AsDouble().ConvertToMilliMeters();
            set => _wall.get_Parameter(BuiltInParameter.WALL_BASE_OFFSET).Set((double)value.ConvertToFeet());
        }

        public ElementId TopLevelId
        {
            get => _wall.get_Parameter(BuiltInParameter.WALL_HEIGHT_TYPE).AsElementId() ?? ElementId.InvalidElementId;
            set => _wall.get_Parameter(BuiltInParameter.WALL_HEIGHT_TYPE).Set(value);
        }

        public double TopOffset
        {
            get => _wall.get_Parameter(BuiltInParameter.WALL_TOP_OFFSET).AsDouble();
            set => _wall.get_Parameter(BuiltInParameter.WALL_TOP_OFFSET).Set((double)value.ConvertToFeet());
        }

        public XYZ Direction => _wall.Orientation;
        #endregion



        public List<XYZ> GetIntersectPoints()
        {
            List<Wall> walls = _wall.GetIntersectElements(BuiltInCategory.OST_Walls).Select(x => x as Wall).ToList();
            Curve wallCurve = (_wall.Location as LocationCurve)?.Curve;
            List<XYZ> intersectPoints = new List<XYZ>();
            foreach (var item in walls)
            {
                XYZ intersectPoint = new XYZ();
                Curve curve = (item.Location as LocationCurve)?.Curve;
                IntersectionResultArray resultArray = new IntersectionResultArray();
                SetComparisonResult comparisonResult = wallCurve.Intersect(curve, out resultArray);
                if (comparisonResult == SetComparisonResult.Overlap)
                {
                    intersectPoints.Add(resultArray.get_Item(0).XYZPoint);
                }
            }
            XYZ endPoint0 = wallCurve.GetEndPoint(0);
            intersectPoints.Add(wallCurve.GetEndPoint(0));
            intersectPoints.Add(wallCurve.GetEndPoint(1));
            intersectPoints.Sort((x, y) => (x.DistanceTo(endPoint0).CompareTo(y.DistanceTo(endPoint0))));
            //intersectPoints.Where((p, i) => intersectPoints.FindIndex(x => x.Equal(p)) == i);
            //intersectPoints.Where((p, i) => intersectPoints.FindIndex(x => x.DistanceTo(p) < 100 / 304.8) == i);
            List<XYZ> points = intersectPoints.Where((x, i) => intersectPoints.FindIndex(z => z.DistanceTo(x) < 10 / 304.8) == i).ToList();
            return points;
        }

    }
}
