using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using IronMan.Interfaces;
using IronMan.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace IronMan.Commands
{
    [Transaction(TransactionMode.Manual)]
    [Journaling(JournalingMode.NoCommandData)]
    [Regeneration(RegenerationOption.Manual)]
    public class MaterialsCommand : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uiDoc = commandData.Application.ActiveUIDocument;
            Document doc = uiDoc.Document;
            IDataContext dataContext = new DataContext(doc);
            Views.MaterialsWindow materialsWindow = new Views.MaterialsWindow(new MaterialService(dataContext));
            TransactionStatus status;
            using (TransactionGroup groups = new TransactionGroup(doc, "材质管理"))
            {
                groups.Start();
                if (materialsWindow.ShowDialog().Value)
                {
                    status = groups.Assimilate();
                }
                else
                {
                    status = groups.RollBack();
                }
            }
            if (status == TransactionStatus.Committed)
            {
                return Result.Succeeded;
            }
            return Result.Cancelled;
        }
    }
}
