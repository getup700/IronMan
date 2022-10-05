using Autodesk.Revit.DB;
using IronMan.Revit.Entity;
using IronMan.Revit.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IronMan.Revit.IServices
{
    public interface IMaterialService : IDataService<MaterialPlus>, IExcelTransfer<MaterialPlus>
    {
        public void Save(MaterialPlus materialPlus, string name, Color color, Color appearanceColor);
    }
}
