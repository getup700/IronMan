using Autodesk.Revit.DB;
using Autodesk.Revit.DB.ExtensibleStorage;
using IronMan.Revit.Entity.Contants;
using IronMan.Revit.Toolkit.Extension;
using IronMan.Revit.Toolkit.Mvvm.Interfaces;
using System.Collections.Generic;
using System.Linq;
using RvtEntity = Autodesk.Revit.DB.ExtensibleStorage.Entity;

namespace IronMan.Revit.Updaters
{
    public class AddLabelToParameterFilterDMU : IUpdater
    {
        private readonly IUIProvider _uiProvider;

        public AddLabelToParameterFilterDMU(IUIProvider uiProvider)
        {
            _uiProvider = uiProvider;
        }

        public void Execute(UpdaterData data)
        {
            Document document = data.GetDocument();
            var AddedFilter = data.GetAddedElementIds().ToList();
            FilteredElementCollector elements = new FilteredElementCollector(document, AddedFilter);
            IEnumerable<ParameterFilterElement> filters = elements.OfClass(typeof(ParameterFilterElement)).Cast<ParameterFilterElement>();
            document.NewSubTransaction(() =>
            {
                if (filters.Count() != 0)
                {
                    Schema schema = Schema.Lookup(ExtensibleStorage.ParameterFilterElementInfoGuid);
                    var entity = new RvtEntity(schema);
                    if (schema != null)
                    {
                        foreach (var filter in filters)
                        {
                            filter.SetEntity(entity);
                        }
                    }
                }
            });
        }

        public string GetAdditionalInformation()
        {
            return "add extensiable storage to added filter";
        }

        public ChangePriority GetChangePriority()
        {
            return ChangePriority.GridsLevelsReferencePlanes;
        }

        public UpdaterId GetUpdaterId()
        {
            return new UpdaterId(_uiProvider.GetAddInId(), Contants.DynamicModelUpdate.AddLaberToParameterFilterGuid);
        }

        public string GetUpdaterName()
        {
            return nameof(AddLabelToParameterFilterDMU);
        }
    }
}
