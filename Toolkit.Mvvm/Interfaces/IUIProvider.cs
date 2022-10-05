using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IronMan.Revit.Toolkit.Mvvm.Interfaces
{
    public interface IUIProvider
    {
        UIControlledApplication GetUIApplication();

        ControlledApplication GetApplication();

        /// <summary>
        /// 窗口句柄
        /// </summary>
        /// <returns></returns>
        IntPtr GetWindowHandle();

        AddInId GetAddInId();
    }
}
