using Autodesk.Revit.DB.ExtensibleStorage;
using IronMan.Revit.Toolkit.Mvvm.Interfaces;
using IronMan.Revit.Toolkit.Mvvm.Service.Attributes;
using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace IronMan.Revit.Toolkit.Mvvm.Service.ExtensibleService
{
    public class DataStorage : IDataStorage
    {
        private readonly IUIProvider _uiProvider;

        public DataStorage(IUIProvider uiProvider)
        {
            _uiProvider = uiProvider;
        }

        public void ErasureSchema<T>()
        {

        }

        public Schema GetSchema<T>()
        {
            ExtensibleAttribute extensibleAttribute = (ExtensibleAttribute)typeof(T).GetCustomAttribute(typeof(ExtensibleAttribute));
            if (extensibleAttribute == null) return null;
            IDataInfo dataInfo = Activator.CreateInstance(extensibleAttribute.Profile) as IDataInfo;
            Schema schema = Schema.Lookup(dataInfo.SchemaUniqueId);
            return schema;
        }

        [DebuggerStepThrough]
        public Schema GetSchema(Type type)
        {
            ExtensibleAttribute extensibleAttribute = (ExtensibleAttribute)type.GetCustomAttribute(typeof(ExtensibleAttribute));
            if (extensibleAttribute == null) return null;
            IDataInfo dataInfo = Activator.CreateInstance(extensibleAttribute.Profile) as IDataInfo;
            Schema schema = Schema.Lookup(dataInfo.SchemaUniqueId);
            return schema;
        }

        public void Append<T>() where T : class
        {
            //通过反射获取IDataInfo的实现类
            ExtensibleAttribute extensibleAttribute = (ExtensibleAttribute)typeof(T).GetCustomAttribute(typeof(ExtensibleAttribute));
            if (extensibleAttribute == null) return;
            IDataInfo dataInfo = Activator.CreateInstance(extensibleAttribute.Profile) as IDataInfo;
            Schema schema = Schema.Lookup(dataInfo.SchemaUniqueId);
            if (schema != null) return;
            using (SchemaBuilder schemaBuilder = new SchemaBuilder(dataInfo.SchemaUniqueId))
            {
                if (!schemaBuilder.Ready()) return;
                schemaBuilder.SetSchemaName(dataInfo.Name)
                    .SetApplicationGUID(_uiProvider.GetAddInId().GetGUID())
                    .SetVendorId("StarkIndustries")
                    .SetDocumentation(dataInfo.Documentation)
                    .SetReadAccessLevel(dataInfo.ReadAccessLevel)
                    .SetWriteAccessLevel(dataInfo.WriteAccessLevel);
                //把含有ExtensibleMemberAttribute的属性添加到字段
                PropertyInfo[] properties = typeof(T).GetProperties();
                foreach (PropertyInfo property in properties)
                {
                    ExtensibleMemberAttribute propertyAttribute = property.GetCustomAttribute<ExtensibleMemberAttribute>();
                    if (propertyAttribute == null) continue;
                    Type propertyType = property.PropertyType;
                    if (SchemaStaticResources.IsAllowedType(propertyType))
                    {
                        schemaBuilder.AddSimpleField(property.Name, propertyType); 
                    }
                    else if (propertyType.IsGenericType)
                    {
                        int argCount = propertyType.GetGenericArguments().Count();
                        //一个值是列表IList
                        if (argCount == 1)
                        {
                            Type type = propertyType.GenericTypeArguments.FirstOrDefault();
                            if (type != null)
                            {
                                if (SchemaStaticResources.IsAllowedType(type))
                                {
                                    schemaBuilder.AddArrayField(property.Name, type);
                                }
                            }
                        }
                        //两个值是键值对IDictionary
                        if (argCount == 2)
                        {
                            Type keyType = propertyType.GenericTypeArguments[0];
                            Type valueType = propertyType.GenericTypeArguments[1];
                            if (SchemaStaticResources.IsAllowedType(keyType) && SchemaStaticResources.IsAllowedType(valueType))
                            {
                                schemaBuilder.AddMapField(property.Name, keyType, valueType);
                            }
                        }
                    }
                    else
                    {
                        return;
                    }
                }
                schema = schemaBuilder.Finish();
            }
        }


    }
}
