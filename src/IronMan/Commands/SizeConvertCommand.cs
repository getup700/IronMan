using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using IronMan.Revit.Toolkit.Mvvm;
using IronMan.Revit.Toolkit.Mvvm.Extension;
using IronMan.Revit.Toolkit.Mvvm.IOC;
using IronMan.Revit.ViewModels;
using IronMan.Revit.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace IronMan.Revit.Commands
{
    [Transaction(TransactionMode.Manual)]
    [Journaling(JournalingMode.NoCommandData)]
    [Regeneration(RegenerationOption.Manual)]
    public class SizeConvertCommand : CommandBase
    {
        public override Window CreateMainWindow()
        {
            return SingletonIOC.Current.Container.Resolve<SizeConvertView, SizeConvertViewModel>(true);
        }

        public override Result Execute(ref string message, ElementSet elements)
        {
            MainWindow.Show();
            return Result.Succeeded;
        }
    }
}
