using Autodesk.Revit.DB.Architecture;
using IronMan.Revit.Entity;
using IronMan.Revit.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IronMan.Revit.IServices
{
    public interface ICollectService:IExcelTransfer<RoomProxy>
    {
        double GetLength(RoomProxy room );
    }
}
