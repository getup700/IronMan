using Autodesk.Revit.DB;
using IronMan.Revit.DTO;
using IronMan.Revit.Entity;
using IronMan.Revit.Interfaces;
using IronMan.Revit.IServices;
using IronMan.Revit.Toolkit.Extension;
using IronMan.Revit.Toolkit.Mvvm.Interfaces;
using IronMan.Revit.Toolkit.Mvvm.Service.ExtensibleService;
using NPOI.OpenXmlFormats.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static NPOI.HSSF.Util.HSSFColor;

namespace IronMan.Revit.Services
{
    public class ParameterFilterService : IParameterFilterService, IDataCleanup<ParameterFilterProxy>, IDataCreator<ParameterFilterProxy, DTO_Filter>, IDataProvider<ParameterFilterProxy>
    {
        private readonly IDataContext _dataContext;
        private readonly IDataStorage _dataStorage;

        public ParameterFilterService(IDataContext dataContext, IDataStorage dataStorage)
        {
            _dataContext = dataContext;
            _dataStorage = dataStorage;
        }

        public ParameterFilterProxy CreateElement(DTO_Filter dto)
        {
            throw new NotImplementedException();
        }

        [Obsolete("", true)]
        public IEnumerable<ParameterFilterProxy> CreateElements(DTO_Filter dto)
        {
            throw new NotImplementedException();
        }

        public void DeleteElement(ParameterFilterProxy element)
        {
            _dataContext.GetDocument().NewTransaction(() =>
            {
                _dataContext.GetDocument().Delete(element.Id);
            });
        }

        public void DeleteElements(IEnumerable<ParameterFilterProxy> elements)
        {
            _dataContext.GetDocument().NewTransaction(() =>
            {
                foreach (var filter in elements)
                {
                    _dataContext.GetDocument().Delete(filter.Id);
                }
            });
        }

        public IEnumerable<ParameterFilterProxy> GetElements(Func<ParameterFilterProxy, bool> predicate = null)
        {
            IEnumerable<ParameterFilterElement> filters = this._dataContext.GetDocument().GetElements<ParameterFilterElement>();
            IEnumerable<ParameterFilterProxy> result = filters.ToList().ConvertAll<ParameterFilterProxy>(x => new ParameterFilterProxy(x));
            if (predicate != null)
            {
                result = result.Where(predicate);
            }
            return result;
        }

        public IEnumerable<ParameterFilterProxy> GetElementsInView(ElementId viewId, Func<ParameterFilterProxy, bool> predicate = null)
        {
            IEnumerable<ParameterFilterElement> filters = this._dataContext.GetDocument().GetElementsInView<ParameterFilterElement>(viewId).Cast<ParameterFilterElement>();
            IEnumerable<ParameterFilterProxy> result = filters.ToList().ConvertAll<ParameterFilterProxy>(x => new ParameterFilterProxy(x));
            if (predicate != null)
            {
                result = result.Where(predicate);
            }
            return result;
        }


        public IEnumerable<ParameterFilterProxy> FindByFilterOr(IEnumerable<ParameterFilterProxy> filters, DTO_Filter dto)
        {
            if (filters.Count() == 0) return null;
            if (dto == null || dto.Labels.Count == 0) return filters;
            var result = new List<ParameterFilterProxy>();
            foreach (var filter in filters)
            {
                foreach (var label in dto.Labels)
                {
                    if (filter.Name.Contains(label))
                    {
                        result.Add(filter);
                        break;
                    }
                }
            }
            return result;

        }

        public IEnumerable<ParameterFilterProxy> FindByFilterWith(IEnumerable<ParameterFilterProxy> filters, DTO_Filter dto)
        {
            if (filters.Count() == 0) return null;
            if (dto.Labels.Count == 0) return filters;
            var result = new List<ParameterFilterProxy>();
            foreach (var filter in filters)
            {
                bool flag = true;
                foreach (var label in dto.Labels)
                {
                    //如果有一个标签过滤器名称不包含
                    if (!filter.Name.Contains(label))
                    {
                        flag = false;
                    }
                }
                if (flag)
                {
                    result.Add(filter);
                }
            }
            return result;
        }
        public void AddFiltersToView(IEnumerable<ParameterFilterProxy> filters, View view, Action<ParameterFilterProxy> action = null)
        {
            this._dataContext.GetDocument().NewTransaction(delegate
            {
                foreach (ParameterFilterProxy item in filters)
                {
                    bool flag = view.IsFilterApplied(item.Id);
                    if (!flag)
                    {
                        view.AddFilter(item.Id);
                        Action<ParameterFilterProxy> action2 = action;
                        if (action2 != null)
                        {
                            action2(item);
                        }
                    }
                }
            });
        }

        public void RemoveFiltersFromView(IEnumerable<ParameterFilterProxy> filters, View view)
        {
            if (filters.Count() == 0) return;
            this._dataContext.GetDocument().NewTransaction(() =>
            {
                foreach (var filter in filters)
                {
                    bool flag = view.IsFilterApplied(filter.Id);
                    if (flag)
                    {
                        view.RemoveFilter(filter.Id);
                    }
                }
            });
        }

        public void ReplaceApplyFiltersInView(IEnumerable<ParameterFilterProxy> filters, View view)
        {
            if (filters.Count() == 0) return;
            _dataContext.GetDocument().NewTransaction(() =>
            {
                foreach (var filter in filters)
                {
                    if (filter.IsApplied)
                    {
                        if (!view.IsFilterApplied(filter.Id))
                        {
                            view.AddFilter(filter.Id);
                        }
                    }
                    else
                    {
                        if (view.IsFilterApplied(filter.Id))
                        {
                            //filter.SetVisible(false);
                            view.RemoveFilter(filter.Id);
                            //filter.SetApply(false);
                        }
                    }
                }
            });
            //_dataContext.GetDocument().Regenerate();
        }

        public void ReplaceVisivilityFiltersInView(IEnumerable<ParameterFilterProxy> filters, View view)
        {
            if (filters.Count() == 0) return;
            _dataContext.GetDocument().NewTransaction(() =>
            {
                foreach (var filter in filters)
                {
                    if (filter.IsApplied && filter.IsVisible != view.GetFilterVisibility(filter.Id))
                    {
                        filter.SetVisible(filter.IsVisible);
                    }
                }
            });
        }

        public void ReplaceColorFilterInView(IEnumerable<ParameterFilterProxy> filters, View view)
        {
            if (filters.Count() == 0) return;
            _dataContext.GetDocument().NewTransaction(() =>
            {
                foreach (var filter in filters)
                {
                    OverrideGraphicSettings ogs = new OverrideGraphicSettings();
                    ogs = view.GetElementOverrides(filter.Id);
                    ogs.SetSurfaceForegroundPatternColor(filter.Color);
                    view.SetElementOverrides(filter.Id, ogs);
                }
            });
        }

        public void SetFiltersVisibility(IEnumerable<ParameterFilterProxy> filters, View view)
        {
            this._dataContext.GetDocument().NewTransaction(delegate
            {
                foreach (ParameterFilterProxy item in filters)
                {
                    view.SetFilterVisibility(item.Id, (bool)item.IsVisible);
                }
            });
        }

        public void OverrideColor(ParameterFilterProxy filter, View view, Color color)
        {
            var document = filter.Document;
            if (document == null) return;
            document.NewTransaction(() =>
            {
                OverrideGraphicSettings ogs = new OverrideGraphicSettings();
                ogs = view.GetElementOverrides(filter.Id);
                ogs.SetSurfaceForegroundPatternColor(color);
                view.SetElementOverrides(filter.Id, ogs);
            });
        }

        private void ChangeColor(Document doc, ElementId elementId, Color red)
        {
            //获取填充图案
            FilteredElementCollector fillPatternElementCollector = new FilteredElementCollector(doc);
            fillPatternElementCollector.OfClass(typeof(FillPatternElement));
            FillPatternElement fillPatternElement = fillPatternElementCollector.First(f => (f as FillPatternElement).GetFillPattern().IsSolidFill)
                as FillPatternElement;
            //修改图元的填充图案与颜色并应用于当前视图
            OverrideGraphicSettings ogs = new OverrideGraphicSettings();
            ogs = doc.ActiveView.GetElementOverrides(elementId);
            ogs.SetSurfaceForegroundPatternColor(red);
            ogs.SetSurfaceForegroundPatternId(fillPatternElement.Id);
            doc.ActiveView.SetElementOverrides(elementId, ogs);
        }
    }
}
