using Autodesk.Revit.DB;
using Autodesk.Revit.DB.ExtensibleStorage;
using Autodesk.Revit.DB.Visual;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using Autodesk.Windows;
using IronMan.Revit.Toolkit.Extension.Class;
using NPOI.OpenXml4Net.OPC;
using NPOI.SS.Formula.Functions;
using Org.BouncyCastle.Bcpg.OpenPgp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.TextFormatting;

namespace IronMan.Revit.Toolkit.Extension
{
    public static class DocumentExtension
    {
        [DebuggerStepThrough]
        public static TransactionStatus NewTransactionGroup(this Document document, string name, Func<bool> func)
        {
            if (document==null)
            {
                throw new ArgumentNullException(nameof(document));
            }
            TransactionStatus status = TransactionStatus.Uninitialized;
            using (TransactionGroup group = new TransactionGroup(document, name))
            {
                group.Start();
                bool result = func.Invoke();
                status = result ? group.Assimilate() : group.RollBack();
                if (status!= TransactionStatus.Committed)
                {
                    Console.WriteLine("log");
                }
            }
            return status;
        }

        [DebuggerStepThrough]
        public static void NewTransaction(this Document document, Action action, string name = "Default Transaction Name")
        {
            if (document == null)
            {
                throw new ArgumentNullException(nameof(document));
            }
            using (Transaction transaction = new Transaction(document, name))
            {
                transaction.Start();
                action.Invoke();
                if (transaction.Commit()!=TransactionStatus.Committed)
                {
                    Console.WriteLine("log");
                }
            }
        }

        [DebuggerStepThrough]
        public static void NewSubTransaction(this Document document, Action action)
        {
            if (document == null)
            {
                throw new ArgumentNullException(nameof(document));
            }
            using (SubTransaction transaction = new SubTransaction(document))
            {
                transaction.Start();
                action.Invoke();
                if (transaction.Commit() != TransactionStatus.Committed)
                {
                    Console.WriteLine("log");
                }
            }
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
            AppearanceAssetElement element2 = document.GetElements<AppearanceAssetElement>().FirstOrDefault();
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

        #region Filter Element
        private static FilteredElementCollector GetElements(this Document document)
        {
            if (document == null)
            {
                throw new ArgumentNullException(nameof(document));
            }
            return new FilteredElementCollector(document);
        }

        //如果用OfClass过滤，返回的Collector仍然是Element，需要在Cast,返回值仍然是.NET类型，使用用法不符。
        //public static FilteredElementCollector GetElements<T>(this Document document) where T : Element
        //{
        //    return GetElements(document).OfClass(typeof(T));
        //}

        public static FilteredElementCollector GetElementInstances(this Document document, BuiltInCategory category)
        {
            return GetElements(document).OfCategory(category).WhereElementIsNotElementType();
        }

        public static FilteredElementCollector GetElementTypes(this Document document, BuiltInCategory category)
        {
            return GetElements(document).OfCategory(category).WhereElementIsElementType();
        }

        public static FilteredElementCollector GetElementsWithOption(this Document document, ElementId optionId)
        {
            return GetElements(document).ContainedInDesignOption(optionId);
        }

        public static FilteredElementCollector GetElementsInView(this Document document, ElementId viewId)
        {
            if (document == null)
            {
                throw new ArgumentNullException(nameof(document));
            }
            return new FilteredElementCollector(document, viewId);
        }

        public static FilteredElementCollector GetElementsInView<T>(this Document document, ElementId viewId) where T : Element
        {
            return GetElementsInView(document, viewId).OfClass(typeof(T));
        }

        public static bool ElementExist<T>(this Document document, Func<T, bool> func) where T : Element
        {
            return GetElements(document).OfClass(typeof(T)).Cast<T>().Any<T>(func);
        }

        /// <summary>
        /// 全面的过滤方法，返回IEnumerable类型。
        /// </summary>
        /// <typeparam name="T">系统族实例、系统族类型、FamilyInstance、FamilySymbol、Family</typeparam>
        /// <param name="document"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static IEnumerable<T> GetElements<T>(this Document document, Func<T, bool> predicate = null) where T : Element
        {
            IEnumerable<T> source = GetElements(document).OfClass(typeof(T)).Cast<T>();
            if (predicate != null)
            {
                source = source.Where(predicate);
            }
            return source;
        }

        public static IEnumerable<T> GetSpatialElement<T>(this Document document,Func<T,bool> predicate =null) where T:SpatialElement
        {
            IEnumerable<T> source = GetElements(document).OfClass(typeof(T)).Cast<T>();
            if (predicate != null)
            {
                source = source.Where(predicate);
            }
            return source;
        }

        public static IEnumerable<T> GetElementsByElementFilter<T>(this Document document, ElementFilter elementFilter, Func<T, bool> predicate = null) where T : Element
        {
            IEnumerable<T> source = GetElements(document).WhereElementIsNotElementType().WherePasses(elementFilter).ToElements().Cast<T>();
            if (predicate != null)
            {
                source = source.Where<T>(predicate);
            }
            return source;
        }

        public static IEnumerable<T> GetElementsByCategoryId<T>(this Document document, ElementId categoryId, Func<T, bool> predicate = null)
        {
            IEnumerable<T> source = GetElements(document).OfCategoryId(categoryId).WhereElementIsNotElementType().Cast<T>();
            if (predicate != null)
            {
                source = source.Where<T>(predicate);
            }
            return source;
        }

        public static IEnumerable<T> GetElementTypes<T>(this Document document, Func<T, bool> predicate = null) where T : ElementType
        {
            IEnumerable<T> source = GetElements(document).WhereElementIsElementType().OfClass(typeof(T)).ToElements().Cast<T>();
            if (predicate != null)
            {
                source = source.Where<T>(predicate);
            }
            return source;
        }

        public static Entity GetRvtEntity(this Document document, string dataStorageName, Schema schema)
        {
            Element dataStorage = GetElements(document).OfClass(typeof(DataStorage)).Where(x => x.Name == dataStorageName).FirstOrDefault();
            if (dataStorage == null) return null;
            var entity = dataStorage.GetEntity(schema);
            if (entity.Schema == null) return null;
            return entity;
        }

        public static void SetRvtEntity(this Document document, string dataStorageName, Entity entity)
        {
            Element dataStorage = GetElements(document).OfClass(typeof(DataStorage)).Where(x => x.Name == dataStorageName).FirstOrDefault();
            if (dataStorage == null) return;
            dataStorage.SetEntity(entity);
        }

        public static Element SelectElement(this Document document, ObjectType objectType = ObjectType.Element, ISelectionFilter selectionFilter = null, string tips = null)
        {
            Reference reference;
            try
            {
                reference = (new UIDocument(document)).Selection.PickObject(objectType, selectionFilter, tips);
            }
            catch (Autodesk.Revit.Exceptions.OperationCanceledException)
            {
                return null;
            }
            if (reference == null) return null;
            return document.GetElement(reference);
        }

        public static void TransientDisplay(this Document document, Line element)
        {
            document.MakeTransientElements(new TransientElementMaker(() =>
            {

            }));
        }

        #endregion
    }
}
