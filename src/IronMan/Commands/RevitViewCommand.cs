///************************************************************************************
///   Author:Tony Stark
///   CretaeTime:2023/3/6 23:40:29
///   Mail:2609639898@qq.com
///   Github:https://github.com/getup700
///
///   Description:
///
///************************************************************************************

using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using IronMan.Revit.Toolkit.Extension;
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
    internal class RevitViewCommand : Toolkit.Mvvm.CommandBase
    {
        public override Window CreateMainWindow()
        {
            return SingletonIOC.Current.Container.Resolve<RevitView, RevitViewViewModel>();
        }

        public override Result Execute(ref string message, ElementSet elements)
        {
            var status = DataContext.GetDocument().NewTransactionGroup("RevitView", () => MainWindow.ShowDialog().Value);
            return status == TransactionStatus.Committed ? Result.Succeeded : Result.Cancelled;
        }
    }
}
