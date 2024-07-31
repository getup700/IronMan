using GalaSoft.MvvmLight.Ioc;
using IronMan.Revit.Toolkit.Mvvm.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;

namespace IronMan.Revit.Toolkit.Mvvm.IOC
{
    public class SingletonIOC : IServiceLocate
    {
        public static SingletonIOC Current = new SingletonIOC();
        private SimpleIoc _container;
        public SimpleIoc Container
        {
            get
            {
                if (_container == null)
                {
                    _container = new SimpleIoc();
                }
                return _container;
            }
        }

        public SimpleIoc Locator => Container;
    }
}
