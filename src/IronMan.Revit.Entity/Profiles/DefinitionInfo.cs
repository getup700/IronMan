using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IronMan.Revit.Entity.Profiles
{
    public class DefinitionInfo:IDefinition
    {
        public string Name { get; set; }

        public BuiltInParameterGroup ParameterGroup { get; set; }

        public ParameterType ParameterType { get; set; }

        public UnitType UnitType { get; set; }

        public string Value { get; set; }

        public DefinitionInfo()
        {

        }

        public DefinitionInfo(string name, BuiltInParameterGroup parameterGroup, ParameterType parameterType, UnitType unitType)
        {
            Name = name;
            ParameterGroup = parameterGroup;
            ParameterType = parameterType;
            UnitType = unitType;
        }

        public bool Equal(Definition definition)
        {
            if (definition == null) return false;
            if (definition.Name != Name) return false;
            if (definition.ParameterGroup != ParameterGroup) return false;
            if (definition.ParameterType != ParameterType) return false;
            if (definition.UnitType != UnitType) return false;
            return true;
        }
    }
}
