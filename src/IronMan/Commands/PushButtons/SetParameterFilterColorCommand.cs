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
            var view = document.ActiveView;
            //var filters = view.GetFilters();
            //if (filters.Count == 0)
            //{
            //    return Result.Succeeded;
            //}
            document.NewTransaction(() =>
            {
                //foreach (var filterId in filters)
                //{
                //    OverrideGraphicSettings overrides = view.GetFilterOverrides(filterId);
                //    OverrideGraphicSettings ogs = new OverrideGraphicSettings();
                //    overrides.SetSurfaceForegroundPatternId(ogs.SurfaceForegroundPatternId);
                //    overrides.SetSurfaceForegroundPatternVisible(false);
                //    overrides.SetSurfaceForegroundPatternColor(ogs.SurfaceForegroundPatternColor)
                //    .SetProjectionLineWeight(ogs.ProjectionLineWeight)
                //    .SetProjectionLineColor(ogs.ProjectionLineColor)
                //    .SetSurfaceBackgroundPatternColor(ogs.SurfaceBackgroundPatternColor)
                //    .SetSurfaceBackgroundPatternVisible(false);

                //FilteredElementCollector fillFilter = new FilteredElementCollector(document);
                //fillFilter.OfClass(typeof(FillPatternElement));
                //var fillPatterns = document.GetElements<FillPatternElement>();
                //FillPatternElement fp = fillFilter.First(m => (m as FillPatternElement).GetFillPattern().IsSolidFill) as FillPatternElement;

                //ogs.SetSurfaceForegroundPatternId(fp.Id);
                //ogs.SetSurfaceForegroundPatternColor(Color.InvalidColorValue);
                //ogs.SetSurfaceTransparency(1);
                //if (overrides.ProjectionLineColor != null)
                //{
                //    ogs.SetProjectionLineColor(overrides.ProjectionLineColor);
                //}
                //if(overrides.SurfaceForegroundPatternColor!= null)
                //{
                //    ogs.SetSurfaceForegroundPatternColor(new Color(0,0,0));
                //}
                //if (overrides.Transparency != 0)
                //{
                //    ogs.SetSurfaceTransparency(overrides.Transparency);
                //}


                //}
                //var filterid = filters.First();
                var filterid = document.ActiveView.GetFilters().First();
                OverrideGraphicSettings overridess = view.GetFilterOverrides(filterid);
                OverrideGraphicSettings ogss = new OverrideGraphicSettings();
                overridess.SetSurfaceForegroundPatternId(ogss.SurfaceForegroundPatternId);
                overridess.SetSurfaceForegroundPatternVisible(false);
                overridess.SetSurfaceForegroundPatternColor(ogss.SurfaceForegroundPatternColor)
                .SetProjectionLineWeight(ogss.ProjectionLineWeight)
                .SetProjectionLineColor(ogss.ProjectionLineColor)
                .SetSurfaceBackgroundPatternColor(ogss.SurfaceBackgroundPatternColor)
                .SetSurfaceBackgroundPatternVisible(false);

                //var info = GetShareInfo(commandData.Application.Application);
                var views = document.GetElements<View>().Where(x => x.Name.Contains("出图-"));
                if (views.Count() == 0) return;
                foreach (var v in views)
                {
                    InitialMep(v, overridess);
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

        //获取共享参数
        private string GetShareInfo(Autodesk.Revit.ApplicationServices.Application revitApp)
        {
            StringBuilder str = new StringBuilder();
            DefinitionFile definitionFile = revitApp.OpenSharedParameterFile();
            DefinitionGroups groups = definitionFile.Groups;
            foreach (DefinitionGroup group in groups)
            {
                foreach (Definition definition in group.Definitions)
                {
                    string name = definition.Name;
                    ParameterType type = definition.ParameterType;
                    str.AppendLine(string.Format("{0}---{1}", name, type.ToString()));
                }
            }
            return str.ToString();

        }


    }
}

