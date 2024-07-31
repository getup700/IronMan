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
using IronMan.Revit.Utils;

namespace IronMan.Revit.Commands
{
    [Transaction(TransactionMode.Manual)]
    [Journaling(JournalingMode.NoCommandData)]
    [Regeneration(RegenerationOption.Manual)]
    public class ShieldF1Command : Toolkit.Mvvm.CommandBase
    {
        private static bool _hookSwitch = false;
        public override Window CreateMainWindow()
        {
            return null;
        }

        public override Result Execute(ref string message, ElementSet elements)
        {
            ThreadHook1 threadHook = SingletonIOC.Current.Container.GetInstance<ThreadHook1>();
            threadHook.SetHook();
            //if (_hookSwitch)
            //{
            //    threadHook.SetHook();
            //    _hookSwitch = true;
            //}
            //else
            //{
            //    threadHook.UnHook();
            //    _hookSwitch= false;
            //}
            return Result.Succeeded;
        }
    }
}
