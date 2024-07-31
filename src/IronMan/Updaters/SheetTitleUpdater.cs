using Autodesk.Revit.DB;
using IronMan.Revit.IServices;
using IronMan.Revit.Toolkit.Extension;
using IronMan.Revit.Toolkit.Mvvm.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IronMan.Revit.Updaters
{
    public class SheetTitleUpdater : ISheetTitleUpdater
    {
        private IUIProvider _uiProvider;
        private ISheetTitleServicve _sheetTitleServicveProvider;
        private readonly IDataContext _dataContext;

        public SheetTitleUpdater(IUIProvider uiProvider, ISheetTitleServicve sheetTitleServicveProvider, IDataContext dataContext)
        {
            _uiProvider = uiProvider;
            _sheetTitleServicveProvider = sheetTitleServicveProvider;
            _dataContext = dataContext;
        }

        public void Execute(UpdaterData data)
        {
            var document = data.GetDocument();
            var addedIds = data.GetAddedElementIds();
            if (addedIds.Count > 0)
            {
                foreach (var id in addedIds)
                {
                    var element = document.GetElement(id);
                    if (element is Viewport viewport)
                    {
                        _sheetTitleServicveProvider.CreateSheetTitls(viewport);
                    }
                }
                return;
            }

            var deletedIds = data.GetDeletedElementIds();
            if (deletedIds.Count>0)
            {
                _sheetTitleServicveProvider.DeleteSheetTitle();
                return;
            }

            var modifideIds =data.GetModifiedElementIds();
            if (modifideIds.Count > 0)
            {
                foreach (var id in modifideIds)
                {
                    var element = document.GetElement(id );
                    if(element is Viewport viewport)
                    {
                        if (data.IsChangeTriggered(id, Element.GetChangeTypeAny()))
                        {
                            _sheetTitleServicveProvider.UpdateSheetTitle(viewport);
                        }
                    }
                }
                return;
            }

        }

        public string GetAdditionalInformation() => "this is a updater which to monitor viewport action";

        public ChangePriority GetChangePriority() => ChangePriority.Annotations;

        public UpdaterId GetUpdaterId() => new UpdaterId(_uiProvider.GetAddInId(), new Guid("DF3961D9-901F-4FE6-AF87-54AC20314260"));

        public string GetUpdaterName() => nameof(SheetTitleUpdater);

        public void Register()
        {
            UpdaterRegistry.RegisterUpdater(this);

            ChangeType changeType = null;
            var instance = _dataContext.GetDocument().GetElements<FamilyInstance>().First();
            var parameters = instance.GetOrderedParameters();
            foreach (var item in parameters)
            {
                var itemChangeType = Element.GetChangeTypeParameter(item);
                changeType = ChangeType.ConcatenateChangeTypes(changeType, itemChangeType);
            }

            UpdaterRegistry.AddTrigger(this.GetUpdaterId(), new ElementClassFilter(typeof(Viewport)), Element.GetChangeTypeElementAddition());
            UpdaterRegistry.AddTrigger(this.GetUpdaterId(), new ElementClassFilter(typeof(Viewport)), Element.GetChangeTypeElementDeletion());
            UpdaterRegistry.AddTrigger(this.GetUpdaterId(), new ElementClassFilter(typeof(Viewport)), Element.GetChangeTypeAny());
        }

        public void Unregister()
        {
            if (UpdaterRegistry.IsUpdaterRegistered(this.GetUpdaterId()))
            {
                UpdaterRegistry.UnregisterUpdater(this.GetUpdaterId());
            }
        }
    }
}
