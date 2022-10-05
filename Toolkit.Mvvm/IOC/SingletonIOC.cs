using GalaSoft.MvvmLight.Ioc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IronMan.Revit.Toolkit.Mvvm.IOC
{
    public class SingletonIOC
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

    }
}
