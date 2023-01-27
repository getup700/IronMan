using Autodesk.Revit.DB;
using IronMan.Revit.DTO;
using IronMan.Revit.Entity;
using IronMan.Revit.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IronMan.Revit.IServices
{
    public interface IFloorService:IDataUpdater<FloorProxy,DTO_Floor>
    {

    }
}
