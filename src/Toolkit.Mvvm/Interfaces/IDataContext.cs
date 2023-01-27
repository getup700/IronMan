using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IronMan.Revit.Toolkit.Mvvm.Interfaces
{
    public interface IDataContext
    {
        //Document TransientDocument();

        Document GetDocument();

        UIDocument GetUIDocument();

        UIApplication GetUIApplication();
    }
}
