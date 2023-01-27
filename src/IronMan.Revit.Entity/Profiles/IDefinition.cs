using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IronMan.Revit.Entity.Profiles
{
    public interface IDefinition
    {
        public string Name { get; set; }

        public BuiltInParameterGroup ParameterGroup { get; set; }

        public ParameterType ParameterType { get; set; }

        public UnitType UnitType { get; set; }
    }
}
