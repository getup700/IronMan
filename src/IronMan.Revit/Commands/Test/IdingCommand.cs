using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using CommonServiceLocator;
using IronMan.Revit.Toolkit.Extension;
using IronMan.Revit.Toolkit.Mvvm.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IronMan.Revit.Commands.Test
{
    internal class IdingCommand : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            var dataContext = ServiceLocator.Current.GetInstance<IDataContext>();
            var doc = dataContext.GetDocument();
            var uiapp = dataContext.GetUIApplication();
            uiapp.Idling += Uiapp_Idling;


            return Result.Succeeded;
        }

        private void Uiapp_Idling(object sender, Autodesk.Revit.UI.Events.IdlingEventArgs e)
        {
            var uiapp = sender as UIApplication;
            var doc = uiapp.ActiveUIDocument.Document;
            doc.NewTransaction(() =>
            {
                var textNode = doc.GetElements<FamilyInstance>(x => x.Category.Id.IntegerValue == (int)BuiltInCategory.OST_TextNotes)
                .FirstOrDefault();

            });
        }
    }
}
