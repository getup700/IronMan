using Autodesk.Revit.DB.ExtensibleStorage;
using IronMan.Revit.Entity.Contants;
using IronMan.Revit.Toolkit.Mvvm.Service.ExtensibleService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IronMan.Revit.Entity.Profiles
{
    public class ParameterFilterElementInfo : IDataInfo
    {
        public Guid SchemaUniqueId => ExtensibleStorage.ParameterFilterElementInfoGuid;

        public string Name => nameof(ParameterFilterElementInfo);

        public string Documentation => "this is Revit filter extensible schema data";

        public AccessLevel ReadAccessLevel => AccessLevel.Application;

        public AccessLevel WriteAccessLevel => AccessLevel.Application;
    }
}
