using Autodesk.Revit.UI;
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
using IronMan.Revit.Plugin;
using Microsoft.Extensions.DependencyInjection;

namespace IronMan.Revit
{
    public class App : ApplicationBase
    {
        public sealed override void RegisterTyped(IServiceCollection container)
        {
            container.AddSingleton<IApplicationUI, AppUI>();
            container.AddSingleton<IEventManager, AppEvent>();

            container.AddSingleton<IExternalEventService, ExternalEventService>();
            container.AddSingleton<IFamilyService, FamilyService>();
            container.AddSingleton<IFloorService, FloorService>();
            container.AddSingleton<IProgressBarService, ProgressBarService>();
            container.AddSingleton<IMaterialService, MaterialService>();
            container.AddSingleton<IWallService, WallService>();
            container.AddSingleton<ICollectService, CollectService>();
            container.AddSingleton<IRoomLengthExcelService, RoomLengthExcelService>();
            container.AddSingleton<IMepService, MepService>();
            container.AddSingleton<IParameterFilterService, ParameterFilterService>();
            container.AddSingleton<IParameterFilterLabelService, ParameterFilterLaberService>();
            container.AddSingleton<ISheetTitleServicve,SheetTitleService>();
            container.AddSingleton<IDefinitionParameterService, DefinitionParameterService>();
            container.AddSingleton<CadToRevitService>();
            container.AddSingleton<SmartRoomService>();
            container.AddSingleton<MepInfomationService>();
            container.AddSingleton<DocumentParameterService>();

            container.AddSingleton<DataCollectViewModel>();
            container.AddSingleton<ProgerssBarDialogViewModel>();
            container.AddSingleton<MaterialsViewModel>();
            container.AddSingleton<MaterialDialogViewModel>();
            container.AddSingleton<FloorTypeManageViewModel>();
            container.AddSingleton<CadToRevitViewModel>();
            container.AddSingleton<QuicklyWallViewModel>();
            container.AddSingleton<ModeViewModel>();
            container.AddSingleton<DockablePaneControlViewModel>();
            container.AddSingleton<SmartRoomViewModel>();
            container.AddSingleton<ParameterFilterLabelDialogViewModel>();
            container.AddSingleton<ParameterFilterLabelLibraryViewModel>();
            container.AddSingleton<InformationImportViewModel>();
            container.AddSingleton<InformationExportViewModel>();

            container.AddSingleton<ProgressBarDialog>();
            container.AddSingleton<MaterialsWindow>();
            container.AddSingleton<MaterialDialogWindow>();
            container.AddSingleton<FloorTypeManageView>();
            container.AddSingleton<CadToRevitView>();
            container.AddSingleton<QuicklyWallView>();
            container.AddSingleton<SmartRoomView>();
            container.AddSingleton<DockablePaneControlView>();
            container.AddSingleton<ModeView>();
            container.AddSingleton<TransparentView>();
            container.AddSingleton<FilterDialogView>();
            container.AddSingleton<FilterLabelLibrary>();

            container.AddSingleton<IFamilyLoadOptions, FamilyLoadOptions>();

            container.AddSingleton<FrameworkElementCreator>();
            //var application = container.GetInstance<UIControlledApplication>();
            //application.RegisterDockablePane(DockablePanes.DockablePaneProvider.Id, "DockablePane", new DockablePaneProvider());

            container.AddSingleton<AddLabelToParameterFilterDMU>();
            container.AddSingleton<WindowCenteredDMU>();
            container.AddSingleton<ISheetTitleUpdater, SheetTitleUpdater>();
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

        }
        public sealed override void RegisterSchema(IDataStorage dataStorage)
        {
            dataStorage.Append<ParameterFilterProxy>();
            dataStorage.Append<DocumentProxy>();
        }

    }
}
