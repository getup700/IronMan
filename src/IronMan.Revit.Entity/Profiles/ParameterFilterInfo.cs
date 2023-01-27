using Autodesk.Revit.DB.ExtensibleStorage;
using IronMan.Revit.Entity.Contants;
using IronMan.Revit.Toolkit.Mvvm.Interfaces;
using IronMan.Revit.Toolkit.Mvvm.IOC;
using IronMan.Revit.Toolkit.Mvvm.Service.ExtensibleService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IronMan.Revit.Entity.Profiles
{
    [Obsolete]
    public class ParameterFilterInfo : ISchemaInfo
    {
        private readonly IUIProvider _uiProvider;

        public ParameterFilterInfo(IUIProvider uiProvider)
        {
            _uiProvider = uiProvider;
        }

        public Guid ApplicationUniqueId => _uiProvider.GetAddInId().GetGUID();

        public string VendorId => "ADSK";

        public string Documentation => "Parameter Filter";

        public AccessLevel ReadAccessLevel => AccessLevel.Vendor;

        public AccessLevel WriteAccessLevel => AccessLevel.Vendor;

        public string Name => "IronMan_ParameterFilter";

        public Guid SchemaUniqueId => ExtensibleStorage.ParameterFilterInfoGuid;

        public FieldProfile Profile => SingletonIOC.Current.Container.GetInstance<ParameterFilterProfile>();
    }
}
