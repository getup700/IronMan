using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace IronMan.Revit.Toolkit.Mvvm.Service.ExtensibleService
{
    [Obsolete("obsolete class",false)]
    /// <summary>
    /// 字段概述基类
    /// </summary>
    public class FieldProfile
    {
        /// <summary>
        /// 创建简单类型字段
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <param name="documetation"></param>
        /// <param name="unitType"></param>
        /// <returns></returns>
        protected FieldInfo CreateInfo<T>([CallerMemberName] string name = null, string documetation = null, UnitType unitType = UnitType.UT_Undefined)
        {
            return CreateInfo(name, typeof(T), documetation, unitType);
        }

        /// <summary>
        /// 创建Entity字段
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="schemaInfo"></param>
        /// <param name="name"></param>
        /// <param name="documetation"></param>
        /// <param name="unitType"></param>
        /// <returns></returns>
        protected FieldInfo CreateInfo<T>(ISchemaInfo schemaInfo, [CallerMemberName] string name = null, string documetation = null, UnitType unitType = UnitType.UT_Undefined)
        {
            FieldInfo fieldInfo = CreateInfo(name, typeof(T), documetation, unitType);
            fieldInfo.SubschemaUniqueId = schemaInfo.SchemaUniqueId;
            return fieldInfo;
        }

        private FieldInfo CreateInfo(string name, Type type, string documetation, UnitType unitType)
        {
            FieldInfo fieldInfo = new FieldInfo()
            {
                Name = name,
                Type = type,
                UnitType = unitType,
                Documetation = documetation,
            };
            return fieldInfo;
        }

        public List<FieldInfo> Fields { get; set; } = new List<FieldInfo>();

    }
}
