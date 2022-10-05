using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Visual;
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
        public static void NewTransaction(this Document document, Action action, string name = "Default Transaction Name")
        {
            using (Transaction transaction = new Transaction(document, name))
            {
                transaction.Start();
                action.Invoke();
                transaction.Commit();
            }
        }
        /// <summary>
        /// 开启新的事务组
        /// </summary>
        /// <param name="document">Revit</param>
        /// <param name="name">事务组名称</param>
        /// <param name="func">事务委托</param>
        /// <returns></returns>
        public static TransactionStatus NewTransactionGroup(this Document document, string name, Func<bool> func)
        {
            TransactionStatus status = TransactionStatus.Uninitialized;
            using (TransactionGroup group = new TransactionGroup(document, name))
            {
                group.Start();
                bool result = func.Invoke();
                status = result ? group.Assimilate() : group.RollBack();
            }
            return status;
        }

        public static AppearanceAssetElement CreateAppearanceElement(this Document document, string name)
        {
            AppearanceAssetElement element = null;
            Asset asset = (from x in document.Application.GetAssets(AssetType.Appearance)
                           where x.Name == "Generic"
                           select x).FirstOrDefault<Asset>();
            if (asset != null)
            {
                return AppearanceAssetElement.Create(document, name, asset);
            }
            AppearanceAssetElement element2 = document.GetElements<AppearanceAssetElement>(null).FirstOrDefault<AppearanceAssetElement>();
            if (element2 != null)
            {
                element = element2.Duplicate(name);
            }
            return element;
        }

        public static void CreateParameterFilterElement(this Document document, string name, ICollection<ElementId> ids, FilterRule filterRule)
        {
            ElementParameterFilter elementFilter = new ElementParameterFilter(filterRule);
            ParameterFilterElement element = ParameterFilterElement.Create(document, name, ids, elementFilter);
        }

        public static bool ElementExist<T>(this Document document, Func<T, bool> func) where T : Element =>
            new FilteredElementCollector(document).OfClass(typeof(T)).Cast<T>().Any<T>(func);

        public static List<T> GetElements<T>(this Document document, Func<T, bool> func = null) where T : Element
        {
            IEnumerable<T> source = new FilteredElementCollector(document).WhereElementIsNotElementType().OfClass(typeof(T)).Cast<T>();
            if (func != null)
            {
                source = source.Where<T>(func);
            }
            return source.ToList<T>();
        }

        public static List<T> GetElements<T>(this Document document, ElementFilter elementFilter, Func<T, bool> func = null) where T : Element
        {
            IEnumerable<T> source = new FilteredElementCollector(document).WhereElementIsNotElementType().WherePasses(elementFilter).OfClass(typeof(T)).ToElements().Cast<T>();
            if (func != null)
            {
                source = source.Where<T>(func);
            }
            return source.ToList<T>();
        }

        public static List<T> GetElementTypes<T>(this Document document, Func<T, bool> func = null) where T : ElementType
        {
            IEnumerable<T> source = new FilteredElementCollector(document).WhereElementIsElementType().OfClass(typeof(T)).ToElements().Cast<T>();
            if (func != null)
            {
                source = source.Where<T>(func);
            }
            return source.ToList<T>();
        }

    }
}
