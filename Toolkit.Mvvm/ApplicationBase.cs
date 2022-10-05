using Autodesk.Revit.UI;
using CommonServiceLocator;
using GalaSoft.MvvmLight.Ioc;
using IronMan.Revit.Toolkit.Mvvm.Interfaces;
using IronMan.Revit.Toolkit.Mvvm.IOC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.Design;

namespace IronMan.Revit.Toolkit.Mvvm
{
    public abstract class ApplicationBase : IExternalApplication
    {
        public abstract void RegisterTyped(SimpleIoc container);
        public static SimpleIoc current =  SingletonIOC.Current.Container;

        public Result OnShutdown(UIControlledApplication application)
        {
            var events  = ServiceLocator.Current.GetInstance<IEventManager>();
            if(events != null)
            {
                events.Unsubscribe();
            }
            return Result.Succeeded;
        }

        public Result OnStartup(UIControlledApplication application)
        {
            if(!ServiceLocator.IsLocationProviderSet)
            {
                ServiceLocator.SetLocatorProvider(()=>SingletonIOC.Current.Container);
            }
            //必需的注入
            SingletonIOC.Current.Container.Register<UIControlledApplication>(() => application);
            SingletonIOC.Current.Container.Register<IUIProvider, UIProvider>();
            SingletonIOC.Current.Container.Register<IDataContext, DataContext>();

            //自定义服务注入
            RegisterTyped(SingletonIOC.Current.Container);

            //IOC中订阅事件
            var events = ServiceLocator.Current.GetInstance<IEventManager>();
            if(events != null)
            {
                events.Subscribe();
            }

            //IOC中获取RibbonUI
            var appUI = ServiceLocator.Current.GetInstance<IApplicationUI>();
            return appUI == null ? Result.Cancelled : appUI.Initial();

        }
    }
}
