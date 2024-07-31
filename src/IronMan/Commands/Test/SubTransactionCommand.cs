using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using CommonServiceLocator;
using IronMan.Revit.Toolkit.Extension;
using IronMan.Revit.Toolkit.Mvvm.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace IronMan.Revit.Commands.Test
{
    internal class SubTransactionCommand : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            var dataContext = ServiceLocator.Current.GetInstance<IDataContext>();
            var doc = dataContext.GetDocument();
            var xyz = XYZ.Zero;
            doc.NewTransaction(() =>
            {
                var symbol = doc.GetElements<FamilySymbol>(x => x.Family.FamilyCategory.Id.IntegerValue == (int)BuiltInCategory.OST_GenericModel)
                    .FirstOrDefault();
                doc.Create.NewFamilyInstance(xyz, symbol, structuralType: StructuralType.NonStructural);
                doc.Create.NewFamilyInstance(xyz, symbol, structuralType: StructuralType.NonStructural);
                doc.NewSubTransaction(() =>
                {
                    doc.Create.NewFamilyInstance(null, symbol, structuralType: StructuralType.NonStructural);
                });
                doc.Create.NewFamilyInstance(null, symbol, structuralType: StructuralType.NonStructural);
            });
            return Result.Succeeded;
        }
    }
}
