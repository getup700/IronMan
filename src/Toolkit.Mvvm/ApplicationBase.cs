using Autodesk.Revit.UI;
using IronMan.Revit.Toolkit.Mvvm.Interfaces;
using IronMan.Revit.Toolkit.Mvvm.IOC;
using IronMan.Revit.Toolkit.Mvvm.Service.ExtensibleService;
using Microsoft.Extensions.DependencyInjection;
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
        public abstract void RegisterTyped(IServiceCollection container);

        public abstract void RegisterSchema(IDataStorage dataStorage);

        public static IServiceProvider Provider { get; private set; }

        internal static Dictionary<string, IServiceProvider> Providers { get; private set; }
        public Result OnShutdown(UIControlledApplication application)
        {
            var events = Provider.GetRequiredService<IEventManager>();
            events?.Unsubscribe();
            return Result.Succeeded;
        }


        public Result OnStartup(UIControlledApplication application)
        {
            var services = new ServiceCollection()
                .AddSingleton(application)
                .AddSingleton<IUIProvider, UIProvider>()
                .AddSingleton<IDataContext, DataContext>()
                .AddSingleton<IDataStorage, DataStorage>();
            RegisterTyped(services);

            //RegisterSchema(SingletonIOC.Current.Container.GetInstance<IDataStorage>());
            if (Providers.TryGetValue("IronMan",out var outProvider))
            {
                if (outProvider == null)
                {
                    var provider = services.BuildServiceProvider();
                    Providers.Add("IronMan", provider);
                    Provider = provider;
                }
                else
                {
                    Provider = outProvider;
                }
            }

            //IOC中订阅事件
            var events = Provider.GetService<IEventManager>();
            events?.Subscribe();

            //IOC中获取RibbonUI
            var appUI = Provider.GetService<IApplicationUI>();
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
