using Autodesk.Revit.DB;
using IronMan.Revit.DTO;
using IronMan.Revit.Entity;
using IronMan.Revit.Interfaces;
using System.Collections.Generic;

namespace IronMan.Revit.IServices
{
    public interface IWallService : IDataUpdater<WallProxy,DTO_Wall>,IDataCreator<WallProxy,DTO_Wall>
    {
        List<XYZ> CreateLocationPoint(WallProxy wall, double distance);
    }
}