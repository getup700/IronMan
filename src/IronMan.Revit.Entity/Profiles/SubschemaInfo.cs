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
    public class SubschemaInfo : ISchemaInfo
    {
        private readonly  IUIProvider _uiProvider;
        private readonly Guid _schemaGuid = new Guid("1513504C-B755-4899-A98C-EE96F7F30D2C");

        public SubschemaInfo(IUIProvider uiProvider)
        {
            _uiProvider = uiProvider;
        }

        public Guid ApplicationUniqueId =>_uiProvider.GetAddInId().GetGUID();

        public Guid SchemaUniqueId => _schemaGuid;

        public string VendorId => "ADSK";

        public string Documentation => "This is a Test SubSchema data";

        public AccessLevel ReadAccessLevel => AccessLevel.Public;

        public AccessLevel WriteAccessLevel => AccessLevel.Public;

        public string Name => "subschemainfo";

        public FieldProfile Profile => new SubschemaProfile(_uiProvider);
    }
}
