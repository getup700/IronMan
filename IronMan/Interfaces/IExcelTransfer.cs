using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IronMan.Revit.Interfaces
{
    public interface IExcelTransfer<TElement>
    {
        /// <summary>
        /// 数据导入
        /// </summary>
        /// <returns></returns>
        IEnumerable<TElement> Import();

        /// <summary>
        /// 数据导出
        /// </summary>
        /// <param name="elements"></param>
        void Export(IEnumerable<TElement> elements);
    }
}
