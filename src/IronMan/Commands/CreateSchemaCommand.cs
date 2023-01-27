using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.ExtensibleStorage;
using Autodesk.Revit.UI;
using IronMan.Revit.Toolkit.Mvvm.Service.ExtensibleService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using IronMan.Revit.Toolkit.Extension;
using RvtEntity = Autodesk.Revit.DB.ExtensibleStorage.Entity;
using DataStorage = Autodesk.Revit.DB.ExtensibleStorage.DataStorage;
using IronMan.Revit.Entity.Profiles;

namespace IronMan.Revit.Commands
{
    [Transaction(TransactionMode.Manual)]
    [Journaling(JournalingMode.NoCommandData)]
    [Regeneration(RegenerationOption.Manual)]
    public class CreateSchemaCommand : IExternalCommand
    {
        private readonly string SchemaGUID = "36BEB4F9-71B5-46F6-A214-A4890DF3B9A9";
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uiDoc = commandData.Application.ActiveUIDocument;
            Document doc = uiDoc.Document;

            #region Erase Data
            doc.NewTransaction(() =>
            {
                foreach (var item in Schema.ListSchemas())
                {
                    Schema.EraseSchemaAndAllEntities(item, true);
                }
            }, "Erase");

            #endregion

            //#region Create extension storage
            //Schema schema = SchemaFactory.CreateSchema(new SampleInfo(commandData.Application.ActiveAddInId.GetGUID()));
            //Schema subSchema = SchemaFactory.CreateSchema(new SubschemaInfo(commandData.Application.ActiveAddInId.GetGUID()));
            ////try
            ////{
            ////    using (SchemaBuilder schemaBuilder = new SchemaBuilder(new Guid(SchemaGUID)))
            ////    {
            ////        //define construction information
            ////        schemaBuilder
            ////            .SetApplicationGUID(commandData.Application.ActiveAddInId.GetGUID())
            ////            .SetVendorId("ADSK")
            ////            .SetSchemaName("IronMan")
            ////            .SetDocumentation("this is a test schema data")
            ////            .SetReadAccessLevel(AccessLevel.Public)
            ////            .SetWriteAccessLevel(AccessLevel.Public);
            ////        //define fields
            ////        //三种添加字段的方式：添加简单数据类型，列表类型，键值对类型；三种字段加起来不能超过256个

            ////        //字段类型一：添加简单数据类型
            ////        schemaBuilder.AddSimpleField("byte", typeof(byte));
            ////        schemaBuilder.AddSimpleField("int", typeof(int));
            ////        schemaBuilder.AddSimpleField("short", typeof(short));
            ////        var fieldBuilder = schemaBuilder.AddSimpleField("double", typeof(double));
            ////        //UnitType的设置是可选的，设置后其要和DisplayUnitType对应
            ////        fieldBuilder.SetUnitType(UnitType.UT_Length);
            ////        schemaBuilder.AddSimpleField("float", typeof(float)).SetUnitType(UnitType.UT_Number);
            ////        schemaBuilder.AddSimpleField("bool", typeof(bool));
            ////        schemaBuilder.AddSimpleField("string", typeof(string));//string不能>16mb，图片转为字段
            ////        schemaBuilder.AddSimpleField("guid", typeof(Guid));
            ////        schemaBuilder.AddSimpleField("element", typeof(ElementId));
            ////        schemaBuilder.AddSimpleField("xyz", typeof(XYZ)).SetUnitType(UnitType.UT_Number);
            ////        schemaBuilder.AddSimpleField("uv", typeof(UV)).SetUnitType(UnitType.UT_Number);
            ////        //custom data type
            ////        //添加复杂类型entity必须指向schema的guid
            ////        schemaBuilder.AddSimpleField("entity", typeof(RvtEntity)).SetSubSchemaGUID(new Guid(SchemaGUID));

            ////        //字段类型二：添加列表类型（必须是IList）
            ////        schemaBuilder.AddArrayField("stringList", typeof(string));
            ////        //字段类型三：添加键值对类型（必须是IDictionary<TKey,TValue>）
            ////        schemaBuilder.AddMapField("keyValueList", typeof(int), typeof(string));

            ////        //construct finish
            ////        schema = schemaBuilder.Finish();
            ////    }
            ////}
            ////catch (Exception e)
            ////{
            ////    MessageBox.Show($"{e.StackTrace + e.Message}");
            ////}

            //#endregion

            //#region Attach data

            //var reference = uiDoc.Selection.PickObject(Autodesk.Revit.UI.Selection.ObjectType.Element);
            //var element = uiDoc.Document.GetElement(reference);
            //doc.NewTransaction(() =>
            //{
            //    //data hook on a element
            //    RvtEntity entity = new RvtEntity(schema);
            //    //entity.Set(nameof(SampleProfile.TestDemo), new RvtEntity(subSchema));


            //    //var field = schema.GetField("float");
            //    //entity.Set<double>("double", 12, DisplayUnitType.DUT_MILLIMETERS);
            //    //entity.Set<float>(field, 12, DisplayUnitType.DUT_GENERAL);
            //    //entity.Set("entity", entity);
            //    //挂接类型一
            //    element.SetEntity(entity);

            //    //挂接类型二
            //    //data hook on a document while no need to point to elements
            //    //Autodesk.Revit.DB.ExtensibleStorage.DataStorage dataStorage = DataStorage.Create(doc);
            //    //dataStorage.SetEntity(entity);
            //}, "Attach Data");
            ////RvtEntity entityGet = element.GetEntity(schema);
            ////entityGet.Set<int>("Code",50,DisplayUnitType.DUT_DECIMAL_FEET);
            ////var value = entityGet.Get<int>("Code");
            //////var value = entityGet.Get<float>("float", DisplayUnitType.DUT_GENERAL);
            ////MessageBox.Show($"{value}");
            //#endregion

            #region GetSchemaDataStorage
            //ExtensibleStorageFilter extensibleStorageFilter = new ExtensibleStorageFilter(schema.GUID);
            //FilteredElementCollector collector = new FilteredElementCollector(doc);
            //var instance = collector.OfClass(typeof(FamilyInstance)).WherePasses(extensibleStorageFilter).ToElements().FirstOrDefault();

            //var entityGet2 = element.GetEntity(schema);
            //var value2 = entityGet.Get<string>("float", DisplayUnitType.DUT_GENERAL);

            //MessageBox.Show($"{value2.ToString()}");
            #endregion

            return Result.Succeeded;
        }
    }
}
