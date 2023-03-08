using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IronMan.Revit.Toolkit.Mvvm.Interfaces
{
    /// <summary>
    /// 注册UI
    /// </summary>
    public interface IApplicationUI
    {
        /// <summary>
        /// 注册基本的Ribbon
        /// </summary>
        /// <returns></returns>
        Result Initial();

        Result CreatePanel();

        void CreateTab();

    }
}
