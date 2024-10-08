﻿using Autodesk.Revit.Attributes;
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
using IronMan.Revit.Toolkit.Extension;
using Autodesk.Revit.DB.Mechanical;

namespace IronMan.Revit.Commands.Test
{
    [Transaction(TransactionMode.Manual)]
    [Journaling(JournalingMode.NoCommandData)]
    [Regeneration(RegenerationOption.Manual)]
    public class ModeCommand : Toolkit.Mvvm.CommandBase
    {
        public override Window CreateMainWindow()
        {
            return SingletonIOC.Current.Container.Resolve<ModeView, ModeViewModel>(true);
        }

        public override Result Execute(ref string message, ElementSet elements)
        {
            //TransactionStatus status = DataContext.GetDocument().NewTransactionGroup("模态窗口", () =>
            //{
            //    MainWindow.Show();
            //    return true;
            //});
            //return status == TransactionStatus.Committed ? Result.Succeeded : Result.Cancelled;
            MainWindow.Show();
            return Result.Succeeded;
        }
    }
}
