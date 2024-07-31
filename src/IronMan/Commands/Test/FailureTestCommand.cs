using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using IronMan.Revit.Toolkit.Extension;
using IronMan.Revit.Toolkit.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace IronMan.Revit.Commands.Test
{
    [Transaction(TransactionMode.Manual)]
    [Journaling(JournalingMode.NoCommandData)]
    [Regeneration(RegenerationOption.Manual)]
    public class FailureTestCommand : Toolkit.Mvvm.CommandBase
    {
        public override Window CreateMainWindow()
        {
            return null;
        }

        public override Result Execute(ref string message, ElementSet elements)
        {
            var document = DataContext.GetDocument();
            document.NewTransaction(() =>
            {
                var familys = document.GetElements<Family>(x => x.Category.Name == "柱");
                FamilySymbol familySymbol = familys.FirstOrDefault().GetFamilySymbolIds() as FamilySymbol;
                if (familySymbol == null)
                {
                    return;
                }
                if (!familySymbol.IsActive)
                {
                    familySymbol.Activate();
                }
                document.Create.NewFamilyInstance(XYZ.Zero, familySymbol, Autodesk.Revit.DB.Structure.StructuralType.Column);
            });

            return Result.Succeeded;
        }
    }
}
