﻿using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using IronMan.Revit.Entity.Attributes;
using IronMan.Revit.Toolkit.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace IronMan.Revit.Commands.PushButtons
{
    [Transaction(TransactionMode.Manual)]
    public class BackgroundConvertCommand : IExternalCommand
    {
        [ButtonName("反转背景")]
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            var app = commandData.Application.Application;
            var c = app.BackgroundColor;
            app.BackgroundColor = new Color((byte)(255 - c.Red), (byte)(255 - c.Green), (byte)(255 - c.Blue));
            return Result.Succeeded;
        }
    }
}
