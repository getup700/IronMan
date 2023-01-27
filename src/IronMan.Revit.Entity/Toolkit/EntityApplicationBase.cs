using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Messaging;
using IronMan.Revit.Entity.Profiles;
using IronMan.Revit.Toolkit.Mvvm;
using IronMan.Revit.Toolkit.Mvvm.IOC;
using IronMan.Revit.Toolkit.Mvvm.Service.ExtensibleService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IronMan.Revit.Entity.Toolkit
{
    public class EntityApplicationBase : ApplicationBase
    {
        public override void RegisterSchema(IDataStorage dataStorage)
        {
            return;
        }

        public override void RegisterTyped(SimpleIoc container)
        {
            throw new Exception();
        }

        public override void RegisterField(SimpleIoc container)
        {
            Current.Register<SheetTitleProfile>();
            Current.Register<ISchemaInfo, SheetTitleSchemaInfo>();
            Current.Register<DecorationParameterProfile>();
        }
    }
}
