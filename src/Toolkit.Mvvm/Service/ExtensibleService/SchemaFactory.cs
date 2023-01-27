using Autodesk.Revit.DB;
using Autodesk.Revit.DB.ExtensibleStorage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace IronMan.Revit.Toolkit.Mvvm.Service.ExtensibleService
{
    [Obsolete("obsolete class",false)]
    public static class SchemaFactory
    {
        /// <summary>
        /// 创建扩展存储
        /// </summary>
        /// <param name="schemaInfo"></param>
        /// <returns></returns>
        public static Schema CreateSchema(ISchemaInfo schemaInfo)
        {
            Schema schema = Schema.Lookup(schemaInfo.SchemaUniqueId);
            if(schema == null)
            {
                using(SchemaBuilder builder  = new SchemaBuilder(schemaInfo.SchemaUniqueId))
                {
                    //Step1：构造Schema信息
                    if (builder.Ready())
                    {
                        builder
                            .SetApplicationGUID(schemaInfo.ApplicationUniqueId)
                            .SetSchemaName(schemaInfo.Name)
                            .SetDocumentation(schemaInfo.Documentation)
                            .SetVendorId(schemaInfo.VendorId)
                            .SetReadAccessLevel(schemaInfo.ReadAccessLevel)
                            .SetWriteAccessLevel(schemaInfo.WriteAccessLevel);
                    };
                    //Step2：构造字段
                    SetFields(builder, schemaInfo.Profile);
                    schema = builder.Finish();
                }
            }
            return schema;
        }

        /// <summary>
        /// 简单工厂模式的核心
        /// 构造多种类型的字段
        /// </summary>
        /// <param name="builder">被构造字段</param>
        /// <param name="profile">存储自定义的字段信息</param>
        private static void SetFields(SchemaBuilder builder, FieldProfile profile)
        {
            foreach (FieldInfo info in profile.Fields)
            {
                //判断非法字符
                if(builder.AcceptableName(info.Name))
                {
                    switch (info.FieldType)
                    {
                        case FieldType.Undefined:
                            //write to log...
                            break;
                        case FieldType.Simple:
                            builder.AddSimpleField(info);
                            break;
                        case FieldType.Array:
                            builder.AddArrayField(info);
                            break;
                        case FieldType.Map:
                            builder.AddMapField(info);
                            break;
                        case FieldType.Entity:
                            builder.AddEntityField(info);
                            break;
                    }
                }
                else
                {
                    //write to log...
                }
            }
        }
        
    }
}
