using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using GalaSoft.MvvmLight.Ioc;
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
using Toolkit.Extension;
using UIFramework;

namespace IronMan.Revit.Commands
{
    [Transaction(TransactionMode.Manual)]
    [Journaling(JournalingMode.NoCommandData)]
    [Regeneration(RegenerationOption.Manual)]
    public class FloorTransformCommand:CommandBase
    {
        public override Window CreateMainWindow()
        {
            return SingletonIOC.Current.Container.Resolve<FloorTransformView, FloorTransformViewModel>(false);
        }

        public override Result Execute(ref string message, ElementSet elements)
        {
            TransactionStatus status = DataContext.GetDocument().NewTransactionGroup( "地板铺装", () => MainWindow.ShowDialog().Value);
            return status == TransactionStatus.Committed ? Result.Succeeded : Result.Cancelled;
            ////DataContext.GetDocument().NewTransaction(() => MainWindow.Show(), "地板铺装");
            ////return Result.Succeeded;
            //DataContext.GetDocument().NewTransaction(() => MainWindow.ShowDialog());
            //return Result.Succeeded;
        }

        public override void RegisterTypes(SimpleIoc container)
        {
            base.RegisterTypes(container);
        }
    }
}
