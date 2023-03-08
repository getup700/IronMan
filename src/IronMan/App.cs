using Autodesk.Revit.UI;
using GalaSoft.MvvmLight.Ioc;
using IronMan.Revit.IServices;
using IronMan.Revit.Toolkit.Mvvm;
using IronMan.Revit.Toolkit.Mvvm.Interfaces;
using IronMan.Revit.Services;
using IronMan.Revit.ViewModels;
using IronMan.Revit.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IronMan.Revit.Toolkit.Mvvm.IOC;
using IronMan.Revit.DockablePanes;
using IronMan.Revit.Commands;
using IronMan.Revit.Updaters;
using Autodesk.Revit.DB;
using IronMan.Revit.Toolkit.Mvvm.Service.ExtensibleService;
using IronMan.Revit.Entity;
using System.Reflection;
using System.IO;
using System.Windows;
using System.Diagnostics;
using IronMan.Revit.Entity.Toolkit;
using IronMan.Revit.Utils;

namespace IronMan.Revit
{
    public class App : EntityApplicationBase
    {
        public sealed override void RegisterTyped(SimpleIoc container)
        {
            //Offical Register
            container.Register<IApplicationUI, AppUI>();
            container.Register<IEventManager, AppEvent>();

            //Register Service
            container.Register<IExternalEventService, ExternalEventService>();
            container.Register<IFamilyService, FamilyService>();
            container.Register<IFloorService, FloorService>();
            container.Register<IProgressBarService, ProgressBarService>();
            container.Register<IMaterialService, MaterialService>();
            container.Register<IWallService, WallService>();
            container.Register<ICollectService, CollectService>();
            container.Register<IRoomLengthExcelService, RoomLengthExcelService>();
            container.Register<IMepService, MepService>();
            container.Register<IParameterFilterService, ParameterFilterService>();
            container.Register<IParameterFilterLabelService, ParameterFilterLaberService>();
            container.Register<ISheetTitleServicve,SheetTitleService>();
            container.Register<IDefinitionParameterService, DefinitionParameterService>();
            container.Register<CadToRevitService>();
            container.Register<SmartRoomService>();
            container.Register<MepInfomationService>();
            container.Register<DocumentParameterService>();

            //Register ViewModel
            container.Register<DataCollectViewModel>();
            container.Register<ProgerssBarDialogViewModel>();
            container.Register<MaterialsViewModel>();
            container.Register<MaterialDialogViewModel>();
            container.Register<FloorTypeManageViewModel>();
            container.Register<CadToRevitViewModel>();
            container.Register<QuicklyWallViewModel>();
            container.Register<ModeViewModel>();
            container.Register<DockablePaneControlViewModel>();
            container.Register<SmartRoomViewModel>();
            container.Register<ParameterFilterLabelDialogViewModel>();
            container.Register<ParameterFilterLabelLibraryViewModel>();
            container.Register<InformationImportViewModel>();
            container.Register<InformationExportViewModel>();

            //Register View
            container.Register<ProgressBarDialog>();
            container.Register<MaterialsWindow>();
            container.Register<MaterialDialogWindow>();
            container.Register<FloorTypeManageView>();
            container.Register<CadToRevitView>();
            container.Register<QuicklyWallView>();
            container.Register<SmartRoomView>();
            container.Register<DockablePaneControlView>();
            container.Register<ModeView>();
            container.Register<TransparentView>();
            container.Register<FilterDialogView>();
            container.Register<FilterLabelLibrary>();

            //Register DockablePane
            container.Register<FrameworkElementCreator>();
            var application = container.GetInstance<UIControlledApplication>();
            application.RegisterDockablePane(DockablePanes.DockablePaneProvider.Id, "DockablePane", new DockablePaneProvider());

            //Register DMU
            container.Register<AddLabelToParameterFilterDMU>();
            container.Register<WindowCenteredDMU>();
            container.Register<ISheetTitleUpdater, SheetTitleUpdater>();
            //container.Register<WindowCenteredDMU>();
            //container.Register<GetModelLineDMU>();
            //GetModelLineDMU getModelLineDMU = SingletonIOC.Current.Container.GetInstance<GetModelLineDMU>();
            //UpdaterRegistry.RegisterUpdater(getModelLineDMU, false);

            //Register ApplicationData
            //container.Register<AddInId>();

            string currentDirectory = this.GetType().Assembly.Location;
            var myPath = Path.GetDirectoryName(currentDirectory);
            string path = Process.GetCurrentProcess().MainModule.FileName;
            string path2 = AppDomain.CurrentDomain.BaseDirectory;
            string path3 = this.GetType().Assembly.Location;
            string handyControlPath = Path.Combine(myPath, "HandyControl.dll");
            Assembly.LoadFrom(handyControlPath);

            container.Register<ThreadHook>(() => new ThreadHook());
        }
        public sealed override void RegisterSchema(IDataStorage dataStorage)
        {
            dataStorage.Append<ParameterFilterProxy>();
            dataStorage.Append<DocumentProxy>();
        }

    }
}
