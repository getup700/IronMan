using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Toolkit.Extension
{
    public static class UnitExtension
    {
        public static double ConverToFeet(this double value)
        {
#if (RVT_2019 || RVT_2020||DEBUG20 )
            return UnitUtils.Convert(value, DisplayUnitType.DUT_MILLIMETERS, DisplayUnitType.DUT_DECIMAL_FEET);
#endif

#if (RVT_2021 == true)
            return UnitUtils.Convert(value, UnitTypeId.Millimeters, UnitTypeId.Feet);
#endif
#if (RVT_2022 == true)
            return UnitUtils.Convert(value, UnitTypeId.Millimeters, UnitTypeId.Feet);
#endif

        }
        public static double ConvertToMM(this double value)
        {
#if (RVT_2019 || RVT_2020 || DEBUG20)
            return UnitUtils.Convert(value, DisplayUnitType.DUT_DECIMAL_FEET, DisplayUnitType.DUT_MILLIMETERS);
#endif
#if RVT_2021
            return UnitUtils.Convert(value, UnitTypeId.Feet, UnitTypeId.Millimeters);
#endif
#if RVT_2022
            return UnitUtils.Convert(value, UnitTypeId.Feet, UnitTypeId.Millimeters);
#endif
        }
    }
}
