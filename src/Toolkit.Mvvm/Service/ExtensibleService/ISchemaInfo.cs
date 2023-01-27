using Autodesk.Revit.DB;
using Autodesk.Revit.DB.ExtensibleStorage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IronMan.Revit.Toolkit.Mvvm.Service.ExtensibleService
{
    /// <summary>
    /// 创建Schema的必要信息
    /// </summary>
    public interface ISchemaInfo
    {
        Guid ApplicationUniqueId { get; }

        string VendorId { get; }

        string Documentation { get; }

        AccessLevel ReadAccessLevel { get; }

        AccessLevel WriteAccessLevel { get; }

        string Name { get; }

        Guid SchemaUniqueId { get; }

        FieldProfile Profile { get; }

    }
}
