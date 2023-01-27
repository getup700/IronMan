using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using CommonServiceLocator;
using GalaSoft.MvvmLight.Ioc;
using IronMan.Revit.Interfaces;
using IronMan.Revit.IServices;
using IronMan.Revit.Toolkit.Mvvm;
using IronMan.Revit.Toolkit.Mvvm.Extension;
using IronMan.Revit.Services;
using IronMan.Revit.ViewModels;
using IronMan.Revit.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using IronMan.Revit.Toolkit.Extension;
using IronMan.Revit.Toolkit.Mvvm.IOC;

namespace IronMan.Revit.Commands
{
    [Transaction(TransactionMode.Manual)]
    [Journaling(JournalingMode.NoCommandData)]
    [Regeneration(RegenerationOption.Manual)]
    public class MaterialsCommand : Toolkit.Mvvm.CommandBase
    {
        public sealed override Window CreateMainWindow()
        {
            return SingletonIOC.Current.Container.Resolve<MaterialsWindow, MaterialsViewModel>(false);//模态窗口-单例窗口
        }

        public sealed override Result Execute(ref string message, ElementSet elements)
        {
            TransactionStatus status = DataContext.GetDocument().NewTransactionGroup("资源管理", () => MainWindow.ShowDialog().Value);
            return status == TransactionStatus.Committed ? Result.Succeeded : Result.Cancelled;
        }

        //调用方需要注入新东西则在此注入
        public sealed override void RegisterTypes(SimpleIoc simpleIoc)
        {
            base.RegisterTypes(simpleIoc);
        }

        //public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        //{

        //UIDocument uiDoc = commandData.Application.ActiveUIDocument;
        //Document document = uiDoc.Document;

        ////判断Provider是否注册
        //if(!ServiceLocator.IsLocationProviderSet)
        //{
        //    ServiceLocator.SetLocatorProvider(() => SingletonIOC.Current.Container);
        //}
        //SingletonIOC.Current.Container.Reset();//清空所有IOC，类似于Clear//Reset之后，全部变为瞬时
        /////A构建IOC容器
        //ServiceLocator.SetLocatorProvider(() => SingletonIOC.Current.Container);

        /////B注册服务
        /////服务有两种：1接口和细节的注入；2实例注入；
        /////1：注入实例
        //SingletonIOC.Current.Container.Register<Document>(() => document);//直接给一个实例
        //SingletonIOC.Current.Container.Register<MaterialsViewModel>();
        /////2：注入接口和细节
        /////ctor需要数据上下文，IOC自动创建实例，并且ctor需要什么参数就传入什么参数
        ///////容器会检索构造函数，ctor需要Document就给她传入Document
        //SingletonIOC.Current.Container.Register<IDataContext, DataContext>();
        //SingletonIOC.Current.Container.Register<IMaterialService, MaterialService>();//使用时通过MaterialService拿到细节
        //SingletonIOC.Current.Container.Register<MaterialsWindow>();




        /////C使用服务
        /////第一种使用IOC，Provider拿到服务
        /////第二种依赖注入=>三种方式：构造函数注入、方法注入、属性注入
        /////构造函数注入：数据上下文注入到IOC时，通过IOC可实例化上下文，实例化时IOC自动检测datacontext的ctor，ctor需要什么给她传什么值
        //Views.MaterialsWindow materialsWindow = ServiceLocator.Current.GetInstance<MaterialsWindow>();//这是一个瞬时
        //materialsWindow.DataContext =  ServiceLocator.Current.GetInstance<MaterialsViewModel>();//IOC给创建的VM

        ////使用IOC容器也可以SingletonIOC.Current.Container.GetInstance<MaterialsViewModel>();但是使用ServiceLocator可灵活更换IOC


        //SingletonIOC.Current.Container.GetAllInstances<IMaterialService>();//这种方式如果没有实例就会创建新的
        ////var services = SingletonIOC.Current.Container.GetAllCreatedInstances<MaterialsWindow>();//这种方式没有实例不会创建新的
        ////MessageBox.Show($"{services.Count()}-{services.FirstOrDefault()?.GetHashCode()}");


        //SingletonIOC.Current.Container.IsRegistered<MaterialsViewModel>();
        /////生命周期管理：从实例被创建，到被销毁的过程
        /////常规的IOC管理器（Autofac,Unity,DI）都会有这三种模式
        /////生命周期管理模式：1.Singleton单例 2.Scope区域瞬时 3.Transient瞬时
        /////1单例：创建后一直在内存，直到应用结束；（默认情况）
        /////2瞬时：请求时才会提供新的实例；
        /////3区域瞬时：代码块间实例，离开就结束；（介于单例与瞬时）

        ////IDataContext dataContext = new DataContext(document);
        ////Views.MaterialsWindow materialsWindow = new Views.MaterialsWindow(document);
        //TransactionStatus status;
        //using (TransactionGroup groups = new TransactionGroup(document, "材质管理"))
        //{
        //    groups.Start();
        //    if (materialsWindow.ShowDialog().Value)
        //    {
        //        status = groups.Assimilate();
        //    }
        //    else
        //    {
        //        status = groups.RollBack();
        //    }
        //}
        /////D注销IOC服务
        //SingletonIOC.Current.Container.Unregister<Document>();

        //if (status == TransactionStatus.Committed)
        //{
        //    return Result.Succeeded;
        //}
        //    //return Result.Cancelled;
        //}

    }
}
