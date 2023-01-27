using Autodesk.Revit.DB;
using Autodesk.Revit.DB.ExtensibleStorage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IronMan.Revit.Toolkit.Mvvm.Service.ExtensibleService
{
    [Obsolete("obsolete class",true)]
    public static class FieldFactory
    {
        public static void AddSimpleField(this SchemaBuilder schemaBuilder, FieldInfo info)
        {
            FieldBuilder fieldBuilder = schemaBuilder.AddSimpleField(info.Name, info.Type);
            if (fieldBuilder.Ready())
            {
                fieldBuilder.ConfigureUnitType(info.UnitType)
                    .ConfigureDocumetation(info.Documetation);
            }
        }

        public static void AddEntityField(this SchemaBuilder schemaBuilder, FieldInfo info)
        {
            FieldBuilder fieldBuilder = schemaBuilder.AddSimpleField(info.Name, info.Type);
            if (fieldBuilder.Ready())
            {
                fieldBuilder.ConfigureUnitType(info.UnitType)
                    .ConfigureDocumetation(info.Documetation)
                    .SetSubSchemaGUID(info.SubschemaUniqueId);
            }
        }

        public static void AddArrayField(this SchemaBuilder schemaBuilder, FieldInfo info)
        {
            FieldBuilder fieldBuilder = schemaBuilder.AddArrayField(info.Name, info.Type.GenericTypeArguments.FirstOrDefault());
            if (fieldBuilder.Ready())
            {
                fieldBuilder.ConfigureUnitType(info.UnitType)
                    .ConfigureDocumetation(info.Documetation);
            }
        }

        public static void AddMapField(this SchemaBuilder schemaBuilder, FieldInfo info)
        {
            FieldBuilder fieldBuilder = schemaBuilder.AddMapField(info.Name, info.Type.GenericTypeArguments[0], info.Type.GenericTypeArguments[1]);
            if (fieldBuilder.Ready())
            {
                fieldBuilder.ConfigureUnitType(info.UnitType)
                    .ConfigureDocumetation(info.Documetation);
            }
        }

        private static FieldBuilder ConfigureDocumetation(this FieldBuilder fieldBuilder, string documetation)
        {
            if (!string.IsNullOrEmpty(documetation))
            {
                fieldBuilder.SetDocumentation(documetation);
            }
            return fieldBuilder;
        }

        private static FieldBuilder ConfigureUnitType(this FieldBuilder fieldBuilder, UnitType unitType)
        {
            if (unitType != UnitType.UT_Undefined)
            {
                fieldBuilder.SetUnitType(unitType);
            }
            else
            {
                if (fieldBuilder.NeedsUnits())
                {
                    fieldBuilder.SetUnitType(UnitType.UT_Number);
                }
            }
            return fieldBuilder;
        }
    }
}
