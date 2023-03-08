using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Electrical;
using Autodesk.Revit.DB.Mechanical;
using Autodesk.Revit.DB.Plumbing;
using Autodesk.Revit.UI;
using IronMan.Revit.Entity.Attributes;
using IronMan.Revit.Toolkit.Extension;
using IronMan.Revit.Toolkit.Mvvm;
using NPOI.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IronMan.Revit.Commands.PushButtons
{
    [Transaction(TransactionMode.Manual)]
    public class SetParameterFilterColorCommand : IExternalCommand
    {
        [ButtonName("清除过滤器填充","清除前景填充","this is description")]
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            var document = commandData.Application.ActiveUIDocument.Document;
            var views = document.GetElements<View>().Where(x => x.Name.Contains("出图-"));
            if (views.Count() == 0)
            {
                return Result.Cancelled;
            }
            document.NewTransaction(() =>
            {
                //FilteredElementCollector fillFilter = new FilteredElementCollector(document);
                //fillFilter.OfClass(typeof(FillPatternElement));
                //var fillPatterns = document.GetElements<FillPatternElement>();
                //FillPatternElement fp = fillFilter.First(m => (m as FillPatternElement).GetFillPattern().IsSolidFill) as FillPatternElement;

                foreach (var v in views)
                {
                    //InitialMep(v, overridess);
                    ResetParameterFileterPattern(v);
                }

            }, "清除填充");
            return Result.Succeeded;
        }

        private void InitialMep(View view, OverrideGraphicSettings ogs)
        {
            var document = view.Document;

            var ductIds = document.GetElementsInView<Duct>(view.Id).Select(e => e.Id).ToList();
            var ductOtherIds = document.GetElementsInView<FamilyInstance>(view.Id).ToElements().Where(x => x.Category.Name.Contains("风管")).Select(x => x.Id);
            var pipiIds = document.GetElementsInView<Pipe>(view.Id).Select(e => e.Id).ToList();
            var pipeOtherIds = document.GetElementsInView<FamilyInstance>(view.Id).Where(x => x.Category.Name.Contains("管道")||x.Category.Name.Contains("管件")).Select(x => x.Id);
            var cableTrayIds = document.GetElementsInView<CableTray>(view.Id).Select(e => e.Id).ToList();
            var cableTrayOtherIds = document.GetElementsInView<FamilyInstance>(view.Id).Where(x => x.Category.Name.Contains("桥架")).Select(x => x.Id);
            var resultIds = ductIds
            .Union(ductOtherIds)
            .Union(pipiIds)
            .Union(pipeOtherIds)
            .Union(cableTrayIds)
            .Union(cableTrayOtherIds);
            foreach (var elementId in resultIds)
            {
                view.SetElementOverrides(elementId, ogs);
            }
        }

        private void ResetParameterFileterPattern(View view)
        {
            Document document = view.Document;
            var filterIds = view.GetFilters();
            foreach (var filterId in filterIds)
            {
                OverrideGraphicSettings ogs  = view.GetFilterOverrides(filterId);
                var newOgs = new OverrideGraphicSettings();
                newOgs.SetProjectionLinePatternId(ogs.ProjectionLinePatternId)
                    .SetProjectionLineColor(ogs.ProjectionLineColor)
                    .SetProjectionLineWeight(ogs.ProjectionLineWeight)
                    .SetSurfaceTransparency(ogs.Transparency);
                view.SetFilterOverrides(filterId, newOgs);
            }
        }
    }
}

