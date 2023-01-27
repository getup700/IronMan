using Autodesk.Revit.DB;
using IronMan.Revit.DTO;
using IronMan.Revit.Entity;
using IronMan.Revit.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IronMan.Revit.IServices
{
    public interface IParameterFilterService:IDataProvider<ParameterFilterProxy>,IDataCleanup<ParameterFilterProxy>,IDataCreator<ParameterFilterProxy,DTO_Filter>
    {
        IEnumerable<ParameterFilterProxy> GetElementsInView(ElementId viewId, Func<ParameterFilterProxy, bool> predicate = null);

        IEnumerable<ParameterFilterProxy> FindByFilterOr(IEnumerable<ParameterFilterProxy> filters, DTO_Filter dto);

        IEnumerable<ParameterFilterProxy> FindByFilterWith(IEnumerable<ParameterFilterProxy> filters, DTO_Filter dto);

        void AddFiltersToView(IEnumerable<ParameterFilterProxy> filters, View view, Action<ParameterFilterProxy> action = null);

        void RemoveFiltersFromView(IEnumerable<ParameterFilterProxy> filters, View view);

        void ReplaceApplyFiltersInView(IEnumerable<ParameterFilterProxy> filters, View view);

        void ReplaceVisivilityFiltersInView(IEnumerable<ParameterFilterProxy>filters,View view);

        void ReplaceColorFilterInView(IEnumerable<ParameterFilterProxy> filters, View view);

        void SetFiltersVisibility(IEnumerable<ParameterFilterProxy> filters, View view);

        void OverrideColor(ParameterFilterProxy filter, View view, Color color);
    }
}
