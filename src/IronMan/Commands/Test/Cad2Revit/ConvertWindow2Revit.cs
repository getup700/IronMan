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
    public class ConvertWindow2Revit : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document document = uidoc.Document; // 获取当前软件、文档

            Reference refer = uidoc.Selection.PickObject(
                ObjectType.PointOnElement,
                "请拾取CAD链接中的窗户");   // 拾取CAD链接

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

            List<GeometryObject> windowsObjs = new List<GeometryObject>();

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
                                windowsObjs.Add(Line.CreateBound(
                                    transform.OfPoint(endpoints[i - 1]),
                                    transform.OfPoint(endpoints[i])
                                    ));
                            }
                        }
                        else
                        {
                            Line line = insObj as Line;
                            windowsObjs.Add(
                                Line.CreateBound(
                                    transform.OfPoint(line.GetEndPoint(0)),
                                    transform.OfPoint(line.GetEndPoint(1))
                                )
                            );
                        }
                    }
                }
            }
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
            foreach(List<Line> horizontalSplitWindow in verticalSplitWindows)
            {
                ans += "n\n";
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
            Transaction trans = new Transaction(document, "生成窗");
            trans.Start();
            Level level = document.ActiveView.GenLevel;
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
                ElementPositionUtils.CreateWindow(document, center, width, 3600 / 304.8, wall, level);
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
                ElementPositionUtils.CreateWindow(document, center, width, 3600 / 304.8, wall, level);
                endPoints.Clear();
            }
            trans.Commit();
            return Result.Succeeded;
        }
    }


}
