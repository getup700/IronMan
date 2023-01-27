using Autodesk.Revit.DB;
using NPOI.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IronMan.Revit.Toolkit.Extension.Class
{
    public class TransientElementMaker : ITransientElementMaker
    {
        private Action _action;

        public TransientElementMaker(Action action)
        {
            _action = action;
        }

        public void Execute()
        {
            _action.Invoke();
        }

        private void CreateElement(Document document)
        {
            List<Curve> profile = new List<Curve>();

            // first create sphere with 2' radius
            XYZ center = XYZ.Zero;
            double radius = 2;
            XYZ profile00 = center;
            XYZ profilePlus = center + new XYZ(0, radius, 0);
            XYZ profileMinus = center - new XYZ(0, radius, 0);

            profile.Add(Line.CreateBound(profilePlus, profileMinus));
            profile.Add(Arc.Create(profileMinus, profilePlus, center + new XYZ(radius, 0, 0)));

            CurveLoop curveLoop = CurveLoop.Create(profile);
            SolidOptions options = new SolidOptions(document.GetElements<Material>().FirstOrDefault().Id, document.GetElements<GraphicsStyle>().FirstOrDefault().Id);

            Frame frame = new Frame(center, XYZ.BasisX, XYZ.BasisY, XYZ.BasisZ);




            List<Curve> curves = new List<Curve>();
            curves.Add(Line.CreateBound(XYZ.Zero, new XYZ(1000, 0, 0)) as Curve);
            curves.Add(Line.CreateBound(new XYZ(1000, 0, 0), new XYZ(1000, 1000, 0)) as Curve);
            curves.Add(Line.CreateBound(new XYZ(1000, 1000, 0), new XYZ(0, 1000, 0)) as Curve);
            curves.Add(Line.CreateBound(new XYZ(0, 1000, 0), XYZ.Zero) as Curve);

            CurveLoop curveLooppp = CurveLoop.Create(curves);
            List<CurveLoop> loop = new List<CurveLoop>() { curveLooppp };
            if (Frame.CanDefineRevitGeometry(frame) == true)
            {
                //Solid sphere = GeometryCreationUtilities.CreateRevolvedGeometry(frame, new CurveLoop[] { curveLoop }, 0, 2 * Math.PI, options);
                Solid sphere = GeometryCreationUtilities.CreateExtrusionGeometry(loop, XYZ.BasisZ, 1000);
                // create direct shape and assign the sphere shape
                DirectShape ds = DirectShape.CreateElement(document, new ElementId(BuiltInCategory.OST_GenericModel));

                ds.ApplicationId = "Application id";
                ds.ApplicationDataId = "Geometry object id";
                ds.SetShape(new GeometryObject[] { sphere });

            }
        }
    }
}
