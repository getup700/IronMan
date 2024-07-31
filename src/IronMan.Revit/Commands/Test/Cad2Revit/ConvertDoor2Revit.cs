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
    public class ConvertDoor2Revit : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document document = uidoc.Document; // 获取当前软件、文档

            Reference refer = uidoc.Selection.PickObject(
                ObjectType.PointOnElement,
                "请拾取CAD链接中的门");   // 拾取CAD链接

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

            List<GeometryObject> doorsObjs = new List<GeometryObject>();

            foreach (GeometryObject gObj in geoElem)
            {
                //只取第一个元素
                GeometryInstance geomInstance = gObj as GeometryInstance;
                Transform totalTransform = geomInstance.Transform;
                foreach (var insObj in geomInstance.SymbolGeometry)
                {
                    Category insCategory = (document.GetElement(insObj.GraphicsStyleId) as GraphicsStyle).GraphicsStyleCategory;
                    if (targetCategory.Id == insCategory.Id)
                    {
                        if (insObj.GetType().ToString() == "Autodesk.Revit.DB.GeometryInstance")
                        {
                            GeometryInstance geom = insObj as GeometryInstance;
                            Transform transform = geom.Transform;
                            List<XYZ> units = new List<XYZ>();
                            foreach (var unit in geom.SymbolGeometry)
                            {
                                if (unit.GetType().ToString() == "Autodesk.Revit.DB.Arc")
                                {
                                    Arc arc = unit as Arc;
                                    units.Add(totalTransform.OfPoint(transform.OfPoint(arc.GetEndPoint(0))));
                                    units.Add(totalTransform.OfPoint(transform.OfPoint(arc.GetEndPoint(1))));
                                }
                                else if (unit.GetType().ToString() == "Autodesk.Revit.DB.Line")
                                {
                                    Line line = unit as Line;
                                    units.Add(totalTransform.OfPoint(transform.OfPoint(line.GetEndPoint(0))));
                                    units.Add(totalTransform.OfPoint(transform.OfPoint(line.GetEndPoint(1))));
                                }
                            }

                            List<XYZ> isolatePoint = new List<XYZ>();
                            foreach (XYZ p1 in units)
                            {
                                int count = 0;
                                foreach (XYZ p2 in units)
                                {
                                    if (ElementPositionUtils.IsSameXYZ(p1, p2))
                                    {
                                        count++;
                                    }
                                }
                                if (1 == count)
                                {
                                    isolatePoint.Add(p1);
                                }
                            }
                            doorsObjs.Add(Line.CreateBound(isolatePoint[0], isolatePoint[1]));
                        }
                        else
                        {
                            doorsObjs.Add(insObj);
                        }
                    }
                }
            }

            refer = uidoc.Selection.PickObject(
                ObjectType.PointOnElement,
                "请拾取CAD链接中的墙");   // 拾取CAD链接

            element = document.GetElement(refer);
            geoElem = element.get_Geometry(new Options());
            geoObj = element.GetGeometryObjectFromReference(refer);

            targetCategory = null;    // 获取Category

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

            List<GeometryObject> wallsObjs = new List<GeometryObject>();

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
                            IList<XYZ> endpoints = polyLine.GetCoordinates();
                            for (int i = 1; i < endpoints.Count; i++)
                            {
                                wallsObjs.Add(Line.CreateBound(
                                    transform.OfPoint(endpoints[i - 1]),
                                    transform.OfPoint(endpoints[i])
                                    ));
                            }
                        }
                        else
                        {
                            Line line = insObj as Line;
                            wallsObjs.Add(
                                Line.CreateBound(
                                    transform.OfPoint(line.GetEndPoint(0)),
                                    transform.OfPoint(line.GetEndPoint(1))
                                )
                            );
                        }
                    }
                }
            }

            Transaction trans = new Transaction(document, "生成门");
            trans.Start();
            Level level = document.ActiveView.GenLevel;
            foreach (GeometryObject geo in doorsObjs)
            {
                double width = 0;
                Line door = geo as Line;
                XYZ doorStart = door.GetEndPoint(0);
                XYZ doorEnd = door.GetEndPoint(1);
                foreach (GeometryObject gObj in wallsObjs)
                {
                    Line wallLine = gObj as Line;
                    if (ElementPositionUtils.IsEqual(0, wallLine.Distance(doorStart)) ||
                        ElementPositionUtils.IsEqual(0, wallLine.Distance(doorEnd)))
                    {
                        width = width == 0 ? wallLine.Length : Math.Min(width, wallLine.Length);
                    }
                }
                Wall wall = ElementPositionUtils.CreateWall(document, door, level, width, 4000 / 304.8, 0);
                ElementPositionUtils.CreateDoor(document, doorStart.Add(doorEnd) / 2, door.Length, 2200 / 304.8, wall, level);
            }
            trans.Commit();
            return Result.Succeeded;
        }
    }

}
