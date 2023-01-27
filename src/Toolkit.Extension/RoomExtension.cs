using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IronMan.Revit.Toolkit.Extension
{
    public static class RoomExtension
    {
        public static List<Wall> GetReferenceWalls(this Room room)
        {
            var wallList=new List<Wall>();
            Document document = room.Document;
            var dic = new Dictionary<ElementId, int>();
            var options = new SpatialElementBoundaryOptions();
            options.SpatialElementBoundaryLocation = SpatialElementBoundaryLocation.Finish;
            options.StoreFreeBoundaryFaces = true;
            var boundaries = room.GetBoundarySegments(options);
            foreach (var item in boundaries)
            {
                foreach (var segment in item)
                {
                    var wall = document.GetElement(segment.ElementId) as Wall;
                    var curve = segment.GetCurve();
                    if(wall == null)continue;
                    if(!dic.Keys.Contains(wall.Id))
                    {
                        dic.Add(wall.Id, 0);
                        wallList.Add(wall);
                    }
                    else { dic[wall.Id]++; }
                }
            }
            return wallList;
        }
    }
}
