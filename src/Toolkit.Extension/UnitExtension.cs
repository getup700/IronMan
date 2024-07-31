using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IronMan.Revit.Toolkit.Extension
{
    public static class UnitExtension
    {
        /// <summary>
        /// 毫米转为英尺
        /// </summary>
        /// <param name="value">公制单位-毫米</param>
        /// <returns></returns>
        public static double ConvertToFeet(this double value)
        {
#if (RVT_2019 || RVT_2020||DEBUG20||DEBUG19||DEBUG18 )
            return UnitUtils.Convert(value, DisplayUnitType.DUT_MILLIMETERS, DisplayUnitType.DUT_DECIMAL_FEET);
#endif

#if (RVT_2021 == true)
            return UnitUtils.Convert(value, UnitTypeId.Millimeters, UnitTypeId.Feet);
#endif
#if (RVT_2022 == true)
            return UnitUtils.Convert(value, UnitTypeId.Millimeters, UnitTypeId.Feet);
#endif

        }

        /// <summary>
        /// 英尺转为毫米
        /// </summary>
        /// <param name="value">公制单位-毫米</param>
        /// <returns></returns>
        public static double ConvertToMilliMeters(this double value)
        {
#if (RVT_2019 || RVT_2020 || DEBUG20||DEBUG19||DEBUG18)
            return UnitUtils.Convert(value, DisplayUnitType.DUT_DECIMAL_FEET, DisplayUnitType.DUT_MILLIMETERS);
#endif
#if RVT_2021
            return UnitUtils.Convert(value, UnitTypeId.Feet, UnitTypeId.Millimeters);
#endif
#if RVT_2022
            return UnitUtils.Convert(value, UnitTypeId.Feet, UnitTypeId.Millimeters);
#endif
        }

        /// <summary>
        /// 平方英尺转为平方米
        /// </summary>
        /// <param name="value">英制面积-平方英尺</param>
        /// <returns></returns>
        public static double ConverToSquareMeters(this double value)
        {
            return UnitUtils.Convert(value, DisplayUnitType.DUT_SQUARE_FEET, DisplayUnitType.DUT_SQUARE_METERS);
        }

        /// <summary>
        /// 平方米转为平方英尺
        /// </summary>
        /// <param name="value">公制面积-平方米</param>
        /// <returns></returns>
        public static double ConverToSquareFeet(this double value)
        {
            return UnitUtils.Convert(value, DisplayUnitType.DUT_SQUARE_METERS, DisplayUnitType.DUT_SQUARE_FEET);
        }

        /// <summary>
        /// 立方英尺转为立方米
        /// </summary>
        /// <param name="value">英制体积-立方英尺</param>
        /// <returns></returns>
        public static double ConvetToCubicMeters(this double value)
        {
            return UnitUtils.Convert(value, DisplayUnitType.DUT_CUBIC_FEET, DisplayUnitType.DUT_CUBIC_METERS);
        }

        /// <summary>
        /// 立方米转为立方英尺
        /// </summary>
        /// <param name="value">公制体积-立方米</param>
        /// <returns></returns>
        public static double ConvertCubicFeet(this double value)
        {
            return UnitUtils.Convert(value, DisplayUnitType.DUT_CUBIC_METERS, DisplayUnitType.DUT_CUBIC_FEET);
        }
    }
}
