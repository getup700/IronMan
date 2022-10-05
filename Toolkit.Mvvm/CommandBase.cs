using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;
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
using System.Windows;
using UIFramework;

namespace IronMan.Revit.Toolkit.Mvvm
{
    public abstract class CommandBase : IExternalCommand
    {
        //自定义服务注入
        public virtual void RegisterTypes(SimpleIoc simpleIoc) { }

        //程序主窗体
        public abstract Window CreateMainWindow();

        public abstract Result Execute(ref string message, ElementSet elements);

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            //注入Document
            SingletonIOC.Current.Container.Register<Document>(() => commandData.Application.ActiveUIDocument.Document);
            //自定义服务注入
            RegisterTypes(SingletonIOC.Current.Container);

            Window window = CreateMainWindow();
            if (window != null)
            {
                MainWindow = window;
            }

            //执行命令
            var result = Execute(ref message, elements);

            //取消注册Document
            SingletonIOC.Current.Container.Unregister<Document>();
            return result;
        }
        public Window MainWindow { get; set; }

        //如果没有窗体可以通过DataContext编写，不用再写UIDocument，Document
        protected IDataContext DataContext { get => ServiceLocator.Current.GetInstance<IDataContext>(); }
    }
}
