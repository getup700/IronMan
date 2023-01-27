using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IronMan.Revit.Interfaces
{
    public interface IDynamicModelUpdater:IUpdater
    {
        void Raise(Action<UpdaterData> action);
    }
}
