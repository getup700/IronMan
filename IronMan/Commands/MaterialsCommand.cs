using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using CommonServiceLocator;
using GalaSoft.MvvmLight.Ioc;
using IronMan.Interfaces;
using IronMan.IServices;
using IronMan.Services;
using IronMan.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace IronMan.Commands
{
    [Transaction(TransactionMode.Manual)]
    [Journaling(JournalingMode.NoCommandData)]
    [Regeneration(RegenerationOption.Manual)]
    public class MaterialsCommand : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uiDoc = commandData.Application.ActiveUIDocument;
            Document document = uiDoc.Document;

            ///构建IOC容器
            //ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            /////注册服务
            /////接口和细节的注入，实例注入
            //SimpleIoc.Default.Register<Document>(() => document);
            //SimpleIoc.Default.Register<IDataContext, DataContext>();
            //SimpleIoc.Default.Register<IMaterialService, MaterialService>();

            //SimpleIoc.Default.Register<MaterialsViewModel>();

            /////使用服务
            /////IOC，Provider拿到服务，依赖注入=>构造函数注入、方法注入、属性注入
            /////do something
            //ServiceLocator.Current.GetInstance<MaterialsViewModel>();



            IDataContext dataContext = new DataContext(document);
            Views.MaterialsWindow materialsWindow = new Views.MaterialsWindow(document);
            TransactionStatus status;
            using (TransactionGroup groups = new TransactionGroup(document, "材质管理"))
            {
                groups.Start();
                if (materialsWindow.ShowDialog().Value)
                {
                    status = groups.Assimilate();
                }
                else
                {
                    status = groups.RollBack();
                }
            }
            if (status == TransactionStatus.Committed)
            {
                return Result.Succeeded;
            }
            return Result.Cancelled;
        }
    }
}
