using Autodesk.Revit.UI;
using Toolkit.Extension;
using System;
using System.Collections.Generic;
using System.Text;
using IronMan.Revit.Toolkit.Mvvm.Interfaces;
using System.Windows.Forms.PropertyGridInternal;
using IronMan.Revit.Commands;

namespace IronMan.Revit
{
    public class AppUI : IApplicationUI
    {
        private readonly IUIProvider _uiProvider;

        public AppUI(IUIProvider uiProvider)
        {
            _uiProvider = uiProvider;
        }

        public Result Initial()
        {
            const string _tab = "IronMan";
            _uiProvider.GetUIApplication().CreateRibbonTab(_tab);

            RibbonPanel panel = _uiProvider.GetUIApplication().CreateRibbonPanel(_tab, "资源");

            panel.CreateButton<MaterialsCommand>((b) =>
            {
                b.Text = "材质管理";
                b.LargeImage = IronMan.Revit.Properties.Resources.Materials.ConvertToBitmapSource();
                b.ToolTip = "颜色分别为外观颜色";
            });
            panel.CreateButton<FloorTransformCommand>((b) =>
            {
                b.Text = "地板铺排";
                b.LargeImage = IronMan.Revit.Properties.Resources.Materials.ConvertToBitmapSource();
                b.ToolTip = "1111";
            });
            panel.CreateButton<FloorTypeManageCommand>((b) =>
            {
                b.Text = "楼板类型";
                b.LargeImage = IronMan.Revit.Properties.Resources.Materials.ConvertToBitmapSource();
                b.ToolTip = "1111";
            });
            return Result.Succeeded;
        }

        //public Result OnShutdown(UIControlledApplication application)
        //{
        //    return Result.Succeeded;
        //}

        //public Result OnStartup(UIControlledApplication application)
        //{
        //    application.CreateRibbonTab(_tab);
        //    RibbonPanel panel = application.CreateRibbonPanel(_tab, "资源");
        //    panel.CreateButton<Commands.MaterialsCommand>((b) =>
        //    {
        //        b.Text = "材质管理";
        //        b.LargeImage = Revit.Properties.Resources.Materials.ConvertToBitmapSource();
        //        b.ToolTip = "这是一个材质管理器";
        //    });
        //    double pi = 3.14159;
        //    pi.ConverToFeet();
        //    return Result.Succeeded;
        //}
    }
}
