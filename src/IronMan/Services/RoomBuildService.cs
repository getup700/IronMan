using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;
using IronMan.Revit.Entity;
using IronMan.Revit.Toolkit.Mvvm.Interfaces;
using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace IronMan.Revit.Services
{
    public class RoomBuildService
    {
        private IDataContext _dataContext;

        public RoomBuildService(IDataContext dataContext)
        {
            _dataContext = dataContext;
        }

        private RoomType _roomType;
        private Major _major;
        private RoomProxy _roomProxy;

        private Document Document => _dataContext.GetDocument();

        public void BuildRoom(Major major, RoomType roomType)
        {
            _roomType = roomType;
            _major = major;
        }

        /// <summary>
        /// Factory design Pattern
        /// </summary>
        /// <param name="roomProxy"></param>
        public void Build(RoomProxy roomProxy)
        {
            _roomProxy = roomProxy;
            switch (_roomType)
            {
                case RoomType.ShowerRoom:
                    BuildShowerRoom();
                    break;

            }
        }

        private void BuildShowerRoom()
        {
            switch (_major)
            {
                case Major.All:
                    Build_Architecture();
                    Build_Structural();
                    Build_HVAC();
                    Build_WaterSupplyAndDrainage();
                    Build_Electrical();
                    break;
                case Major.Architecture:
                    Build_Architecture();
                    break;
                case Major.Strucutre:
                    Build_Structural();
                    break;
                case Major.Electrical:
                    Build_Electrical();
                    break;
                case Major.HVAC:
                    Build_HVAC();
                    break;
                case Major.WaterSupplyAndDrainage:
                    Build_WaterSupplyAndDrainage();
                    break;
            }
        }


        private void Build_Electrical()
        {
            throw new NotImplementedException();
        }

        private void Build_WaterSupplyAndDrainage()
        {
            throw new NotImplementedException();
        }

        private void Build_HVAC()
        {
            throw new NotImplementedException();
        }

        private void Build_Structural()
        {
            throw new NotImplementedException();
        }

        private void Build_Architecture()
        {
            double height = 1400;
            List<Wall> thickWalls = new List<Wall>();
            List<Wall> thinWalls = new List<Wall>();
            Level level = _roomProxy.Level;
            var locationCurve = _roomProxy.Location as LocationCurve;
            CurveLoop curveLoop = new CurveLoop();
            List<XYZ> results = new List<XYZ>();
            SpatialElementBoundaryOptions opts = new SpatialElementBoundaryOptions();
            IList<IList<BoundarySegment>> blist = _roomProxy._room.GetBoundarySegments(opts);
            foreach (var item in blist)
            {
                foreach (var bs in item)
                {
                    Curve curve = bs.GetCurve();
                    curveLoop.Append(curve);
                    results.Add((curve as Line).Direction.CrossProduct(_roomProxy.Direction));
                }
            }
            results.Sort();
            string message = string.Empty;
            foreach (var item in results)
            {
                message += item.ToString();
            }
            MessageBox.Show($"{message}");
        }


    }
}
