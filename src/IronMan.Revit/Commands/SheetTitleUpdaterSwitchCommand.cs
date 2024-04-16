using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using IronMan.Revit.Toolkit.Mvvm;
using IronMan.Revit.Toolkit.Mvvm.IOC;
using IronMan.Revit.Updaters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace IronMan.Revit.Commands
{
    public class SheetTitleUpdaterSwitchCommand : CommandBase
    {
        public override Window CreateMainWindow()
        {
            return null;
        }

        public override Result Execute(ref string message, ElementSet elements)
        {
            IUpdater updater = SingletonIOC.Current.Container.GetInstance<ISheetTitleUpdater>();
            if (UpdaterRegistry.IsUpdaterRegistered(updater.GetUpdaterId()))
            {
                UpdaterRegistry.DisableUpdater(updater.GetUpdaterId());
                TaskDialog.Show("IronMan",$"{nameof(SheetTitleUpdater)}已停用");
            }
            else
            {
                UpdaterRegistry.EnableUpdater(updater.GetUpdaterId());
                TaskDialog.Show("IronMan", $"{nameof(SheetTitleUpdater)}已启用");
            }
            return Result.Succeeded;
        }
    }
}
