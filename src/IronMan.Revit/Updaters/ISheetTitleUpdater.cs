using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IronMan.Revit.Updaters
{
    public interface ISheetTitleUpdater : IUpdater
    {
        void Register();

        void Unregister();
    }
}
