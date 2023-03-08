using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using GalaSoft.MvvmLight.Ioc;
using IronMan.Revit.Toolkit.Mvvm.Interfaces;
using IronMan.Revit.Toolkit.Mvvm.IOC;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace IronMan.Revit.Toolkit.Mvvm
{
    [DebuggerStepThrough]
    public class DataContext : IDataContext
    {
        private UIControlledApplication _uicApp;
        private IUIProvider _uiProvider;
        public DataContext(IUIProvider uiProvider)
        {
            //_uicApp = uicApp;
            _uiProvider = uiProvider;
        }

        public UIApplication GetUIApplication()
        {
            //return GetUIDocument().Application;
            var uiApp = _uiProvider.GetUIApplication();
            var method = uiApp.GetType().GetMethod("getUIApplication", BindingFlags.Instance | BindingFlags.NonPublic);
            return method?.Invoke(uiApp, null) as UIApplication;
        }

        public Document GetDocument()
        {
            //每次点击Command都需要新的对象，所以通过IOC获取新的实例，而不是传入Document构造实例
            //return SingletonIOC.Current.Container.GetInstance<Document>();
            return GetUIApplication().ActiveUIDocument.Document;
        }

        public UIDocument GetUIDocument()
        {
            return new UIDocument(GetDocument());
        }
    }
}
