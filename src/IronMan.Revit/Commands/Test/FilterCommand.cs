using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using IronMan.Revit.Toolkit.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IronMan.Revit.Commands.Test
{
    [Transaction(TransactionMode.Manual)]
    internal class FilterCommand : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            //OfCategory过滤结果为族实例与族类别。OfClass(typeof(WallType))过滤墙类别
            //OfClass过滤结果是什么？过滤结果由Type决定。
            Document document = commandData.Application.ActiveUIDocument.Document;

            //ofclass层叠墙会被统计
            FilteredElementCollector ofClass = new FilteredElementCollector(document).OfClass(typeof(FamilyInstance));
            //层叠墙不是新的墙体类型，不会被统计
            FilteredElementCollector ofCategory = new FilteredElementCollector(document).OfCategory(BuiltInCategory.OST_Walls);
            FilteredElementCollector wallInstances = document.GetElementInstances(BuiltInCategory.OST_Walls);
            var wallssss = document.GetElements<Wall>();
            var wallTypes = document.GetElements<WallType>();
            var doorsss = document.GetElements<FamilyInstance>();
            var doorTypes = document.GetElements<FamilySymbol>();
            var families = document.GetElements<Family>(x => x.FamilyCategory.Name == "门");
            //Type:FamilySymbol 4
            //door
            FilteredElementCollector initial = new FilteredElementCollector(document).OfClass(typeof(FamilyInstance));
            FilteredElementCollector inieeee = new FilteredElementCollector(document).OfClass(typeof(FamilyInstance));
            //Type:FamilyInstance 4
            //door,door,column
            var docExtension = document.GetElements<FamilyInstance>();


            //Type:FamilySymbol 4
            //door
            FilteredElementCollector instances = initial.WherePasses(new ElementIsElementTypeFilter(false));

            //Type:null
            //FilteredElementCollector elementType = initial.WhereElementIsElementType();

            //Type:FamilyInstance 2
            FilteredElementCollector doors = document.GetElementInstances(BuiltInCategory.OST_Doors).WhereElementIsNotElementType();

            FilteredElementCollector result = instances.UnionWith(doors);//并集。IntersectWith交集

            string instanceInfo = string.Empty;
            foreach (var item in families)
            {
                instanceInfo += $"\t\t{item.Id}\t\t{item.Name}\n";
            }
            TaskDialog.Show("Title", instanceInfo);

            return Result.Succeeded;
        }
    }
}
