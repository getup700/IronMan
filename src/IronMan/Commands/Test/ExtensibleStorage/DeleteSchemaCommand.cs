using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IronMan.Revit.Toolkit.Extension;
using Autodesk.Revit.DB.ExtensibleStorage;

namespace IronMan.Revit.Commands.Test.ExtensibleStorage
{
    internal class DeleteSchemaCommand : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            var doc = commandData.Application.ActiveUIDocument.Document;

            doc.NewTransaction(() =>
            {
                foreach (var item in Schema.ListSchemas())
                {
                    Schema.EraseSchemaAndAllEntities(item, true);
                }
            }, "Erase");

            return Result.Succeeded;
        }
    }
}
