using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IronMan.Revit.IServices
{
    public interface IExternalEventService : IExternalEventHandler
    {
        void Raise(Action<UIApplication> action);

    }
}
