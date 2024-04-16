using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using IronMan.Revit.Interfaces;
using IronMan.Revit.Toolkit.Extension.Class;
using IronMan.Revit.Toolkit.Mvvm.Interfaces;
using IronMan.Revit.Toolkit.Mvvm.IOC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using IronMan.Revit.Toolkit.Extension;

namespace IronMan.Revit.Services
{
    public class CadToRevitService
    {
        public CadToRevitService(IDataContext dataContext)
        {
            _dataContext = dataContext;
        }

        private IDataContext _dataContext;
        public Document Document => _dataContext.GetDocument();
        public Autodesk.Revit.ApplicationServices.Application Application => _dataContext.GetUIApplication().Application;

        public void SelectCad(Reference reference)
        {
            try
            {
                Element element = Document.GetElement(reference);
                GeometryObject geometryObject = element.GetGeometryObjectFromReference(reference);
                ImportInstance importInstance = element as ImportInstance;
                //GeometryElement geometryElement = importInstance.get_Geometry(new Options());
                //var transform = geometryElement.Select(x => (x as GeometryInstance).Transform);
                //MessageBox.Show("",geometryElement.Count().ToString());
                Transform transform = importInstance.GetTransform();
                XYZ location = transform.Origin;

                PolyLine polyLine = geometryObject as PolyLine;

                CurveArray curveArray = new CurveArray();
                CurveArrArray curveArrArray = new CurveArrArray();
                IList<XYZ> listPoint = polyLine.GetCoordinates();
                for (int i = 0; i < listPoint.Count - 2; i++)
                {
                    Curve curve = Line.CreateBound(listPoint[i], listPoint[i + 1]);
                    curveArray.Append(curve);
                }
                curveArrArray.Append(curveArray);
                string rftPaht = GetRtfPath();
                CurveArray rftArray = new CurveArray();
                rftArray.Append(Line.CreateBound(new XYZ(0, 0, 0), new XYZ(50, 0, 0)));
                rftArray.Append(Line.CreateBound(new XYZ(50, 0, 0), new XYZ(50, 100, 0)));
                rftArray.Append(Line.CreateBound(new XYZ(50, 100, 0), new XYZ(0, 100, 0)));
                rftArray.Append(Line.CreateBound(new XYZ(0, 100, 0), new XYZ(0, 0, 0)));
                CurveArrArray curveArrArray2= new CurveArrArray();
                curveArrArray2.Append(rftArray);
                CreateColumn(rftPaht, curveArrArray2);
            }
            catch
            {
                SingletonIOC.Current.Container.Unregister<Document>();
            }
        }

        public Reference SelectReference()
        {
            return _dataContext.GetUIDocument().Selection.PickObject(
                Autodesk.Revit.UI.Selection.ObjectType.PointOnElement, "Select");
        }

        private string GetRtfPath()
        {
            string rtfPath = @"C:\ProgramData\Autodesk\RVT 2020\Family Templates\Chinese\公制结构柱.rft";
            string path = _dataContext.GetUIApplication().Application.FamilyTemplatePath + "\\公制结构柱.rft";
            if (rtfPath == null)
            {
                MessageBox.Show("请检查“公制结构柱”族路径是否正确");
                return null;
            }
            return rtfPath;
        }


        private void CreateColumn(string path, CurveArrArray curveArrArray)
        {

            Document familyDocument = _dataContext.GetUIApplication().Application.NewFamilyDocument(path);
            //XYZ origin = XYZ.Zero;
            //XYZ normal = XYZ.BasisZ;
            //Plane plane = Plane.CreateByNormalAndOrigin(normal, origin);
            //SketchPlane sketchPlane = SketchPlane.Create(familyDocument, plane);
            Element element = new FilteredElementCollector(familyDocument).OfClass(typeof(Level)).FirstOrDefault();
            Level level = element as Level;
            familyDocument.NewTransaction(() =>
            {
                SketchPlane sketchPlane = SketchPlane.Create(familyDocument, level.GetPlaneReference());
                familyDocument.FamilyCreate.NewExtrusion(true, curveArrArray, sketchPlane, 1000);
                familyDocument.FamilyManager.NewType("type1");
                familyDocument.FamilyManager.AddParameter("材质", BuiltInParameterGroup.PG_MATERIALS, ParameterType.Material, false);
            },"创建族");
            familyDocument.SaveAs("NewType.rfa");
            familyDocument.Close();
        }
    }
}
