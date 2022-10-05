using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using GalaSoft.MvvmLight.Ioc;
using IronMan.Revit.Toolkit.Mvvm.Interfaces;
using IronMan.Revit.Toolkit.Mvvm.IOC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IronMan.Revit.Toolkit.Mvvm
{
    public class DataContext : IDataContext
    {
        public Document Document { get => GetDocument(); }
        public Document GetDocument()
        {
            //每次点击Command都需要新的对象，所以通过IOC获取新的实例，而不是传入Document构造实例
            return SingletonIOC.Current.Container.GetInstance<Document>();
        }

        public UIApplication GetUIApplication()
        {
            return GetUIDocument().Application;
        }

        public UIDocument GetUIDocument()
        {
            return new UIDocument(GetDocument());
        }
    }
}
