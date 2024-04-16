using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Events;
using Autodesk.Revit.DB.ExtensibleStorage;
using RvtEntity = Autodesk.Revit.DB.ExtensibleStorage.Entity;
using IronMan.Revit.Entity;
using IronMan.Revit.Toolkit.Extension;
using IronMan.Revit.Toolkit.Mvvm.Interfaces;
using IronMan.Revit.Toolkit.Mvvm.IOC;
using IronMan.Revit.Toolkit.Mvvm.Service.ExtensibleService;
using IronMan.Revit.Updaters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using DataStorage = Autodesk.Revit.DB.ExtensibleStorage.DataStorage;
using Autodesk.Revit.UI;

namespace IronMan.Revit
{
    public class AppEvent : IEventManager
    {
        private readonly IUIProvider _uiProvider;
        private readonly IDataStorage _dataStorage;
        private readonly IDataContext _dataContext;
        private readonly ISheetTitleUpdater _sheetTitleUpdater;

        public AppEvent(IUIProvider uiProvider, IDataStorage dataStorage, IDataContext dataContext, ISheetTitleUpdater sheetTitleUpdater)
        {
            _uiProvider = uiProvider;
            _dataStorage = dataStorage;
            _dataContext = dataContext;
            _sheetTitleUpdater = sheetTitleUpdater;
        }

        public void Subscribe()
        {
            _uiProvider.GetApplication().DocumentOpened += AppEvent_DocumentOpened;
            _uiProvider.GetApplication().DocumentClosed += AppEvent_DocumentClosed;
            _uiProvider.GetApplication().DocumentCreated += AppEvent_DocumentCreated;

            //WindowCenteredDMU windowCenteredDMU = new WindowCenteredDMU(_uiProvider.GetAddInId());
            ////ElementFilter filter = new ElementCategoryFilter(BuiltInCategory.OST_Walls);
            ////UpdaterRegistry.RegisterUpdater(windowCenteredDMU, false);
            ////UpdaterRegistry.AddTrigger(windowCenteredDMU.GetUpdaterId(), filter, Element.GetChangeTypeAny());
            //bool flag = UpdaterRegistry.IsUpdaterRegistered(windowCenteredDMU.GetUpdaterId());
            //if (flag)
            //{
            //    UpdaterRegistry.UnregisterUpdater(windowCenteredDMU.GetUpdaterId());
            //}
            //_sheetTitleUpdater.Register();

        }

        public void Unsubscribe()
        {
            _uiProvider.GetApplication().DocumentOpened -= AppEvent_DocumentOpened;
            _uiProvider.GetApplication().DocumentClosed -= AppEvent_DocumentClosed;
            _uiProvider.GetApplication().DocumentCreated -= AppEvent_DocumentCreated;

            _sheetTitleUpdater.Unregister();

        }

        private void AppEvent_DocumentCreated(object sender, DocumentCreatedEventArgs e)
        {
            Document document = _dataContext.GetDocument();
            var filters = document.GetElements<ParameterFilterElement>();
            Schema schema = _dataStorage.GetSchema(typeof(ParameterFilterProxy));
            var entity = new RvtEntity(schema);

            Schema filterSetSchema = _dataStorage.GetSchema(typeof(DocumentProxy));
            var filterSetEntity = new RvtEntity(filterSetSchema);
            document.NewTransaction(() =>
            {
                DataStorage dataStorage = DataStorage.Create(document);
                dataStorage.Name = Contants.IronManID.DataStorageName;
                if (dataStorage.GetEntity(filterSetSchema).Schema == null)
                {
                    dataStorage.SetEntity(filterSetEntity);
                }
                foreach (var filter in filters)
                {
                    filter.SetEntity(entity);
                }
            }, "Attach Data");

            var addLabelDMU = SingletonIOC.Current.Container.GetInstance<AddLabelToParameterFilterDMU>();
            UpdaterRegistry.RegisterUpdater(addLabelDMU, true);
            UpdaterRegistry.AddTrigger(addLabelDMU.GetUpdaterId(), new ElementClassFilter(typeof(ParameterFilterElement)), Element.GetChangeTypeElementAddition());
        }

        private void AppEvent_DocumentClosed(object sender, DocumentClosedEventArgs e)
        {
            var addLabelDMU = SingletonIOC.Current.Container.GetInstance<AddLabelToParameterFilterDMU>();
            UpdaterRegistry.RemoveAllTriggers(addLabelDMU.GetUpdaterId());

        }

        private void AppEvent_DocumentOpened(object sender, DocumentOpenedEventArgs e)
        {
            Document document = _dataContext.GetDocument();
            document.NewTransaction(() =>
            {
                Schema labelSetSchema = _dataStorage.GetSchema(typeof(DocumentProxy));
                if (document.GetRvtEntity(Contants.IronManID.DataStorageName, labelSetSchema) == null)
                {
                    DataStorage dataStorage = DataStorage.Create(document);
                    dataStorage.Name = Contants.IronManID.DataStorageName;
                    if (dataStorage.GetEntity(labelSetSchema).Schema == null)
                    {
                        var filterSetEntity = new RvtEntity(labelSetSchema);
                        dataStorage.SetEntity(filterSetEntity);
                    }
                }

                var filters = document.GetElements<ParameterFilterElement>();
                Schema filterSchema = _dataStorage.GetSchema(typeof(ParameterFilterProxy));
                var filterEntity = new RvtEntity(filterSchema);
                foreach (var filter in filters)
                {
                    if (filter.GetEntity(filterSchema).Schema == null)
                    {
                        filter.SetEntity(filterEntity);
                    }
                }
            }, "Attach Data");

            var addLabelDMU = SingletonIOC.Current.Container.GetInstance<AddLabelToParameterFilterDMU>();
            UpdaterRegistry.RegisterUpdater(addLabelDMU, true);
            UpdaterRegistry.AddTrigger(addLabelDMU.GetUpdaterId(), new ElementClassFilter(typeof(ParameterFilterElement)), Element.GetChangeTypeElementAddition());
            IList<UpdaterInfo> updaters = UpdaterRegistry.GetRegisteredUpdaterInfos();

            string info = string.Empty;
            foreach (var updater in updaters)
            {
                info += $"{updater.ApplicationName}\t\t{updater.AdditionalInformation}\t{updater.IsOptional}\n";
            }
            TaskDialog.Show("title", info);
            //WindowCenteredDMU windowCenteredDMU = SingletonIOC.Current.Container.GetInstance<WindowCenteredDMU>();
            ////ElementFilter filter = new ElementCategoryFilter(BuiltInCategory.OST_Walls);
            ////UpdaterRegistry.RegisterUpdater(windowCenteredDMU, false);
            ////UpdaterRegistry.AddTrigger(windowCenteredDMU.GetUpdaterId(), filter, Element.GetChangeTypeAny());
            //bool flag = UpdaterRegistry.IsUpdaterRegistered(windowCenteredDMU.GetUpdaterId());
            //if (flag)
            //{
            //    UpdaterRegistry.UnregisterUpdater(windowCenteredDMU.GetUpdaterId());
            //}
            //UpdaterRegistry.RemoveAllTriggers(windowCenteredDMU.GetUpdaterId());
            //bool result = UpdaterRegistry.IsUpdaterRegistered(windowCenteredDMU.GetUpdaterId(), document);
            //UpdaterRegistry.UnregisterUpdater(windowCenteredDMU.GetUpdaterId(), document);
            _sheetTitleUpdater.Register();
        }
    }
}
