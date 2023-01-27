using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.ExtensibleStorage;
using RvtEntity =Autodesk.Revit.DB.ExtensibleStorage.Entity;
using Autodesk.Revit.UI;
using IronMan.Revit.Entity;
using IronMan.Revit.Toolkit.Extension;
using IronMan.Revit.Toolkit.Mvvm;
using IronMan.Revit.Toolkit.Mvvm.Extension;
using IronMan.Revit.Toolkit.Mvvm.Interfaces;
using IronMan.Revit.Toolkit.Mvvm.IOC;
using IronMan.Revit.ViewModels;
using IronMan.Revit.Views;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace IronMan.Revit.Commands
{
    [Transaction(TransactionMode.Manual)]
    [Journaling(JournalingMode.NoCommandData)]
    [Regeneration(RegenerationOption.Manual)]
    public class ParameterFilterCommand : CommandBase
    {
        public override Window CreateMainWindow()
        {
            DataContext.GetDocument().NewTransaction(() =>
            {
                Schema filterSetSchema = Data.GetSchema(typeof(DocumentProxy));
                if (DataContext.GetDocument().GetRvtEntity(Contants.IronManID.DataStorageName, filterSetSchema) == null)
                {
                    DataStorage dataStorage = DataStorage.Create(DataContext.GetDocument());
                    dataStorage.Name = Contants.IronManID.DataStorageName;
                    if (dataStorage.GetEntity(filterSetSchema).Schema == null)
                    {
                        var filterSetEntity = new RvtEntity(filterSetSchema);
                        dataStorage.SetEntity(filterSetEntity);
                    }
                }

                var filters = DataContext.GetDocument().GetElements<ParameterFilterElement>();
                Schema filterSchema = Data.GetSchema(typeof(ParameterFilterProxy));
                var filterEntity = new RvtEntity(filterSchema);
                foreach (var filter in filters)
                {
                    if (filter.GetEntity(filterSchema).Schema == null)
                    {
                        filter.SetEntity(filterEntity);
                    }
                }
            }, "Attach Data");
            return SingletonIOC.Current.Container.Resolve<FilterView, ParameterFilterLabelViewModel>(false);
        }

        [DebuggerStepThrough]
        public override Result Execute(ref string message, ElementSet elements)
        {
            TransactionStatus status = DataContext.GetDocument().NewTransactionGroup("Stark过滤器", () => MainWindow.ShowDialog().Value);
            return status == TransactionStatus.Committed ? Result.Succeeded : Result.Cancelled;
        }
    }
}
