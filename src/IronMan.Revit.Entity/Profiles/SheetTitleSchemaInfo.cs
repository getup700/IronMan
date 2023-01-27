using Autodesk.Revit.DB.ExtensibleStorage;
using IronMan.Revit.Toolkit.Mvvm.Interfaces;
using IronMan.Revit.Toolkit.Mvvm.Service.ExtensibleService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IronMan.Revit.Entity.Profiles
{
    public class SheetTitleSchemaInfo : ISchemaInfo,IDataInfo
    {
        private readonly Guid _applicationUniqueId;

        public SheetTitleSchemaInfo(Guid applicationUniqueId)
        {
            _applicationUniqueId = applicationUniqueId;
        }

        public Guid ApplicationUniqueId => _applicationUniqueId;

        public string VendorId => "StarkIndustries";

        public string Documentation => "This is a schema used to record title element id";

        public AccessLevel ReadAccessLevel => AccessLevel.Application;

        public AccessLevel WriteAccessLevel =>AccessLevel.Application;

        public string Name => "SheetTitle";

        public Guid SchemaUniqueId => new Guid("E99ED400-B677-40E6-A279-65CD897B5D42");

        public FieldProfile Profile =>new SheetTitleProfile();
    }
}
