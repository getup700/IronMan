///************************************************************************************
///   Author:Tony Stark
///   CreateTime:2023/5/10 星期三 15:54:49
///   Mail:2609639898@qq.com
///   Github:https://github.com/getup700
///
///   Description:
///
///************************************************************************************

using Autodesk.Revit.DB;
using IronMan.Revit.Toolkit.Extension;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IronMan.Revit.Toolkit.Extension
{
    public static class FamilyExtension
    {

        /// <summary>
        /// Convert FamilyParameterSet to List<FamilyParameter>
        /// </summary>
        /// <param name="familyParameterSet"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static List<FamilyParameter> ToList(this FamilyParameterSet familyParameterSet, Predicate<FamilyParameter> predicate = null)
        {
            var familyParameters = new List<FamilyParameter>();
            foreach (var item in familyParameterSet)
            {
                var familyParameter = item as FamilyParameter;
                if (familyParameter == null)
                {
                    continue;
                }
                if (predicate != null && predicate(familyParameter) || predicate == null)
                {
                    familyParameters.Add(familyParameter);
                }
            }
            return familyParameters;
        }

        /// <summary>
        /// Convert ParameterSet to List<Parameter>
        /// </summary>
        /// <param name="parameterSet"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static List<Parameter> ToList(this ParameterSet parameterSet, Predicate<Parameter> predicate = null)
        {
            var parameters = new List<Parameter>();
            var iterator = parameterSet.ForwardIterator();
            while (iterator.MoveNext())
            {
                var parameter = iterator.Current as Parameter;
                if (parameter == null)
                {
                    continue;
                }
                if (predicate == null || predicate(parameter))
                {
                    parameters.Add(parameter);
                }
            }
            return parameters;
        }

        public static List<FamilyInstance> GetInstances(this Document document, string familyName, IEnumerable<FamilyInstance> source = null)
        {
            var result = new List<FamilyInstance>();
            if (source == null)
            {
                result = document.GetElements<FamilyInstance>(x => x.Symbol.Family.Name == familyName).ToList();
            }
            else
            {
                result = source.Where(x => x.Symbol.Family.Name == familyName).ToList();
            }
            return result;
        }

        public static BuiltInCategory GetBuiltInCategory(this Category category)
        {
#if RVT_23_DEBUG || RVT_23_RELEASE || RVT_24_DEBUG || RVT_24_RELEASE
            return category.BuiltInCategory;
#else
            return (BuiltInCategory)category.Id.IntegerValue;
#endif
        }
    }
}
