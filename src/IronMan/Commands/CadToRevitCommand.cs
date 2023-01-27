using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Mechanical;
using Autodesk.Revit.UI;
using GalaSoft.MvvmLight.Messaging;
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
using IronMan.Revit.Toolkit.Extension;

namespace IronMan.Revit.Commands
{
    [Transaction(TransactionMode.Manual)]
    [Journaling(JournalingMode.NoCommandData)]
    [Regeneration(RegenerationOption.Manual)]
    public class CadToRevitCommand : Toolkit.Mvvm.CommandBase
    {
        public override Window CreateMainWindow()
        {
            return null;
        }

        public override Result Execute(ref string message, ElementSet elements)
        {
            TransactionStatus status = DataContext.GetDocument().NewTransactionGroup("CadToRevit", () =>
            {
                try
                {
                    SingletonIOC.Current.Container.GetInstanceWithoutCaching<CadToRevitViewModel>();
                }
                catch
                {
                    return false;
                }
                return true;
            });
            //DataContext.GetDocument().NewTransaction(() => SingletonIOC.Current.Container.GetInstanceWithoutCaching(typeof(CadToRevitViewModel)));
            //return status == TransactionStatus.Committed ? Result.Succeeded : Result.Cancelled;
            //SingletonIOC.Current.Container.GetInstanceWithoutCaching(typeof(CadToRevitViewModel));
            //return Result.Succeeded;
            return status == TransactionStatus.Committed ? Result.Succeeded : Result.Cancelled;
        }

    }
}
