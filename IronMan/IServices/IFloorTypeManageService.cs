using Autodesk.Revit.DB;
using IronMan.Revit.Entity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IronMan.Revit.IServices
{
    public interface IFloorTypeManageService
    {
        IEnumerable<FloorTypeProxy> GetFloorTypes();
        ObservableCollection<FloorType> GetFloorType();
        void MoveFloors(FloorType floorType, int offset);
    }
}
