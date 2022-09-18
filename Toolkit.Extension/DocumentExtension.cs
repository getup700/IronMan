using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Toolkit.Extension
{
    public static class DocumentExtension
    {
        /// <summary>
        /// 开启新事务
        /// </summary>
        /// <param name="document">项目</param>
        /// <param name="name">事务名称</param>
        /// <param name="action">事务的委托方法</param>
        public static void NewTransaction(this Document document ,Action action, string name="Default Transaction Name")
        {
            using (Transaction transaction = new Transaction(document, name))
            {
                transaction.Start();
                action.Invoke();
                transaction.Commit();
            }
        }

    }
}
