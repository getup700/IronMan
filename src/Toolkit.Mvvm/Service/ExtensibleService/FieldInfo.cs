using Autodesk.Revit.DB;
using Autodesk.Revit.DB.ExtensibleStorage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IronMan.Revit.Toolkit.Mvvm.Service.ExtensibleService
{
    /// <summary>
    /// 字段详细信息
    /// </summary>
    public class FieldInfo
    {
        private Type _type;

        public string Name { get; set; }

        public Type Type
        {
            get => _type;
            set
            {
                _type = value;
                //如果是entity
                if (_type == typeof(Entity))
                {
                    FieldType = FieldType.Entity;
                    return;
                }
                //如果是简单类型
                else if (SchemaStaticResources.IsAllowedType(_type))
                {
                    FieldType = FieldType.Simple;
                    return;
                }
                //如果是泛型
                else if (_type.IsGenericType)
                {
                    int argCount = _type.GetGenericArguments().Count();
                    //一个值是列表IList
                    if (argCount == 1)
                    {
                        Type type = _type.GenericTypeArguments.FirstOrDefault();
                        if (type != null)
                        {
                            if (SchemaStaticResources.IsAllowedType(type))
                            {
                                FieldType = FieldType.Array;
                                return;
                            }
                        }
                    }
                    //两个值是键值对IDictionary
                    if (argCount == 2)
                    {
                        Type keyType = _type.GenericTypeArguments[0];
                        Type valueType = _type.GenericTypeArguments[1];
                        if (SchemaStaticResources.IsAllowedType(keyType) && SchemaStaticResources.IsAllowedType(valueType))
                        {
                            FieldType = FieldType.Map;
                            return;
                        }
                    }
                }
                else
                {
                    FieldType = FieldType.Undefined;
                }
            }
        }

        public UnitType UnitType { get; set; }

        public FieldType FieldType { get; private set; }

        public string Documetation { get; set; } = string.Empty;

        public Guid SubschemaUniqueId { get; set; }
    }
}
