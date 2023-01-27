using Autodesk.Revit.DB;
using Autodesk.Revit.DB.ExtensibleStorage;
using IronMan.Revit.Toolkit.Mvvm.Interfaces;
using IronMan.Revit.Toolkit.Mvvm.Service.ExtensibleService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RvtEntity = Autodesk.Revit.DB.ExtensibleStorage.Entity;

namespace IronMan.Revit.Entity.Profiles
{
    public class SampleProfile : FieldProfile
    {
        private readonly IUIProvider _uiProvider;

        public SampleProfile(IUIProvider uiProvider)
        {
            _uiProvider= uiProvider;
            Fields.Add(Code);
            Fields.Add(Title);
            Fields.Add(Features);
            Fields.Add(Unit);
            Fields.Add(Quantity);
            Fields.Add(Price);
            Fields.Add(TotalPrice);
            Fields.Add(TestDemo);
        }

        //清单五要素
        /// <summary>
        /// 项目编码
        /// </summary>
        public FieldInfo Code => CreateInfo<int>(documetation: "项目编码");

        /// <summary>
        /// 项目名称
        /// </summary>
        public FieldInfo Title => CreateInfo<List<string>>(documetation: "项目名称");

        /// <summary>
        /// 项目特征
        /// </summary>
        public FieldInfo Features => CreateInfo<IList<string>>(name: "ffffff", documetation: "项目特征");

        /// <summary>
        /// 项目单位
        /// </summary>
        public FieldInfo Unit => CreateInfo<string>(documetation: "项目单位");

        /// <summary>
        /// 工程量
        /// </summary>
        public FieldInfo Quantity => CreateInfo<double>(name: "asd", documetation: "工程量", unitType: UnitType.UT_Volume);

        /// <summary>
        /// 综合单价
        /// </summary>
        public FieldInfo Price => CreateInfo<double>(documetation: "综合单价");

        /// <summary>
        /// 总价
        /// </summary>
        public FieldInfo TotalPrice => CreateInfo<double>(documetation: "总价");

        /// <summary>
        /// Entity数据类型
        /// </summary>
        public FieldInfo TestDemo => CreateInfo<RvtEntity>(new SubschemaInfo(_uiProvider));
    }
}
