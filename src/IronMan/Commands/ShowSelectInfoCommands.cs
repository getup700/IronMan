///************************************************************************************
///   Author:Tony Stark
///   CretaeTime:2023/3/12 0:20:34
///   Mail:2609639898@qq.com
///   Github:https://github.com/getup700
///
///   Description:
///
///************************************************************************************

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
    internal class ShowSelectInfoCommands : Toolkit.Mvvm.CommandBase
    {
        public override Window CreateMainWindow()
        {
            return SingletonIOC.Current.Container.Resolve<PropertiesView, ProperitesViewModel>(true);
        }

        public override Result Execute(ref string message, ElementSet elements)
        {
            MainWindow.Show();
            return Result.Succeeded;
        }
    }
}
