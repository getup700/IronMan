using Autodesk.Revit.UI;
using CommonServiceLocator;
using GalaSoft.MvvmLight.Ioc;
using IronMan.Revit.Toolkit.Mvvm.Interfaces;
using IronMan.Revit.Toolkit.Mvvm.IOC;
using IronMan.Revit.Toolkit.Mvvm.Service.ExtensibleService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.Design;

namespace IronMan.Revit.Toolkit.Mvvm
{
    public abstract class ApplicationBase : IExternalApplication
    {
        public abstract void RegisterTyped(SimpleIoc container);

        public abstract void RegisterSchema(IDataStorage dataStorage);

        public virtual void RegisterField(SimpleIoc container) { }

        public static SimpleIoc Current = SingletonIOC.Current.Container;

        public Result OnShutdown(UIControlledApplication application)
        {
            var events = ServiceLocator.Current.GetInstance<IEventManager>();
            events?.Unsubscribe();
            return Result.Succeeded;
        }

        public Result OnStartup(UIControlledApplication application)
        {
            if (!ServiceLocator.IsLocationProviderSet)
            {
                ServiceLocator.SetLocatorProvider(() => SingletonIOC.Current.Container);
            }
            //必需的注入//通过接口和细节注入//通过实例注入
            SingletonIOC.Current.Container.Register<UIControlledApplication>(() => application);
            SingletonIOC.Current.Container.Register<IUIProvider, UIProvider>();
            SingletonIOC.Current.Container.Register<IDataContext, DataContext>();
            SingletonIOC.Current.Container.Register<IDataStorage, DataStorage>();

            //注入应用信息
            IApplicationData applicationData = GetApplicationData();
            SingletonIOC.Current.Container.Register<IApplicationData>(() => applicationData);

            //自定义服务注入
            RegisterTyped(SingletonIOC.Current.Container);
            RegisterSchema(SingletonIOC.Current.Container.GetInstance<IDataStorage>());
            RegisterField(SingletonIOC.Current.Container);

            //IOC中订阅事件
            var events = SingletonIOC.Current.Container.GetInstance<IEventManager>();
            events?.Subscribe();

            //IOC中获取RibbonUI
            var appUI = SingletonIOC.Current.Container.GetInstance<IApplicationUI>();
            return appUI == null ? Result.Cancelled : appUI.Initial();

        }

        private IApplicationData GetApplicationData()
        {
            Assembly assembly = typeof(IApplicationData).Assembly;
            Type[] types = assembly.GetTypes();
            if (types.Count() == 0) return null;
            Type type = types[0];
            if (type.GetInterfaces().Length > 0 && !type.IsAbstract)
            {
                if (type.GetInterfaces()[0] == typeof(IApplicationData))
                {
                    IApplicationData instance = (IApplicationData)Activator.CreateInstance(type);
                    return instance;
                }
            }
            return null;
        }
    }
}
