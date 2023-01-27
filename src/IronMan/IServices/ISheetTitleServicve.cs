using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IronMan.Revit.IServices
{
    public interface ISheetTitleServicve
    {
        void CreateSheetTitls(Viewport viewport);
        void DeleteSheetTitle();
        void UpdateSheetTitle(Viewport viewport);
    }
}
