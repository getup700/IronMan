using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using IronMan.Revit.Toolkit.Extension;
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
    public class SizeConvertCommand : CommandBase
    {
        public override Window CreateMainWindow()
        {
            return null;
            //return SingletonIOC.Current.Container.Resolve<SizeConvertView, SizeConvertViewModel>(true);
        }

        public override Result Execute(ref string message, ElementSet elements)
        {
            //MainWindow.Show();
            var ele = DataContext.GetDocument()
                .GetElements<DirectShape>(x => x.Category.GetBuiltInCategory() == BuiltInCategory.OST_GenericModel);
            var deleteIds = new List<ElementId>();
            foreach (var item in ele)
            {
                var para = item.GetParameters("Reference(Pset_SpaceCommon)").FirstOrDefault();
                if (para != null)
                {
                    var value = para.AsString();
                    if (!string.IsNullOrEmpty(value))
                    {
                        deleteIds.Add(item.Id);
                    }
                }
            }
            var targetInstance = ele
                .Where(x => x.GetParameters("Reference(Pset_SpaceCommon)").FirstOrDefault().AsString() != "");
            var names = targetInstance.Select(x => x.Name).ToList();
            
            DataContext.GetDocument().NewTransaction(() =>
            {
                var ids = targetInstance.Select(x => x.Id).ToList();
                DataContext.GetDocument().Delete(deleteIds);
            });
            return Result.Succeeded;
        }
    }
}
