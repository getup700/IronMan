using Autodesk.Revit.DB;
using IronMan.Revit.DTO;
using IronMan.Revit.Entity;
using IronMan.Revit.Interfaces;
using IronMan.Revit.Toolkit.Extension;
using IronMan.Revit.Toolkit.Mvvm.Interfaces;
using IronMan.Revit.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB.ExtensibleStorage;
using IronMan.Revit.Toolkit.Mvvm.Service.ExtensibleService;
using RvtEntity = Autodesk.Revit.DB.ExtensibleStorage.Entity;
using System.Windows.Documents;
using System.Xml.Linq;

namespace IronMan.Revit.Services
{
    public class ParameterFilterLaberService : IParameterFilterLabelService, IDataUpdater<ParameterFilterProxy, DTO_Filter>
    {
        private IDataContext _dataContext;
        private IDataStorage _dataStorage;

        public ParameterFilterLaberService(IDataContext dataContext, IDataStorage dataStorage)
        {
            _dataContext = dataContext;
            _dataStorage = dataStorage;
        }

        /// <summary>
        /// 给单个过滤器添加标签
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="dto"></param>
        /// <returns></returns>
        public ParameterFilterProxy Update(ParameterFilterProxy filter, DTO_Filter dto)
        {
            if (dto.Labels.Count == 0) return null;
            Schema schema = this._dataStorage.GetSchema(typeof(ParameterFilterProxy));
            if (schema == null) return null;
            RvtEntity entity = filter.GetEntity(schema);
            this._dataContext.GetDocument().NewTransaction(() =>
            {
                entity.Set<IList<string>>("Labels", dto.Labels);
                filter.SetEntity(entity);
            });
            return filter;
        }

        /// <summary>
        /// 删除单个过滤器中所有标签
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public void DeleteElement(ParameterFilterProxy element)
        {
            Schema schema = this._dataStorage.GetSchema(typeof(ParameterFilterProxy));
            if (schema == null) return;
            RvtEntity entity = element.GetEntity(schema);
            this._dataContext.GetDocument().NewTransaction(() =>
            {
                entity.Clear("Labels");
                element.SetEntity(entity);
            });
        }

        /// <summary>
        /// 删除多个过滤器中所有标签
        /// </summary>
        /// <param name="elements"></param>
        public void DeleteElements(IEnumerable<ParameterFilterProxy> elements)
        {
            if (elements.Count() == 0) return;
            Schema schema = this._dataStorage.GetSchema(typeof(ParameterFilterProxy));
            if (schema == null) return;
            this._dataContext.GetDocument().NewTransaction(() =>
            {
                foreach (var element in elements)
                {
                    RvtEntity entity = element.GetEntity(schema);
                    if (entity.Schema == null) continue;
                    entity.Clear("Labels");
                    element.SetEntity(entity);
                }
            });
        }

        public DTO_Filter GetElement(ParameterFilterProxy element)
        {
            Schema schema = _dataStorage.GetSchema(typeof(ParameterFilterProxy));
            var entity = element.GetEntity(schema);
            if (entity.Schema == null) return null;
            var dto = new DTO_Filter() { Labels = entity.Get<IList<string>>("Labels").ToList() };
            return dto;
        }

        /// <summary>
        /// 获取多个过滤器中所有标签
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public DTO_Filter GetElements(IEnumerable<ParameterFilterProxy> elements)
        {
            if (elements.Count() == 0) return null;
            var list = new List<string>();
            Schema schema = _dataStorage.GetSchema(typeof(ParameterFilterProxy));
            foreach (var filter in elements)
            {
                var entity = filter.GetEntity(schema);
                var info = entity.Get<IList<string>>("Labels");
                list.AddRange(info);
            }
            var dto = new DTO_Filter()
            {
                Labels = list.Distinct<string>().ToList()
            };
            return dto;
        }

        /// <summary>
        /// 删除过滤器中含指定名称的标签
        /// </summary>
        /// <param name="elements"></param>
        /// <param name="dto"></param>
        public void DeleteElement(IEnumerable<ParameterFilterProxy> elements, DTO_Filter dto)

        {
            if (dto.Name.Count() == 0 || elements.Count() == 0) return;
            Schema schema = _dataStorage.GetSchema(typeof(ParameterFilterProxy));
            _dataContext.GetDocument().NewTransaction(() =>
            {
                foreach (var filter in elements)
                {
                    var entity = filter.GetEntity(schema);
                    entity.Delete<string>("Labels", dto.Labels);

                }
            }, "DeleteLabels");
        }

        /// <summary>
        /// 查询包含部分标签的过滤器
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IEnumerable<ParameterFilterProxy> FindByLabelsOr(IEnumerable<ParameterFilterProxy> filters, DTO_Filter dto)
        {
            if (filters.Count() == 0) return null;
            var results = new List<ParameterFilterProxy>();
            Schema schema = _dataStorage.GetSchema(typeof(ParameterFilterProxy));
            foreach (var filter in filters)
            {
                var entity = filter.GetEntity(schema);
                var info = entity.Get<IList<string>>("Labels");
                var intersect = info.Intersect(dto.Labels);
                if (intersect.Count() > 0)
                {
                    results.Add(filter);
                }
            }
            return results;
        }

        /// <summary>
        /// 查询包含全部标签的过滤器
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IEnumerable<ParameterFilterProxy> FindByLabelsWith(IEnumerable<ParameterFilterProxy> filters, DTO_Filter dto)
        {
            if (filters.Count() == 0) return null;
            var results = new List<ParameterFilterProxy>();
            Schema schema = _dataStorage.GetSchema(typeof(ParameterFilterProxy));
            foreach (var filter in filters)
            {
                var entity = filter.GetEntity(schema);
                var info = entity.Get<IList<string>>("Labels");
                //如果dto.Labels是Info的子集
                if (info.ToHashSet().IsSubsetOf(dto.Labels))
                {
                    if(filter.Labels.Count > 0)
                    {
                        results.Add(filter);
                    }
                }
            }
            return results;
        }

        public ParameterFilterProxy ReplaceFilterLabels(ParameterFilterProxy filter, DTO_Filter dto)
        {
            Schema schema = _dataStorage.GetSchema(typeof(ParameterFilterProxy));
            var entity = filter.GetEntity(schema);
            _dataContext.GetDocument().NewTransaction(() =>
            {
                entity.Clear("Labels");
                entity.Set<IList<string>>("Labels", dto.Labels);
                filter.SetEntity(entity);
            }, "替换过滤器标签");
            filter.Labels.Clear();
            filter.Labels = dto.Labels;
            return filter;
        }

        public void ReplaceDocumentLabels(Document document, DTO_Filter dto)
        {
            Schema schema = _dataStorage.GetSchema(typeof(DocumentProxy));
            var entity = document.GetRvtEntity(Contants.IronManID.DataStorageName, schema);
            if (entity.Schema == null) return;
            _dataContext.GetDocument().NewTransaction(() =>
            {
                entity.Clear("Labels");
                entity.Set<IList<string>>("Labels", dto.Labels.Select(x => x.Trim()).ToList());
                document.SetRvtEntity(Contants.IronManID.DataStorageName, entity);
            }, "替换文档标签");
        }

        public DTO_Filter GetDocumentLabels(Document document)
        {
            Schema schema = _dataStorage.GetSchema(typeof(DocumentProxy));
            var entity = document.GetRvtEntity(Contants.IronManID.DataStorageName, schema);
            if (entity.Schema == null) return null;
            var dto = new DTO_Filter() { Labels = entity.Get<IList<string>>("Labels").ToList() };
            return dto;
        }

    }
}
