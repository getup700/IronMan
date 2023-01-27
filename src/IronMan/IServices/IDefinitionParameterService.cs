using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;
using IronMan.Revit.Entity.Profiles;
using IronMan.Revit.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IronMan.Revit.IServices
{
    public interface IDefinitionParameterService:IExcelTransfer<Room>
    {
        /// <summary>
        /// 绑定项目参数
        /// </summary>
        /// <param name="categorySet">被绑定的参数类别</param>
        /// <param name="definitionProxy">参数</param>
        void BindingElement(CategorySet categorySet, IDefinition definitionProxy);

        /// <summary>
        /// 获取参数，没有则补充创建
        /// </summary>
        /// <returns></returns>
        List<Parameter> GetDecoraParameter(DecorationParameterProfile decorationProfile);
    }
}
