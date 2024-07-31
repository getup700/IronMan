using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI.Selection;
using ConvertCAD.Models;

namespace IronMan.Revit.Commands.Test.Cad2Revit
{
    [Transaction(TransactionMode.Manual)]
    public class ConvertCAD2Revit : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document document = uidoc.Document; // 获取当前软件、文档

            Reference refer = uidoc.Selection.PickObject(
                ObjectType.Element,
                "选择CAD链接图层");   // 拾取CAD链接

            Element element = document.GetElement(refer);
            GeometryObject geoObj = element.GetGeometryObjectFromReference(refer);

            Category cadLinkCategory = null;    // 获取父Category

            if (geoObj.GraphicsStyleId != ElementId.InvalidElementId)
            {
                if (document.GetElement(geoObj.GraphicsStyleId) is GraphicsStyle gs)
                {
                    cadLinkCategory = gs.GraphicsStyleCategory;
                }
            }

            if (cadLinkCategory == null)
            {
                return Result.Failed;   // 未匹配
            }

            Category wallsCategory = null;
            Category windowsCategory = null;
            Category doorsCategory = null;
            Category axesCategory = null;
            Category columnsCategory = null;

            List<GeometryObject> wallsObjs = new List<GeometryObject>();
            List<GeometryObject> windowsObjs = new List<GeometryObject>();
            List<GeometryObject> doorsObjs = new List<GeometryObject>();
            List<GeometryObject> axesObjs = new List<GeometryObject>();

            foreach (Category subCategory in cadLinkCategory.SubCategories)
            {
                // 匹配各图层名称
                if ("墙" == subCategory.Name ||
                    "WALL" == subCategory.Name ||
                    "Wall" == subCategory.Name ||
                    "wall" == subCategory.Name)
                {
                    wallsCategory = subCategory;
                }

                if ("门" == subCategory.Name ||
                    "DOOR" == subCategory.Name ||
                    "Door" == subCategory.Name ||
                    "door" == subCategory.Name)
                {
                    doorsCategory = subCategory;
                }

                if ("窗" == subCategory.Name ||
                    "WINDOW" == subCategory.Name ||
                    "Window" == subCategory.Name ||
                    "window" == subCategory.Name)
                {
                    windowsCategory = subCategory;
                }

                if ("轴线" == subCategory.Name ||
                    "AXIS" == subCategory.Name ||
                    "Axis" == subCategory.Name ||
                    "axis" == subCategory.Name)
                {
                    axesCategory = subCategory;
                }

                if ("柱" == subCategory.Name ||
                    "柱子" == subCategory.Name ||
                    "COLUMN" == subCategory.Name ||
                    "Column" == subCategory.Name ||
                    "column" == subCategory.Name)
                {
                    columnsCategory = subCategory;
                }
            }

            GeometryElement geometryElement = element.get_Geometry(new Options()); // 获取主图元

            Transaction trans = new Transaction(document, "生成柱");
            trans.Start();
            Level level = document.ActiveView.GenLevel;
            foreach (GeometryObject gObj in geometryElement)
            {
                //只取第一个元素
                GeometryInstance geomInstance = gObj as GeometryInstance;
                Transform totalTransform = geomInstance.Transform;

                foreach (var insObj in geomInstance.SymbolGeometry)
                {
                    Category insCategory = (document.GetElement(insObj.GraphicsStyleId) as GraphicsStyle).GraphicsStyleCategory;
                    if (wallsCategory != null && wallsCategory.Id == insCategory.Id)
                    {
                        if (insObj.GetType().ToString() == "Autodesk.Revit.DB.PolyLine")
                        {
                            PolyLine polyLine = insObj as PolyLine;
                            IList<XYZ> endpoints = polyLine.GetCoordinates();
                            for (int i = 1; i < endpoints.Count; i++)
                            {
                                wallsObjs.Add(Line.CreateBound(
                                    totalTransform.OfPoint(endpoints[i - 1]),
                                    totalTransform.OfPoint(endpoints[i])
                                    ));
                            }
                        }
                        else
                        {
                            Line line = insObj as Line;
                            wallsObjs.Add(
                                Line.CreateBound(
                                    totalTransform.OfPoint(line.GetEndPoint(0)),
                                    totalTransform.OfPoint(line.GetEndPoint(1))
                                )
                            );
                        }
                    }
                    else if (windowsCategory != null && windowsCategory.Id == insCategory.Id)
                    {
                        if (insObj.GetType().ToString() == "Autodesk.Revit.DB.PolyLine")
                        {
                            PolyLine polyLine = insObj as PolyLine;
                            IList<XYZ> endpoints = polyLine.GetCoordinates();
                            for (int i = 1; i < endpoints.Count; i++)
                            {
                                windowsObjs.Add(Line.CreateBound(
                                    totalTransform.OfPoint(endpoints[i - 1]),
                                    totalTransform.OfPoint(endpoints[i])
                                    ));
                            }
                        }
                        else
                        {
                            Line line = insObj as Line;
                            windowsObjs.Add(
                                Line.CreateBound(
                                    totalTransform.OfPoint(line.GetEndPoint(0)),
                                    totalTransform.OfPoint(line.GetEndPoint(1))
                                )
                            );
                        }
                    }
                    else if (doorsCategory != null && doorsCategory.Id == insCategory.Id)
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
                    else if (axesCategory != null && axesCategory.Id == insCategory.Id)
                    {
                        if (insObj.GetType().ToString() == "Autodesk.Revit.DB.PolyLine")
                        {
                            PolyLine polyLine = insObj as PolyLine;
                            IList<XYZ> endpoints = polyLine.GetCoordinates();
                            for (int i = 1; i < endpoints.Count; i++)
                            {
                                axesObjs.Add(Line.CreateBound(
                                    totalTransform.OfPoint(endpoints[i - 1]),
                                    totalTransform.OfPoint(endpoints[i])
                                    ));
                            }
                        }
                        else
                        {
                            Line line = insObj as Line;
                            axesObjs.Add(
                                Line.CreateBound(
                                    totalTransform.OfPoint(line.GetEndPoint(0)),
                                    totalTransform.OfPoint(line.GetEndPoint(1))
                                )
                            );
                        }
                    }
                    else if (columnsCategory != null && columnsCategory.Id == insCategory.Id)
                    {
                        if (insObj.GetType().ToString() == "Autodesk.Revit.DB.PolyLine")
                        {
                            PolyLine polyLine = insObj as PolyLine;
                            XYZ pMax = polyLine.GetOutline().MaximumPoint;
                            XYZ pMin = polyLine.GetOutline().MinimumPoint;
                            double b = Math.Abs(pMin.X - pMax.X);
                            double h = Math.Abs(pMin.Y - pMax.Y);
                            XYZ pp = pMax.Add(pMin) / 2; // 柱中心点
                            pp = totalTransform.OfPoint(pp);

                            ElementPositionUtils.CreateColumn(document, pp, level, b, h);

                        }
                    }
                }
            }
            trans.Commit();

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

            trans = new Transaction(document, "生成墙");
            trans.Start();
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

            List<Line> horizontalWindows = new List<Line>();
            List<Line> verticalWindows = new List<Line>();

            foreach (GeometryObject windowsObj in windowsObjs)
            {
                Line window = windowsObj as Line;
                if (ElementPositionUtils.IsEqual(window.GetEndPoint(0).X, window.GetEndPoint(1).X))
                {
                    verticalWindows.Add(window);
                }
                else if (ElementPositionUtils.IsEqual(window.GetEndPoint(0).Y, window.GetEndPoint(1).Y))
                {
                    horizontalWindows.Add(window);
                }
            }
            for (int i = 0; i < horizontalWindows.Count - 1; i++)
            {
                for (int j = 0; j < horizontalWindows.Count - 1 - i; j++)
                {
                    if (horizontalWindows[j].GetEndPoint(0).Y >
                        horizontalWindows[j + 1].GetEndPoint(1).Y)
                    {
                        var temp = horizontalWindows[j + 1];
                        horizontalWindows[j + 1] = horizontalWindows[j];
                        horizontalWindows[j] = temp;
                    }
                }
            }
            for (int i = 0; i < verticalWindows.Count - 1; i++)
            {
                for (int j = 0; j < verticalWindows.Count - 1 - i; j++)
                {
                    if (verticalWindows[j].GetEndPoint(0).X >
                        verticalWindows[j + 1].GetEndPoint(1).X)
                    {
                        var temp = verticalWindows[j + 1];
                        verticalWindows[j + 1] = verticalWindows[j];
                        verticalWindows[j] = temp;
                    }
                }
            }
            List<List<Line>> verticalSplitWindows = new List<List<Line>>();
            foreach (Line verticalWindow in verticalWindows)
            {
                bool is_exist = false;
                foreach (List<Line> verticalSplitWindow in verticalSplitWindows)
                {
                    if (verticalSplitWindow.Count > 0 &&
                       ElementPositionUtils.IsSameY(verticalWindow, verticalSplitWindow[0]) &&
                       ElementPositionUtils.Distance(verticalWindow, verticalSplitWindow[0]) < 500 / 304.8)
                    {
                        is_exist = true;
                        verticalSplitWindow.Add(verticalWindow);
                    }
                }
                if (!is_exist)
                {
                    List<Line> verticalSplitWindow = new List<Line>
                    {
                        verticalWindow
                    };
                    verticalSplitWindows.Add(verticalSplitWindow);
                }
            }
            List<List<Line>> horizontalSplitWindows = new List<List<Line>>();
            foreach (Line horizontalWindow in horizontalWindows)
            {
                bool is_exist = false;
                foreach (List<Line> horizontalSplitWindow in horizontalSplitWindows)
                {
                    if (horizontalSplitWindow.Count > 0 &&
                       ElementPositionUtils.IsSameX(horizontalWindow, horizontalSplitWindow[0]) &&
                       ElementPositionUtils.Distance(horizontalWindow, horizontalSplitWindow[0]) < 500 / 304.8)
                    {
                        is_exist = true;
                        horizontalSplitWindow.Add(horizontalWindow);
                    }
                }
                if (!is_exist)
                {
                    List<Line> horizontalSplitWindow = new List<Line>
                    {
                        horizontalWindow
                    };
                    horizontalSplitWindows.Add(horizontalSplitWindow);
                }
            }

            /*
            string ans = "";
            foreach(List<Line> horizontalSplitWindow in horizontalSplitWindows)
            {
                foreach(Line horizontalS in horizontalSplitWindow)
                {
                    ans += horizontalS.GetEndPoint(0).X.ToString() + "," +
                        horizontalS.GetEndPoint(0).Y.ToString() + "," +
                        horizontalS.GetEndPoint(0).Z.ToString() + ",,," +
                        horizontalS.GetEndPoint(1).X.ToString() + "," +
                        horizontalS.GetEndPoint(1).Y.ToString() + "," +
                        horizontalS.GetEndPoint(1).Z.ToString() + "\n";
                }
                ans += "\n";
            }
            TaskDialog.Show("H", ans);  // Debug
            */

            List<Point> endPoints = new List<Point>();
            trans = new Transaction(document, "生成窗");
            trans.Start();
            foreach (List<Line> horizontalSplitWindow in horizontalSplitWindows)
            {
                double width = 0;
                foreach (Line horizontalS in horizontalSplitWindow)
                {
                    endPoints.Add(Point.Create(horizontalS.GetEndPoint(0)));
                    endPoints.Add(Point.Create(horizontalS.GetEndPoint(1)));
                    width = horizontalS.Length;
                }
                double maxY = endPoints[0].Coord.Y;
                foreach (Point point in endPoints)
                {
                    if (point.Coord.Y > maxY)
                    {
                        maxY = point.Coord.Y;
                    }
                }
                double minY = endPoints[0].Coord.Y;
                foreach (Point point in endPoints)
                {
                    if (point.Coord.Y < minY)
                    {
                        minY = point.Coord.Y;
                    }
                }
                Point center_temp = ElementPositionUtils.GetCenter(endPoints);
                XYZ center = new XYZ(center_temp.Coord.X,
                                     center_temp.Coord.Y,
                                     level.ProjectElevation + 3);
                Line axesline = Line.CreateBound(
                    new XYZ(
                        horizontalSplitWindow[0].GetEndPoint(0).X,
                        center.Y,
                        horizontalSplitWindow[0].GetEndPoint(0).Z),
                    new XYZ(
                        horizontalSplitWindow[0].GetEndPoint(1).X,
                        center.Y,
                        horizontalSplitWindow[0].GetEndPoint(1).Z)
                    );
                Wall wall = ElementPositionUtils.CreateWall(document, axesline, level, maxY - minY, 4000 / 304.8, 0);
                ElementPositionUtils.CreateWindow(document, center, width, 3, wall, level);
                endPoints.Clear();
            }
            foreach (List<Line> verticalSplitWindow in verticalSplitWindows)
            {
                double width = 0;
                foreach (Line verticalS in verticalSplitWindow)
                {
                    endPoints.Add(Point.Create(verticalS.GetEndPoint(0)));
                    endPoints.Add(Point.Create(verticalS.GetEndPoint(1)));
                    width = verticalS.Length;
                }
                double maxX = endPoints[0].Coord.X;
                foreach (Point point in endPoints)
                {
                    if (point.Coord.X > maxX)
                    {
                        maxX = point.Coord.X;
                    }
                }
                double minX = endPoints[0].Coord.X;
                foreach (Point point in endPoints)
                {
                    if (point.Coord.X < minX)
                    {
                        minX = point.Coord.X;
                    }
                }
                Point center_temp = ElementPositionUtils.GetCenter(endPoints);
                XYZ center = new XYZ(center_temp.Coord.X,
                                     center_temp.Coord.Y,
                                     level.ProjectElevation + 3);
                Line axesline = Line.CreateBound(
                    new XYZ(
                        center.X,
                        verticalSplitWindow[0].GetEndPoint(0).Y,
                        verticalSplitWindow[0].GetEndPoint(0).Z),
                    new XYZ(
                        center.X,
                        verticalSplitWindow[0].GetEndPoint(1).Y,
                        verticalSplitWindow[0].GetEndPoint(1).Z)
                    );
                Wall wall = ElementPositionUtils.CreateWall(document, axesline, level, maxX - minX, 4000 / 304.8, 0);
                ElementPositionUtils.CreateWindow(document, center, width, 3, wall, level);
                endPoints.Clear();
            }
            trans.Commit();

            trans = new Transaction(document, "生成轴网");
            trans.Start();
            ElementPositionUtils.CreateAxes(document, axesObjs);
            trans.Commit();

            trans = new Transaction(document, "生成门");
            trans.Start();
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