using Autodesk.Revit.DB.ExtensibleStorage;
using IronMan.Revit.Toolkit.Mvvm.Service.ExtensibleService;
using NPOI.SS.UserModel.Charts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IronMan.Revit.Entity.Profiles
{
    public class ParameterFilterSetInfo : IDataInfo
    {
        public Guid SchemaUniqueId => Contants.ExtensibleStorage.ParameterFilterSetInfoGuid;

        public string Name => nameof(ParameterFilterSetInfo);

        public string Documentation => "this is a datastorage in revit";

        public AccessLevel ReadAccessLevel => AccessLevel.Application;

        public AccessLevel WriteAccessLevel => AccessLevel.Application;
    }
}
