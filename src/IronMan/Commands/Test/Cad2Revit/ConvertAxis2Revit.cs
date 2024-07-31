using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IronMan.Revit.Utils;

namespace IronMan.Revit.Commands.Test.Cad2Revit
{

    [Transaction(TransactionMode.Manual)]
    public class ConvertAxis2Revit : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document document = uidoc.Document; // 获取当前软件、文档

            Reference refer = uidoc.Selection.PickObject(
                ObjectType.PointOnElement,
                "请拾取CAD链接中的轴线");   // 拾取CAD链接

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

            List<GeometryObject> axesObjs = new List<GeometryObject>();

            foreach (GeometryObject gObj in geoElem)
            {
                //只取第一个元素
                GeometryInstance geomInstance = gObj as GeometryInstance;
                Transform transform = geomInstance.Transform;
                foreach (var insObj in geomInstance.SymbolGeometry)
                {
                    Category insCategory = (document.GetElement(insObj.GraphicsStyleId) as GraphicsStyle).GraphicsStyleCategory;
                    Console.WriteLine(insCategory.Name);
                    if (targetCategory.Id == insCategory.Id)
                    {
                        if (insObj.GetType().ToString() == "Autodesk.Revit.DB.PolyLine")
                        {
                            PolyLine polyLine = insObj as PolyLine;
                            IList<XYZ> endpoints = polyLine.GetCoordinates();
                            for (int i = 1; i < endpoints.Count; i++)
                            {
                                axesObjs.Add(Line.CreateBound(
                                    transform.OfPoint(endpoints[i - 1]),
                                    transform.OfPoint(endpoints[i])
                                    ));
                            }
                        }
                        else
                        {
                            Line line = insObj as Line;
                            axesObjs.Add(
                                Line.CreateBound(
                                    transform.OfPoint(line.GetEndPoint(0)),
                                    transform.OfPoint(line.GetEndPoint(1))
                                )
                            );
                        }
                    }
                }
            }

            Transaction trans = new Transaction(document, "生成轴网");
            trans.Start();
            ElementPositionUtils.CreateAxes(document, axesObjs);
            trans.Commit();
            return Result.Succeeded;
        }
    }

}
