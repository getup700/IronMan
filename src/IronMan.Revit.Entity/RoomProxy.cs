using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;
using Autodesk.Revit.DB.Plumbing;
using IronMan.Revit.Toolkit.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IronMan.Revit.Entity
{
    public class RoomProxy : ElementProxy
    {
        public Room _room;
        private Document _document => _room.Document;

        public RoomProxy(Element element) : base(element)
        {
            _room = element as Room;
        }

        public RoomProxy(Element element, XYZ direction)
        {
            _room = element as Room;
            Direction = direction;
            ReferenceWallList = _room.GetReferenceWalls().ConvertAll(x => new WallProxy(x));
            this.Height = _room.get_Parameter(BuiltInParameter.ROOM_UPPER_OFFSET).AsDouble().ConvertToMilliMeters();
            this.LevelId = _room.LevelId.IntegerValue;
        }


        #region Properties
        public Level Level => _room.Level;

        public int LevelId { get; set; }

        public string TypeName { get; set; }

        public double BaseOffset { get; set; }

        public bool IsFilp { get; set; }

        public bool IsStructural { get; set; }

        public XYZ Direction { get; set; }

        public XYZ Central { get; set; }

        public RoomType RoomType { get; set; }

        public double Width { get; set; }

        public double Length { get; set; }

        public double Height { get; set; }

        public Double Areas => _room.Area.ConverToSquareMeters();

        public Location Location => _room.Location;

        public List<BoundarySegment> BoundarySements => _room.GetBoundarySegments(new SpatialElementBoundaryOptions())?.FirstOrDefault()?.ToList();

        /// <summary>
        /// 房间内要创建的墙
        /// </summary>
        public List<WallProxy> WallList { get; set; } = new List<WallProxy>();

        /// <summary>
        /// 房间要参照的墙
        /// </summary>
        public List<WallProxy> ReferenceWallList { get; set; }

        public List<DoorProxy> DoorList { get; set; } = new List<DoorProxy>();

        /// <summary>
        /// 房间编号
        /// </summary>
        public string Code { get; set; }

        #endregion

        #region Methods

        #endregion


    }
    public enum RoomType
    {
        ShowerRoom,
        WashBasin,
        Urinal,
        RelasRoom
    }

    public enum Major
    {
        All,
        Architecture,
        Strucutre,
        HVAC,
        WaterSupplyAndDrainage,
        Electrical
    }


}
