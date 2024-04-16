using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;
using Autodesk.Revit.UI;
using IronMan.Revit.Toolkit.Mvvm.Interfaces;
using IronMan.Revit.Toolkit.Mvvm.IOC;
using IronMan.Revit.Toolkit.Mvvm.Service.ExtensibleService;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using UIFramework;

namespace IronMan.Revit.Toolkit.Mvvm
{
    public abstract class CommandBase : IExternalCommand
    {
        private bool _abailable = true;

        public IServiceProvider Provider { get; private set; }
        //程序主窗体
        public abstract Window CreateMainWindow();

        public abstract Result Execute(ref string message, ElementSet elements);

        [DebuggerStepThrough]
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Window window = CreateMainWindow();
            if (window != null)
            {
                MainWindow = window;
            }
            Result result = Execute(ref message, elements);
            //Result result = Result.Cancelled;

            //注入Document
            //SingletonIOC.Current.Container.Register<Document>(() => commandData.Application.ActiveUIDocument.Document);
            //自定义服务注入

            //try
            //{
            //    result = Execute(ref message, elements);
            //}
            //catch
            //{
            //    SingletonIOC.Current.Container.Unregister<Document>();
            //}

            //取消注册Document
            //SingletonIOC.Current.Container.Unregister<Document>();
            return result;
        }

        //public virtual bool IsCommandAvailable(UIApplication applicationData, CategorySet selectedCategories)
        //{
        //    Messenger.Default.Register<bool>(this,this, result => _abailable = result);
        //    return _abailable;
        //}

        public Window MainWindow { get; set; }

        //如果没有窗体可以通过DataContext编写，不用再写UIDocument，Document
        protected IDataContext DataContext { get => Provider.GetRequiredService<IDataContext>(); }

        protected IDataStorage Data => Provider.GetRequiredService<IDataStorage>();
    }
}
