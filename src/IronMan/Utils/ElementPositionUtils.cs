using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IronMan.Revit.Geometry;

namespace IronMan.Revit.Utils
{
    public class ElementPositionUtils
    {
        public static bool IsEqual(double x, double y)
        {
            return Math.Abs(x - y) < 1e-3;
        }

        public static bool IsSameX(Line l1, Line l2)
        {
            return ((Math.Abs(l1.GetEndPoint(0).X - l2.GetEndPoint(0).X) < 1e-3 &&
                     Math.Abs(l1.GetEndPoint(1).X - l2.GetEndPoint(1).X) < 1e-3) ||
                    (Math.Abs(l1.GetEndPoint(0).X - l2.GetEndPoint(1).X) < 1e-3 &&
                     Math.Abs(l1.GetEndPoint(1).X - l2.GetEndPoint(0).X) < 1e-3));
        }

        public static bool IsSameY(Line l1, Line l2)
        {
            return ((Math.Abs(l1.GetEndPoint(0).Y - l2.GetEndPoint(0).Y) < 1e-3 &&
                     Math.Abs(l1.GetEndPoint(1).Y - l2.GetEndPoint(1).Y) < 1e-3) ||
                    (Math.Abs(l1.GetEndPoint(0).Y - l2.GetEndPoint(1).Y) < 1e-3 &&
                     Math.Abs(l1.GetEndPoint(1).Y - l2.GetEndPoint(0).Y) < 1e-3));
        }

        public static bool IsSameXYZ(XYZ p1, XYZ p2)
        {
            return (IsEqual(p1.X, p2.X) && IsEqual(p1.Y, p2.Y) && IsEqual(p1.Z, p2.Z));
        }

        public static void PrintPointList(List<Point> points)
        {
            string ans = "";
            foreach (Point point in points)
            {
                ans += point.Coord.X.ToString() + " " +
                                     point.Coord.Y.ToString() + " " +
                                     point.Coord.Z.ToString() + "\n";
            }
            TaskDialog.Show("Point", ans);
        }

        public static void PrintLineList(List<Line> lines)
        {
            string ans = "";
            foreach (Line line in lines)
            {
                ans += line.GetEndPoint(0).X.ToString() + " " +
                       line.GetEndPoint(0).Y.ToString() + " " +
                       line.GetEndPoint(0).Z.ToString() + ", " +
                       line.GetEndPoint(1).X.ToString() + " " +
                       line.GetEndPoint(1).Y.ToString() + " " +
                       line.GetEndPoint(1).Z.ToString() + "\n";
            }
            TaskDialog.Show("Line", ans);
        }

        public static Point GetCenter(List<Point> pointlist)
        {
            double X_sum = 0, Y_sum = 0, Z_sum = 0;
            for (int i = 0; i < pointlist.Count; i++)
            {
                X_sum += pointlist[i].Coord.X;
                Y_sum += pointlist[i].Coord.Y;
                Z_sum += pointlist[i].Coord.Z;
            }

            double center_X = X_sum / pointlist.Count;
            double center_Y = Y_sum / pointlist.Count;
            double center_Z = Z_sum / pointlist.Count;
            Point center = Point.Create(new XYZ(center_X, center_Y, center_Z));
            return center;
        }

        public static double Distance(Line l1, Line l2)
        {
            return l1.Distance(l2.GetEndPoint(0));
        }

        public static Wall CreateWall(Document document, Line axisLine, Level level, double width, double height, double offset)
        {
            FilteredElementCollector filter = new FilteredElementCollector(document);
            filter.OfClass(typeof(WallType));
            string attr = "常规 - " + Math.Round(width * 304.8, 3).ToString() + "mm";
            List<WallType> familySymbols = new List<WallType>();
            foreach (WallType familySymbol in filter)
            {
                if (familySymbol.GetParameters("族名称")[0].AsString() == "基本墙")
                {
                    familySymbols.Add(familySymbol);
                }
            }

            int i;
            bool bo = false;
            int j = 0;
            for (i = 0; i < familySymbols.Count; i++)
            {
                if (attr == familySymbols[i].Name)
                {
                    bo = true;
                    j = i;
                }
            }
            if (bo == true)
            {
                return Wall.Create(document, axisLine, familySymbols[j].Id, level.Id, height, offset, false, false);
            }
            else
            {
                for (i = 0; i < familySymbols.Count; i++)
                {
                    if ("常规 - 200mm" == familySymbols[i].Name) { break; }
                }
                WallType fam = familySymbols[i];
                WallType coluType = fam.Duplicate(attr) as WallType;
                CompoundStructure cs = coluType.GetCompoundStructure();
                int layerIndex = cs.GetFirstCoreLayerIndex();
                cs.SetLayerWidth(layerIndex, width);
                coluType.SetCompoundStructure(cs);
                return Wall.Create(document, axisLine, coluType.Id, level.Id, height, offset, false, false);
            }
        }

        public static void CreateColumn(Document document, XYZ center, Level level, double b, double h)
        {
            FilteredElementCollector filter = new FilteredElementCollector(document);
            filter.OfClass(typeof(FamilySymbol));
            string bh = Math.Round(b * 304.8, 3).ToString() + " " + "x" + " " + Math.Round(h * 304.8, 3).ToString() + "mm";
            List<FamilySymbol> familySymbols = new List<FamilySymbol>();
            foreach (FamilySymbol familySymbol in filter)
            {
                if (familySymbol.GetParameters("族名称")[0].AsString() == "混凝土 - 矩形 - 柱")
                {
                    familySymbols.Add(familySymbol);
                }
            }

            int i;
            bool bo = false;
            int j = 0;
            for (i = 0; i < familySymbols.Count; i++)
            {
                if (bh == familySymbols[i].Name)
                {
                    bo = true;
                    j = i;
                }
            }
            if (bo == true)
            {
                document.Create.NewFamilyInstance(center, familySymbols[j], level, StructuralType.Column);
            }
            else
            {
                FamilySymbol fam = familySymbols[0];
                ElementType coluType = fam.Duplicate(bh);
                coluType.GetParameters("b")[0].Set(b);
                coluType.GetParameters("h")[0].Set(h);
                FamilySymbol fs = coluType as FamilySymbol;
                document.Create.NewFamilyInstance(center, fs, level, StructuralType.Column);
            }
        }

        public static void CreateAxes(Document document, List<GeometryObject> axesObjs)
        {
            List<Line> horizontalAxis = new List<Line>();
            List<Line> verticalAxis = new List<Line>();

            foreach (GeometryObject axesObj in axesObjs)
            {
                Line axis = axesObj as Line;
                if (IsEqual(axis.GetEndPoint(0).X, axis.GetEndPoint(1).X))
                {
                    verticalAxis.Add(axis);
                }
                else if (IsEqual(axis.GetEndPoint(0).Y, axis.GetEndPoint(1).Y))
                {
                    horizontalAxis.Add(axis);
                }
            }
            for (int i = 0; i < horizontalAxis.Count - 1; i++)
            {
                for (int j = 0; j < horizontalAxis.Count - 1 - i; j++)
                {
                    if (horizontalAxis[j].GetEndPoint(0).Y >
                        horizontalAxis[j + 1].GetEndPoint(1).Y)
                    {
                        var temp = horizontalAxis[j + 1];
                        horizontalAxis[j + 1] = horizontalAxis[j];
                        horizontalAxis[j] = temp;
                    }
                }
            }
            for (int i = 0; i < verticalAxis.Count - 1; i++)
            {
                for (int j = 0; j < verticalAxis.Count - 1 - i; j++)
                {
                    if (verticalAxis[j].GetEndPoint(0).X >
                        verticalAxis[j + 1].GetEndPoint(1).X)
                    {
                        var temp = verticalAxis[j + 1];
                        verticalAxis[j + 1] = verticalAxis[j];
                        verticalAxis[j] = temp;
                    }
                }
            }
            if (verticalAxis.Count > 0)
            {
                Grid vertical = Grid.Create(document, verticalAxis[0]);
                vertical.Name = "1";
                for (int i = 1; i < verticalAxis.Count; i++)
                {
                    Grid.Create(document, verticalAxis[i]);
                }
            }
            if (horizontalAxis.Count > 0)
            {
                Grid horizontal = Grid.Create(document, horizontalAxis[0]);
                horizontal.Name = "A";
                for (int i = 1; i < horizontalAxis.Count; i++)
                {
                    Grid.Create(document, horizontalAxis[i]);
                }
            }
        }

        public static void CreateWindow(Document document, XYZ center, double width, double height, Wall desWall, Level level)
        {
            FilteredElementCollector filter = new FilteredElementCollector(document);
            filter.OfClass(typeof(FamilySymbol));
            string bh = Math.Round(width * 304.8, 3).ToString() + " " + "x" + " " +
                        Math.Round(height * 304.8, 3).ToString() + "mm";
            List<FamilySymbol> familySymbols = new List<FamilySymbol>();
            foreach (FamilySymbol familySymbol in filter)
            {
                if (familySymbol.GetParameters("族名称")[0].AsString() == "固定")
                {
                    familySymbols.Add(familySymbol);
                }
            }

            int i;
            bool bo = false;
            int j = 0;
            for (i = 0; i < familySymbols.Count; i++)
            {
                if (bh == familySymbols[i].Name)
                {
                    bo = true;
                    j = i;
                }
            }
            if (bo == true)
            {
                document.Create.NewFamilyInstance(center, familySymbols[j], desWall, level, StructuralType.NonStructural);
            }
            else
            {
                FamilySymbol fam = familySymbols[0];
                ElementType coluType = fam.Duplicate(bh);
                coluType.GetParameters("宽度")[0].Set(width);
                coluType.GetParameters("高度")[0].Set(height);
                FamilySymbol fs = coluType as FamilySymbol;
                document.Create.NewFamilyInstance(center, fs, desWall, level, StructuralType.NonStructural);
            }
        }

        public static void CreateDoor(Document document, XYZ center, double width, double height, Wall desWall, Level level)
        {
            FilteredElementCollector filter = new FilteredElementCollector(document);
            filter.OfClass(typeof(FamilySymbol));
            string bh = Math.Round(width * 304.8, 3).ToString() + " " + "x" + " " +
                        Math.Round(height * 304.8, 3).ToString() + "mm";
            List<FamilySymbol> familySymbols = new List<FamilySymbol>();
            foreach (FamilySymbol familySymbol in filter)
            {
                if (familySymbol.GetParameters("族名称")[0].AsString() == "单扇 - 与墙齐")
                {
                    familySymbols.Add(familySymbol);
                }
            }

            int i;
            bool bo = false;
            int j = 0;
            for (i = 0; i < familySymbols.Count; i++)
            {
                if (bh == familySymbols[i].Name)
                {
                    bo = true;
                    j = i;
                }
            }
            if (bo == true)
            {
                document.Create.NewFamilyInstance(center, familySymbols[j], desWall, level, StructuralType.NonStructural);
            }
            else
            {
                FamilySymbol fam = familySymbols[0];
                ElementType coluType = fam.Duplicate(bh);
                coluType.GetParameters("宽度")[0].Set(width);
                coluType.GetParameters("高度")[0].Set(height);
                FamilySymbol fs = coluType as FamilySymbol;
                document.Create.NewFamilyInstance(center, fs, desWall, level, StructuralType.NonStructural);
            }
        }

        public static double P2P_distance(Point p1, Point p2)
        {
            return Math.Sqrt(Math.Pow(p1.Coord.X - p2.Coord.X, 2) + Math.Pow(p1.Coord.Y - p2.Coord.Y, 2));
        }

        public static double L2L_distance(Line l1, Line l2)
        {
            if (IsEqual(l1.Direction.X, 0))
            {
                return Math.Abs(l1.GetEndPoint(0).X - l2.GetEndPoint(0).X);
            }
            else
            {
                return Math.Abs(l1.GetEndPoint(0).Y - l2.GetEndPoint(0).Y);
            }
        }

        public static void Get_intersections(List<Point> Points, ref List<Point> intersections)    //由所有墙轮廓线的端点，获取墙轴线的端点。
        {
            foreach (Point p in Points)
            {
                List<Point> nearbypoints = new List<Point>();
                foreach (Point q in Points)
                {
                    if (P2P_distance(p, q) <= 500 / 304.8)
                    {
                        nearbypoints.Add(q);        //对于每个点p，nearbypoints含有包括了p自身在内所有与p距离在墙厚以内的点。
                    }
                }
                intersections.Add(GetCenter(nearbypoints));                    //得到这些点的中心，可作为墙轴线的端点，也即intersections。
            }
            for (int i = 0; i < intersections.Count; i++)
            {
                for (int j = intersections.Count - 1; j > i; j--)
                {
                    if (Math.Abs(intersections[i].Coord.X - intersections[j].Coord.X) < 0.000328
                        && Math.Abs(intersections[i].Coord.Y - intersections[j].Coord.Y) < 0.000328
                        && Math.Abs(intersections[i].Coord.Z - intersections[j].Coord.Z) < 0.000328)
                    {
                        intersections.RemoveAt(j);
                    }
                }
            }
        }

        public static void CreateWallAxes(List<Point> intersections, ref List<Line> Lines)     //输入墙轴线端点的list，两两连接得到可能的实际墙轴线。
        {
            for (int i = intersections.Count - 1; i > 0; i--)
            {
                Point point_temp = intersections[i];
                intersections.RemoveAt(i);
                foreach (Point intersection_2 in intersections)
                {
                    Lines.Add(Line.CreateBound(point_temp.Coord, intersection_2.Coord));
                }
            }
        }

        public static bool Is_same_direction(XYZ direction1, XYZ direction2)
        {
            return ((IsEqual(direction1.X, direction2.X) && IsEqual(direction1.Y, direction2.Y) && IsEqual(direction1.Z, direction2.Z)) ||
                   (IsEqual(direction1.X, -direction2.X) && IsEqual(direction1.Y, -direction2.Y) && IsEqual(direction1.Z, -direction2.Z)));
        }

        public static bool Is_parallel(Line l1, Line l2)       //判断两直线是否平行。
        {
            if (Is_same_direction(l1.Direction, l2.Direction))
            {
                return true;
            }
            else return false;
        }

        public static bool Isnot_far(Line l1, Line l2)
        {
            double d = L2L_distance(l1, l2);
            if (d < 500 / 304.8 && (!IsEqual(d, 0)))
            {
                return true;
            }
            else return false;
        }

        public static bool Isnot_staggered(Line l1, Line l2)       //判断两边线是否错开。
        {
            Line shortline, longline;
            if (l1.Length < l2.Length || IsEqual(l1.Length, l2.Length))
            {
                shortline = l1;
                longline = l2;
            }
            else
            {
                shortline = l2;
                longline = l1;
            }

            double mincoord, maxcoord;
            if (IsEqual(shortline.Direction.X, 0))
            {
                maxcoord = Math.Max(longline.GetEndPoint(0).Y, longline.GetEndPoint(1).Y);
                mincoord = Math.Min(longline.GetEndPoint(0).Y, longline.GetEndPoint(1).Y);
                if ((shortline.GetEndPoint(0).Y > mincoord - 1e-3 && shortline.GetEndPoint(0).Y < maxcoord + 1e-3) ||
                    (shortline.GetEndPoint(1).Y > mincoord - 1e-3 && shortline.GetEndPoint(1).Y < maxcoord + 1e-3))
                {
                    return true;
                }
                else return false;
            }
            else
            {
                maxcoord = Math.Max(longline.GetEndPoint(0).X, longline.GetEndPoint(1).X);
                mincoord = Math.Min(longline.GetEndPoint(0).X, longline.GetEndPoint(1).X);
                if ((shortline.GetEndPoint(0).X > mincoord - 1e-3 && shortline.GetEndPoint(0).X < maxcoord + 1e-3) ||
                    (shortline.GetEndPoint(1).X > mincoord - 1e-3 && shortline.GetEndPoint(1).X < maxcoord + 1e-3))
                {
                    return true;
                }
                else return false;
            }
        }

        public static void Get_connect_points(WS ws, ref List<Point> connectpoints)       //获取墙段的邻接节点。
        {
            Line BL1 = ws.BL1, BL2 = ws.BL2;

            if (IsEqual(BL1.Direction.X, 0))
            {
                double mincoord, maxcoord;
                maxcoord = Math.Max(BL2.GetEndPoint(0).Y, BL2.GetEndPoint(1).Y);
                mincoord = Math.Min(BL2.GetEndPoint(0).Y, BL2.GetEndPoint(1).Y);
                if (BL1.GetEndPoint(0).Y > mincoord - 1e-3 && BL1.GetEndPoint(0).Y < maxcoord + 1e-3)
                {
                    connectpoints.Add(Point.Create(BL1.GetEndPoint(0)));
                }
                if (BL1.GetEndPoint(1).Y > mincoord - 1e-3 && BL1.GetEndPoint(1).Y < maxcoord + 1e-3)
                {
                    connectpoints.Add(Point.Create(BL1.GetEndPoint(1)));
                }
            }

            if (IsEqual(BL2.Direction.X, 0))
            {
                double mincoord, maxcoord;
                maxcoord = Math.Max(BL1.GetEndPoint(0).Y, BL1.GetEndPoint(1).Y);
                mincoord = Math.Min(BL1.GetEndPoint(0).Y, BL1.GetEndPoint(1).Y);
                if (BL2.GetEndPoint(0).Y > mincoord - 1e-3 && BL2.GetEndPoint(0).Y < maxcoord + 1e-3)
                {
                    connectpoints.Add(Point.Create(BL2.GetEndPoint(0)));
                }
                if (BL2.GetEndPoint(1).Y > mincoord - 1e-3 && BL2.GetEndPoint(1).Y < maxcoord + 1e-3)
                {
                    connectpoints.Add(Point.Create(BL2.GetEndPoint(1)));
                }
            }

            if (IsEqual(BL1.Direction.Y, 0))
            {
                double mincoord, maxcoord;
                maxcoord = Math.Max(BL1.GetEndPoint(0).X, BL1.GetEndPoint(1).X);
                mincoord = Math.Min(BL1.GetEndPoint(0).X, BL1.GetEndPoint(1).X);
                if (BL2.GetEndPoint(0).X > mincoord - 1e-3 && BL2.GetEndPoint(0).X < maxcoord + 1e-3)
                {
                    connectpoints.Add(Point.Create(BL2.GetEndPoint(0)));
                }
                if (BL2.GetEndPoint(1).X > mincoord - 1e-3 && BL2.GetEndPoint(1).X < maxcoord + 1e-3)
                {
                    connectpoints.Add(Point.Create(BL2.GetEndPoint(1)));
                }
            }

            if (IsEqual(BL2.Direction.Y, 0))
            {
                double mincoord, maxcoord;
                maxcoord = Math.Max(BL2.GetEndPoint(0).X, BL2.GetEndPoint(1).X);
                mincoord = Math.Min(BL2.GetEndPoint(0).X, BL2.GetEndPoint(1).X);
                if (BL1.GetEndPoint(0).X > mincoord - 1e-3 && BL1.GetEndPoint(0).X < maxcoord + 1e-3)
                {
                    connectpoints.Add(Point.Create(BL1.GetEndPoint(0)));
                }
                if (BL1.GetEndPoint(1).X > mincoord - 1e-3 && BL1.GetEndPoint(1).X < maxcoord + 1e-3)
                {
                    connectpoints.Add(Point.Create(BL1.GetEndPoint(1)));
                }
            }
        }

        public static bool Is_neighbor(WS ws1, WS ws2)
        {
            List<Point> pointlist1 = new List<Point>();
            List<Point> pointlist2 = new List<Point>();
            Get_connect_points(ws1, ref pointlist1);
            Get_connect_points(ws2, ref pointlist2);
            int i = 0;
            foreach (Point p1 in pointlist1)
            {
                foreach (Point p2 in pointlist2)
                {
                    if (IsSameXYZ(p1.Coord, p2.Coord))
                    {
                        i++;
                    }
                }
            }
            if (i > 0) return true;
            else return false;
        }

        public static void Cal_Wallaxis(WS ws, ref double X, ref double Y)
        {
            Line l1 = ws.BL1;
            Line l2 = ws.BL2;
            if (IsEqual(l1.Direction.X, 0))
            {
                X = (l1.GetEndPoint(0).X + l2.GetEndPoint(0).X) / 2;
            }
            else
            {
                Y = (l1.GetEndPoint(0).Y + l2.GetEndPoint(0).Y) / 2;
            }
        }

        public static int NotExistsIn(Point p, List<Point> plist)
        {
            int i;
            for (i = 0; i < plist.Count; i++)
            {
                if (IsSameXYZ(p.Coord, plist[i].Coord)) break;
            }
            return i;

        }

        public static void SortByCoord(ref List<Point> pointlist)
        {
            for (int i = 0; i < pointlist.Count - 1; i++)
            {
                for (int j = 0; j < pointlist.Count - 1 - i; j++)
                {
                    if ((pointlist[j].Coord.X + pointlist[j].Coord.Y) >
                        (pointlist[j + 1].Coord.X + pointlist[j + 1].Coord.Y))
                    {
                        var temp = pointlist[j + 1];
                        pointlist[j + 1] = pointlist[j];
                        pointlist[j] = temp;
                    }
                }
            }
        }

        public static bool Exists_NearbyLine(Line L, List<Line> lines)     //判断轴线附近是否存在墙线，返回一个Boolean值。 
        {
            int i = 0;
            foreach (Line L_wall in lines)
            {
                if (L2L_distance(L, L_wall) < 300 / 308.4 && Is_parallel(L, L_wall) && Isnot_staggered(L, L_wall))     //判断依据：存在至少一条线段，使得其与轴线的距离小于最大墙宽的一半。
                {
                    i++;
                }
            }
            if (i == 0)
            {
                return false;
            }
            else return true;
        }

        public static void Gen_WSlist(List<Line> Walloutlines, ref List<WS> WSlist)
        {
            foreach (Line line in Walloutlines)
            {
                foreach (Line line1 in Walloutlines)
                {
                    if (Is_parallel(line, line1) &&
                        Isnot_staggered(line, line1) &&
                        Isnot_far(line, line1))
                    {
                        WSlist.Add(new WS(line, line1));
                    }
                }
            }
        }

        public static bool IsSameLine(Line l1, Line l2)
        {
            XYZ l1_endpoint1 = l1.GetEndPoint(0);
            XYZ l1_endpoint2 = l1.GetEndPoint(1);
            XYZ l2_endpoint1 = l2.GetEndPoint(0);
            XYZ l2_endpoint2 = l2.GetEndPoint(1);

            return ((IsSameXYZ(l1_endpoint1, l2_endpoint1) && IsSameXYZ(l1_endpoint2, l2_endpoint2)) ||
                    (IsSameXYZ(l1_endpoint1, l2_endpoint2) && IsSameXYZ(l1_endpoint2, l2_endpoint1)));
        }

        public static bool IsSameWS(WS ws1, WS ws2)
        {
            return (IsSameLine(ws1.BL1, ws2.BL1) && IsSameLine(ws1.BL2, ws2.BL2)) ||
                   (IsSameLine(ws1.BL1, ws2.BL2) && IsSameLine(ws1.BL2, ws2.BL1));
        }
    }


}
