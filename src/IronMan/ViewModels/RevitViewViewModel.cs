///************************************************************************************
///   Author:Tony Stark
///   CretaeTime:2023/3/6 23:50:26
///   Mail:2609639898@qq.com
///   Github:https://github.com/getup700
///
///   Description:
///
///************************************************************************************

using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using GalaSoft.MvvmLight;
using IronMan.Revit.Toolkit.Extension;
using IronMan.Revit.Toolkit.Mvvm.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Autodesk.Revit.UI.DockablePanes;

namespace IronMan.Revit.ViewModels
{
    internal class RevitViewViewModel : ViewModelBase
    {
        private readonly IDataContext _dataContext;

        public RevitViewViewModel(IDataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public void GetViewBrowser()
        {
            DockablePaneId dockablePaneId = BuiltInDockablePanes.ViewBrowser;
            DockablePane dockablePane = _dataContext.GetUIApplication().GetDockablePane(dockablePaneId);
            var browserOrganization = BrowserOrganization.GetCurrentBrowserOrganizationForViews(_dataContext.GetDocument());
            
        }


    }
}
