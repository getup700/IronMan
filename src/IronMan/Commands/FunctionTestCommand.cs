using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using HandyControl.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IronMan.Revit.Commands
{
    [Transaction(TransactionMode.Manual)]
    [Journaling(JournalingMode.NoCommandData)]
    [Regeneration(RegenerationOption.Manual)]
    public class FunctionTestCommand : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiApplication = commandData.Application;
            UIDocument uiDocument = uiApplication.ActiveUIDocument;
            Document document = uiDocument.Document;
            Application application = uiApplication.Application;
            Application docApplication = document.Application;

            var updaterInfos = UpdaterRegistry.GetRegisteredUpdaterInfos();
            

            return Result.Succeeded;
        }
    }
}
