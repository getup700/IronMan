using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IronMan.Revit.Toolkit.Extension
{
    public static class ParameterFilterRuleFactoryExtension
    {
        public static FilterRule CreateEqualsRule(ElementId id, string name, bool caseSensitive = false) =>
            ParameterFilterRuleFactory.CreateEqualsRule(id, name, caseSensitive);
    }
}
