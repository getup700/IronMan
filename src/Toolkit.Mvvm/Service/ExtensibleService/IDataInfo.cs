using Autodesk.Revit.DB.ExtensibleStorage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IronMan.Revit.Toolkit.Mvvm.Service.ExtensibleService
{
    public interface IDataInfo
    {
        Guid SchemaUniqueId { get; }

        string Name { get; }

        string Documentation { get; }

        AccessLevel ReadAccessLevel { get; }

        AccessLevel WriteAccessLevel { get; }
    }
}
