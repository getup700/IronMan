using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IronMan.Revit.Contants
{
    public static class IronManID
    {
        private static string ClientId => "104AE7B9-0A9C-4875-B67D-48B2020DE40B";

        public static Guid ApplicationId => new Guid(ClientId);

        public static string VendorId => "StarkIndustries";

        public static AddInId IronMan => new AddInId(new Guid(ClientId));

        public static string ProductName => "IronMan";

        public static string DataStorageName => ProductName;


        #region DMU
        private static Guid GetModelLineDMU => new Guid("4FFBF942-68C5-425D-B65B-3422B759A5F3");

        public static UpdaterId GetModelLineUpdaterId => new UpdaterId(IronMan, GetModelLineDMU);

        #endregion

    }
}
