using Autodesk.Revit.DB;
using IronMan.Revit.DTO;
using IronMan.Revit.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IronMan.Revit.IServices
{
    public interface IMepService : IDataUpdater<MEPCurve, DTO_Mep>
    {
    }
}
