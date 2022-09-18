using IronMan.Revit.Entity;
using IronMan.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IronMan.IServices
{
    public interface IMaterialService:IDataService<MaterialPlus>,IExcelTransfer<MaterialPlus>
    {

    }
}
