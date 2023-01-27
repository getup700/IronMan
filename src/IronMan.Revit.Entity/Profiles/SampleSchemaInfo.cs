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
    public class SampleSchemaInfo : ISchemaInfo
    {
        private readonly IUIProvider _uiProvider;
        private readonly Guid schemaGuid = new Guid("4B38084E-1499-488D-B6C7-2349B704294A");

        public SampleSchemaInfo(IUIProvider uiProvider)
        {
            _uiProvider = uiProvider;
        }

        public Guid ApplicationUniqueId =>_uiProvider.GetAddInId().GetGUID();

        public Guid SchemaUniqueId => schemaGuid;

        public string VendorId => "ADSK";

        public string Documentation => "This is a Test Schema data";

        public AccessLevel ReadAccessLevel => AccessLevel.Public;

        public AccessLevel WriteAccessLevel => AccessLevel.Public;

        public string Name => "IronMan";

        public FieldProfile Profile => new SampleProfile(_uiProvider);

    }
}
