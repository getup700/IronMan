using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace IronMan.Revit.Entity.Profiles
{
    public class DefinitionProfile
    {
        protected DefinitionInfo CreateParameter([CallerMemberName] string name = null,
            BuiltInParameterGroup parameterGroup = BuiltInParameterGroup.INVALID, 
            ParameterType parameterType = ParameterType.Invalid,
            UnitType unitType = UnitType.UT_Number)
        {
            DefinitionInfo definitonInfo = new DefinitionInfo()
            {
                Name = name,
                ParameterGroup = parameterGroup,
                ParameterType = parameterType,
                UnitType = unitType
            };
            return definitonInfo;
        }

        public List<DefinitionInfo> Parameters { get; set;} = new List<DefinitionInfo>();

    }
}
