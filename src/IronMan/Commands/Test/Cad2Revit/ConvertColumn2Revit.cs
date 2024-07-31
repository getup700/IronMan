using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IronMan.Revit.Commands.Test.Cad2Revit
{
    [Transaction(TransactionMode.Manual)]
    public class ConvertColumn2Revit : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document document = uidoc.Document; // 获取当前软件、文档

            Reference refer = uidoc.Selection.PickObject(
                ObjectType.PointOnElement,
                "请拾取CAD链接中的柱");   // 拾取CAD链接

            Element element = document.GetElement(refer);
            GeometryElement geoElem = element.get_Geometry(new Options());
            GeometryObject geoObj = element.GetGeometryObjectFromReference(refer);

            Category targetCategory = null;    // 获取Category

            if (geoObj.GraphicsStyleId != ElementId.InvalidElementId)
            {
                if (document.GetElement(geoObj.GraphicsStyleId) is GraphicsStyle gs)
                {
                    targetCategory = gs.GraphicsStyleCategory;
                }
            }

            if (targetCategory == null)
            {
                return Result.Failed;   // 未匹配
            }

            Transaction trans = new Transaction(document, "创建柱");
            trans.Start();
            Level level = document.ActiveView.GenLevel;
            foreach (GeometryObject gObj in geoElem)
            {
                //只取第一个元素
                GeometryInstance geomInstance = gObj as GeometryInstance;
                Transform transform = geomInstance.Transform;
                foreach (var insObj in geomInstance.SymbolGeometry)
                {
                    Category insCategory = (document.GetElement(insObj.GraphicsStyleId) as GraphicsStyle).GraphicsStyleCategory;
                    if (targetCategory.Id == insCategory.Id)
                    {
                        if (insObj.GetType().ToString() == "Autodesk.Revit.DB.PolyLine")
                        {
                            PolyLine polyLine = insObj as PolyLine;
                            XYZ pMax = polyLine.GetOutline().MaximumPoint;
                            XYZ pMin = polyLine.GetOutline().MinimumPoint;
                            double b = Math.Abs(pMin.X - pMax.X);
                            double h = Math.Abs(pMin.Y - pMax.Y);
                            XYZ pp = pMax.Add(pMin) / 2; // 柱中心点
                            pp = transform.OfPoint(pp);
                            ElementPositionUtils.CreateColumn(document, pp, level, b, h);
                        }
                    }
                }
            }
            trans.Commit();
            return Result.Succeeded;
        }
    }

}
