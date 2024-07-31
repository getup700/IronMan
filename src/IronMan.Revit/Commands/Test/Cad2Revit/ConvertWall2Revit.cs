using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;
using Autodesk.Revit.UI;
using ConvertCAD.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IronMan.Revit.Commands.Test.Cad2Revit
{

    [Transaction(TransactionMode.Manual)]
    public class ConvertWall2Revit : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document document = uidoc.Document; // 获取当前软件、文档

            Reference refer = uidoc.Selection.PickObject(
                ObjectType.PointOnElement,
                "请拾取CAD链接中的墙");   // 拾取CAD链接

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

            List<WS> WSlist = new List<WS>();
            List<Line> Walloutlines = new List<Line>();

            foreach (GeometryObject gObj in wallsObjs)
            {
                Line wall = gObj as Line;
                Walloutlines.Add(wall);
            }


            for (int i = Walloutlines.Count - 1; i >= 0; i--)       //去除端线。
            {
                if (Walloutlines[i].Length < 201 / 304.8 && !ElementPositionUtils.IsEqual(Walloutlines[i].Length, 100) || ElementPositionUtils.IsEqual(Walloutlines[i].Length, 250))
                {
                    Walloutlines.RemoveAt(i);
                }
            }


            ElementPositionUtils.Gen_WSlist(Walloutlines, ref WSlist);

            for (int i = WSlist.Count - 1; i >= 0; i--)
            {
                for (int j = i - 1; j >= 0; j--)
                {
                    if (ElementPositionUtils.IsSameWS(WSlist[i], WSlist[j]))
                    {
                        WSlist.RemoveAt(i);
                        break;
                    }
                }
            }


            for (int i = WSlist.Count - 1; i >= 0; i--)
            {
                if (!ElementPositionUtils.IsEqual(WSlist[i].BL1.Direction.X, 0) && !ElementPositionUtils.IsEqual(WSlist[i].BL1.Direction.Y, 0))
                {
                    WSlist.RemoveAt(i);
                }
            }

            /*
            string ans = "";
            foreach (WS ws in WSlist)
            {
                ans += ws.BL1.GetEndPoint(0).X.ToString() + ", " + ws.BL1.GetEndPoint(0).Y.ToString() + ", " + ws.BL1.GetEndPoint(0).Z.ToString() + ",,, " +
                    ws.BL1.GetEndPoint(1).X.ToString() + ", " + ws.BL1.GetEndPoint(1).Y.ToString() + ", " + ws.BL1.GetEndPoint(1).Z.ToString() + "\n" +
                    ws.BL2.GetEndPoint(0).X.ToString() + ", " + ws.BL2.GetEndPoint(0).Y.ToString() + ", " + ws.BL2.GetEndPoint(0).Z.ToString() + ",,, " +
                    ws.BL2.GetEndPoint(1).X.ToString() + ", " + ws.BL2.GetEndPoint(1).Y.ToString() + ", " + ws.BL2.GetEndPoint(1).Z.ToString() + "\n\n";
            }
            TaskDialog.Show("H", ans);
            */

            List<Point> connectpoints = new List<Point>();

            foreach (WS ws1 in WSlist)
            {
                ElementPositionUtils.Cal_Wallaxis(ws1, ref ws1.X, ref ws1.Y);
                foreach (WS ws2 in WSlist)
                {
                    if (ws1 != ws2 && ElementPositionUtils.Is_neighbor(ws1, ws2))
                    {
                        ElementPositionUtils.Cal_Wallaxis(ws2, ref ws2.X, ref ws2.Y);
                        Point p = Point.Create(new XYZ(ws1.X + ws2.X, ws1.Y + ws2.Y, 0));
                        int index = ElementPositionUtils.NotExistsIn(p, connectpoints);
                        if (index == connectpoints.Count)
                        {
                            connectpoints.Add(p);
                            ws1.KnotIndexs.Add(connectpoints.Count - 1);
                            ws2.KnotIndexs.Add(connectpoints.Count - 1);
                        }
                        else
                        {
                            bool isExist = false;
                            foreach (int i in ws1.KnotIndexs)
                            {
                                if (i == index)
                                {
                                    isExist = true;
                                }
                            }
                            if (!isExist) ws1.KnotIndexs.Add(index);

                            isExist = false;
                            foreach (int i in ws2.KnotIndexs)
                            {
                                if (i == index)
                                {
                                    isExist = true;
                                }
                            }
                            if (!isExist) ws2.KnotIndexs.Add(index);
                        }
                    }
                }
            }




            //ElementPositionUtils.PrintLineList(WallAxis);
            Transaction trans = new Transaction(document, "生成墙");
            trans.Start();
            Level level = document.ActiveView.GenLevel;
            foreach (WS ws in WSlist)
            {
                if (ws.KnotIndexs.Count == 0 || ws.KnotIndexs.Count == 1)
                {
                    var p1 = ws.BL1.GetEndPoint(0);
                    var p2 = ws.BL1.GetEndPoint(1);
                    var p3 = ws.BL2.GetEndPoint(0);
                    var p4 = ws.BL2.GetEndPoint(1);
                    Point point = null;
                    if (ElementPositionUtils.IsEqual(ws.BL1.Direction.X, 0))
                    {
                        if (ElementPositionUtils.IsEqual(p1.Y, p3.Y) || ElementPositionUtils.IsEqual(p1.Y, p4.Y))
                        {
                            point = Point.Create(new XYZ((p1.X + p3.X) / 2, p1.Y, 0));
                            connectpoints.Add(point);
                            ws.KnotIndexs.Add(connectpoints.Count - 1);
                        }
                        if (ElementPositionUtils.IsEqual(p2.Y, p3.Y) || ElementPositionUtils.IsEqual(p2.Y, p4.Y))
                        {
                            point = Point.Create(new XYZ((p1.X + p3.X) / 2, p2.Y, 0));
                            connectpoints.Add(point);
                            ws.KnotIndexs.Add(connectpoints.Count - 1);
                        }
                    }
                    else
                    {
                        if (ElementPositionUtils.IsEqual(p1.X, p3.X) || ElementPositionUtils.IsEqual(p1.X, p4.X))
                        {
                            point = Point.Create(new XYZ(p1.X, (p1.Y + p3.Y) / 2, 0));
                            connectpoints.Add(point);
                            ws.KnotIndexs.Add(connectpoints.Count - 1);
                        }
                        if (ElementPositionUtils.IsEqual(p2.X, p3.X) || ElementPositionUtils.IsEqual(p2.X, p4.X))
                        {
                            point = Point.Create(new XYZ(p2.X, (p1.Y + p3.Y) / 2, 0));
                            connectpoints.Add(point);
                            ws.KnotIndexs.Add(connectpoints.Count - 1);
                        }
                    }
                }

                List<Point> Knots = new List<Point>();
                double d = ElementPositionUtils.L2L_distance(ws.BL1, ws.BL2);
                foreach (int index in ws.KnotIndexs)
                {
                    Knots.Add(connectpoints[index]);
                    ElementPositionUtils.SortByCoord(ref Knots);
                    for (int j = 0; j < Knots.Count - 1; j++)
                    {
                        Line WallAxe = Line.CreateBound(Knots[j].Coord, Knots[j + 1].Coord);
                        if (WallAxe.Length > 126 / 304.8 && (ElementPositionUtils.IsEqual(WallAxe.Direction.X, 0) || ElementPositionUtils.IsEqual(WallAxe.Direction.Y, 0)))
                        {
                            ElementPositionUtils.CreateWall(document, WallAxe, level, d, 4000 / 304.8, 0);
                        }
                    }
                }
            }
            trans.Commit();
            return Result.Succeeded;
        }
    }

}
