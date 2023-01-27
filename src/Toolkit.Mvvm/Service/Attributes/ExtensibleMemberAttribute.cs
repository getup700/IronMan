using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IronMan.Revit.Toolkit.Mvvm.Service.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class ExtensibleMemberAttribute : Attribute
    {
        public ExtensibleMemberAttribute()
        {

        }

        public ExtensibleMemberAttribute(UnitType unitType,DisplayUnitType displayUnitType)
        {
            UnitType = unitType;
            DisplayUnitType = displayUnitType;
        }

        public UnitType UnitType { get; set; } = UnitType.UT_Undefined;
        public DisplayUnitType DisplayUnitType { get; set; } = DisplayUnitType.DUT_UNDEFINED;
    }
}
