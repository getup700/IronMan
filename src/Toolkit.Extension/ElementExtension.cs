using Autodesk.Revit.DB;
using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IronMan.Revit.Toolkit.Extension
{
    public static class ElementExtension
    {
        public static List<Element> GetIntersectElements(this Element element, BuiltInCategory category, bool contain = false)
        {
            List<Element> result = new List<Element>();
            Document document = element.Document;
            BoundingBoxXYZ boundingBoxXYZ = element.get_BoundingBox(document.ActiveView);
            Outline outline = new Outline(boundingBoxXYZ.Min, boundingBoxXYZ.Max);

            BoundingBoxIntersectsFilter intersectsFilter = new BoundingBoxIntersectsFilter(outline);
            ElementCategoryFilter elementFilter = new ElementCategoryFilter(category);
            result = new FilteredElementCollector(document)
                .WherePasses(intersectsFilter)
                .WherePasses(elementFilter)
                .WhereElementIsNotElementType().ToList();
            return result;
        }
        public static Parameter LookupParameter(this Element element, ElementId parameterId)
        {
            foreach (Parameter parameter in element.Parameters)
            {
                if (parameter.Id == parameterId)
                {
                    return parameter;
                }
            }
            return null;
        }

        public static void InitialOverrideInView(this Element element, View view)
        {
            OverrideGraphicSettings ogs = new OverrideGraphicSettings();
            view.SetElementOverrides(element.Id, ogs);
        }
    }
}
