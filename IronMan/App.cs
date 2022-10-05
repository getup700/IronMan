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

namespace IronMan.Revit
{
    public class App : ApplicationBase
    {
        public sealed override void RegisterTyped(SimpleIoc container)
        {
            //注册UI,事件
            container.Register<IApplicationUI, AppUI>();
            container.Register<IEventManager, AppEvent>();

            //注册Service
            container.Register<IMaterialService, MaterialService>();
            container.Register<IProgressBarService, ProgressBarService>();

            //注册ViewModel
            container.Register<MaterialsViewModel>();
            container.Register<ProgerssBarDialogViewModel>();

            //注册View
            container.Register<MaterialsWindow>();
            container.Register<ProgressBarDialog>();

            container.Register<IFloorTransformService,FloorTransformService>();
            container.Register<FloorTransformViewModel>();
            container.Register<FloorTransformView>();

            container.Register<IFloorTypeManageService, FloorTypeManageService>();
            container.Register<FloorTypeManageViewModel>();
            container.Register<FloorTypeManageView>();
        }
   
    }
}
