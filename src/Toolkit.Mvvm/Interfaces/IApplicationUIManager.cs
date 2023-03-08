using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IronMan.Revit.Toolkit.Mvvm.Interfaces
{
    public interface IApplicationUIManager
    {
        void AddPanel();

        List<RibbonPanel> GetPanels();

    }
}
