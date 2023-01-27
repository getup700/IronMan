using GalaSoft.MvvmLight.Ioc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IronMan.Revit.Toolkit.Mvvm.IOC
{
    public interface IServiceLocate
    {
        SimpleIoc Locator { get; }
    }
}
