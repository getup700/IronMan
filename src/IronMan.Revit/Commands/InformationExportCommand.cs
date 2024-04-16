using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using IronMan.Revit.Toolkit.Extension;
using IronMan.Revit.Toolkit.Mvvm;
using IronMan.Revit.Toolkit.Mvvm.IOC;
using IronMan.Revit.ViewModels;
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
    public class InformationExportCommand : CommandBase
    {
        public override Window CreateMainWindow()
        {
            return null;
        }

        public override Result Execute(ref string message, ElementSet elements)
        {
            TransactionStatus status = DataContext.GetDocument().NewTransactionGroup("Export", () =>
            {
                try
                {
                    SingletonIOC.Current.Container.GetInstanceWithoutCaching<InformationExportViewModel>();
                }
                catch
                {
                    return false;
                }
                return true;
            });
            return status == TransactionStatus.Committed ? Result.Succeeded : Result.Cancelled;
        }
    }
}
