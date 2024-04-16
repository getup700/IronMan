using Autodesk.Revit.UI;
using IronMan.Revit.Toolkit.Mvvm.IOC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IronMan.Revit.DockablePanes
{
    internal class DockablePaneProvider : IDockablePaneProvider
    {
        public static DockablePaneId Id => new DockablePaneId(new Guid("01A97271-16E5-485E-8148-32B8FEF93AA3"));
        public void SetupDockablePane(DockablePaneProviderData data)
        {
            #region Default Mode
            //var dockablePanestate = new DockablePaneState()
            //{
            //    DockPosition = DockPosition.Right
            //};
            #endregion

            #region FLoating Mode
            //var dockablePaneState = new DockablePaneState()
            //{
            //    DockPosition = DockPosition.Floating
            //};
            //dockablePaneState.SetFloatingRectangle(new  Autodesk.Revit.DB.Rectangle(100, 100, 500, 500));
            #endregion

            #region Tabbed Mode 叠加项目浏览器
            var dockablePaneState = new DockablePaneState
            {
                DockPosition = DockPosition.Tabbed,
                
                //MinimumHeight = 1000,
                //MinimumWidth = 100
            };
            dockablePaneState.TabBehind = Autodesk.Revit.UI.DockablePanes.BuiltInDockablePanes.ProjectBrowser;
            #endregion

            //UI元素构造器
            data.FrameworkElementCreator = new FrameworkElementCreator();
            //初始化加载位置
            data.InitialState = dockablePaneState;
            //初始化加载时是否可见，默认为true
            data.VisibleByDefault = true;

            //UI元素
            //data.FrameworkElement = null;
            //关联帮助
            data.ContextualHelp = null;

        }
    }
}
