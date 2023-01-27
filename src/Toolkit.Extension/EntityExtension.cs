using Autodesk.Revit.DB.ExtensibleStorage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IronMan.Revit.Toolkit.Extension
{
    public static class EntityExtension
    {
        /// <summary>
        /// 删除实体字段中指定值
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="entity">实体</param>
        /// <param name="fieldName">字段名称</param>
        /// <param name="keys">删除的集合</param>
        /// <exception cref="Exception"></exception>
        public static void Delete<T>(this Entity entity, string fieldName, IList<T> keys)
        {
            //T是BUSHI集合
            IList<T> transferStation;
            try
            {
                transferStation = entity.Get<IList<T>>(fieldName);
            }
            catch (Exception)
            {
                throw new Exception($"未在entity中找到对应类型的字段");
            }
            for (int i = keys.Count - 1; i >= 0; i++)
            {
                if (keys.Any(x => x.Equals(transferStation[i])))
                {
                    transferStation.Remove(keys[i]);
                }
            }
            entity.Clear(fieldName);
            entity.Set<IList<T>>(fieldName, transferStation);
        }
    }
}
