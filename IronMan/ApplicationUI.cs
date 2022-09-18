using Autodesk.Revit.UI;
using Toolkit.Extension;
using System;
using System.Collections.Generic;
using System.Text;

namespace IronMan
{
    public class ApplicationUI : IExternalApplication
    {
        private const string _tab = "IronMan";

        public Result OnShutdown(UIControlledApplication application)
        {
            return Result.Succeeded;
        }

        public Result OnStartup(UIControlledApplication application)
        {
            application.CreateRibbonTab(_tab);
            RibbonPanel panel = application.CreateRibbonPanel(_tab, "资源");
            panel.CreateButton<Commands.MaterialsCommand>((b) =>
            {
                b.Text = "材质管理";
                b.LargeImage = Revit.Properties.Resources.Materials.ConvertToBitmapSource();
                b.ToolTip = "这是一个材质管理器";
            });
            double pi = 3.14159;
            pi.ConverToFeet();
            return Result.Succeeded;
        }
    }
}
