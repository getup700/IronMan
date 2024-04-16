using Autodesk.Revit.DB;
using IronMan.Revit.DTO;
using IronMan.Revit.Entity;
using System.Collections.Generic;

namespace IronMan.Revit.IServices
{
    public interface IParameterFilterLabelService
    {
        void DeleteElement(IEnumerable<ParameterFilterProxy> elements, DTO_Filter dto);

        void DeleteElement(ParameterFilterProxy element);

        void DeleteElements(IEnumerable<ParameterFilterProxy> elements);

        /// <summary>
        /// 获取过滤器的标签
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        DTO_Filter GetElement(ParameterFilterProxy element);

        /// <summary>
        /// 获取过个过滤器的全部标签
        /// </summary>
        /// <param name="elements"></param>
        /// <returns></returns>
        DTO_Filter GetElements(IEnumerable<ParameterFilterProxy> elements);

        /// <summary>
        /// 给过滤器增加标签
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="dto"></param>
        /// <returns></returns>
        ParameterFilterProxy Update(ParameterFilterProxy filter, DTO_Filter dto);

        IEnumerable<ParameterFilterProxy> FindByLabelsOr(IEnumerable<ParameterFilterProxy> filters, DTO_Filter dto);

        IEnumerable<ParameterFilterProxy> FindByLabelsWith(IEnumerable<ParameterFilterProxy> filters, DTO_Filter dto);

        ParameterFilterProxy ReplaceFilterLabels(ParameterFilterProxy filter, DTO_Filter dto);

        void ReplaceDocumentLabels(Document document, DTO_Filter dto);

        DTO_Filter GetDocumentLabels(Document document);


    }
}