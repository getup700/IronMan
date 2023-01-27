using Autodesk.Revit.DB;
using IronMan.Revit.Toolkit.Mvvm.Interfaces;
using IronMan.Revit.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using IronMan.Revit.Toolkit.Extension;
using IronMan.Revit.IServices;
using System.IO;
using System.Windows.Forms;
using Autodesk.Revit.UI.Selection;
using IronMan.Revit.Interfaces;
using Autodesk.Revit.DB.Electrical;
using IronMan.Revit.DTO;

namespace IronMan.Revit.Services
{
    public class WallService : IWallService, IDataUpdater<WallProxy,DTO_Wall>, IDataCreator<WallProxy,DTO_Wall>
    {
        private IDataContext _dataContext;

        public WallService(IDataContext dataContext)
        {
            _dataContext = dataContext;
        }

        /// <summary>
        /// 修改墙的底部约束和底部偏移
        /// </summary>
        /// <param name="wall"></param>
        public WallProxy Update(WallProxy wall, DTO_Wall dtoWall)
        {
            _dataContext.GetDocument().NewTransaction(() =>
                {
                    wall.BaseLevelId = dtoWall.BaseLevelId;
                    wall.BaseOffset = dtoWall.BaseOffset;
                },"Updata Wall");
            return wall;
        }

        public List<XYZ> CreateLocationPoint(WallProxy wall, double distance)
        {
            List<XYZ> points = wall.GetIntersectPoints();
            List<XYZ> newPoints = new List<XYZ>();
            if (points.Count < 2) return null;
            if (points[0].DistanceTo(points[points.Count - 1]) < 10 / 304.8) return null;
            for (int i = 0; i < points.Count - 1; i++)
            {
                XYZ p1 = Line.CreateBound(points[i], points[i + 1]).Evaluate(distance.ConvertToFeet(), false);
                XYZ p2 = Line.CreateBound(points[i + 1], points[i]).Evaluate(distance.ConvertToFeet(), false);
                newPoints.Add(p1);
                newPoints.Add(p2);
            }
            return newPoints;
        }

        public WallProxy CreateElement(DTO_Wall dto)
        {
            Wall newWall = null;
            //_dataContext.GetDocument().NewTransaction(() =>
            //{
            //    WallUtils.DisallowWallJoinAtEnd(dto._wall, 0);
            //    WallUtils.DisallowWallJoinAtEnd(dto._wall, 1);
            //    //Get levelId
            //    _dataContext.GetDocument().Regenerate();
            //    var planarFaces = dto._wall.GetSameDirectionPlanarFaces();
            //    var planarFace = planarFaces.FirstOrDefault();
            //    List<Curve> profile = planarFace.GetOutline(out List<CurveArray> openingArrays);
            //    newWall = Wall.Create(_dataContext.GetDocument(),
            //        profile, dto._wall.WallType.Id, dto.BaseLevelId, false, planarFace.FaceNormal);
            //    newWall.get_Parameter(BuiltInParameter.WALL_BASE_CONSTRAINT).Set(dto.BaseLevelId);
            //    newWall.get_Parameter(BuiltInParameter.WALL_BASE_OFFSET).Set(dto.BaseOffset);
            //    if (dto.TopLevelId != ElementId.InvalidElementId)
            //    {
            //        newWall.get_Parameter(BuiltInParameter.WALL_HEIGHT_TYPE).Set(dto.TopLevelId);
            //        newWall.get_Parameter(BuiltInParameter.WALL_TOP_OFFSET).Set(dto.TopOffset);
            //    }
            //    foreach (var item in openingArrays)
            //    {
            //        _dataContext.GetDocument().Create.NewOpening(
            //            newWall, item.get_Item(0).GetEndPoint(0), item.get_Item(1).GetEndPoint(1));
            //    }
            //    Location location = newWall.Location;
            //    XYZ translation = dto.Thickness / 2 * dto.Direction;
            //    location.Move(translation);

            //});
            return new WallProxy(newWall);

        }

        [Obsolete("CreateElements", false)]
        public IEnumerable<WallProxy> CreateElements(DTO_Wall dto)
        {
            return null;
            
        }
    }
}
