using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IronMan.Revit.Commands.Test.ExtensibleStorage
{
    [Transaction(TransactionMode.Manual)]
    [Journaling(JournalingMode.NoCommandData)]
    [Regeneration(RegenerationOption.Manual)]
    internal class AttachDataCommand : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            throw new NotImplementedException();
        }
    }
}
