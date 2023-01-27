using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IronMan.Revit.Entity.Contants
{
    public static class ExtensibleStorage
    {
        public static Guid ParameterFilterInfoGuid => new Guid("84B84080-16A3-4A8E-B947-7544CA5C5958");

        public static Guid SampleInfoGuid => new Guid("4B38084E-1499-488D-B6C7-2349B704294A");

        public static Guid SubschemaInfoGuid => new Guid("1513504C-B755-4899-A98C-EE96F7F30D2C");

        public static Guid ParameterFilterElementInfoGuid => new Guid("A25DF785-F667-4238-A904-AFE6AD400593");

        public static Guid ParameterFilterSetInfoGuid => new Guid("BE02C44B-6101-4DA2-AAE6-CFBBB34112FD");
    }
}
