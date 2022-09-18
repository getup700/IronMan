using Autodesk.Revit.DB;
using IronMan.Revit.Entity;
using IronMan.Interfaces;
using IronMan.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Toolkit.Extension;

namespace IronMan.Services
{
    public class MaterialService : IMaterialService
    {
        private readonly IDataContext _dataContext;

        public MaterialService(IDataContext dataContext)
        {
            this._dataContext = dataContext;
        }

        public MaterialPlus CreateElement(string name)
        {
            Element element = null;
            _dataContext.Document.NewTransaction(() =>
            {
                ElementId id = Material.Create(_dataContext.Document, name);
                element = _dataContext.Document.GetElement(id);
            }, "创建材质");
            return new MaterialPlus(element as Material);
        }

        public void DeleteElement(MaterialPlus element)
        {
            if(element == null)return;
            _dataContext.Document.NewTransaction(() =>
            {
                _dataContext.Document.Delete(element.Id);
            },"删除材质");
        }

        public void DeleteElements(IEnumerable<MaterialPlus> elements)
        {
            if (elements.Count() == 0) return;
            _dataContext.Document.NewTransaction(() =>
            {
                foreach (var element in elements)
                {
                    _dataContext.Document.Delete(element.Id);
                }
            }, "删除材质");
        }

        public IEnumerable<MaterialPlus> GetElements(Func<MaterialPlus, bool> predicate = null)
        {
            FilteredElementCollector elements = new FilteredElementCollector(_dataContext.Document).OfClass(typeof(Material));
            IEnumerable<MaterialPlus> materialPlus = elements.ToList().ConvertAll(x => new MaterialPlus(x as Material));
            if(predicate != null)
            {
                materialPlus = materialPlus.Where(predicate);
            }
            return materialPlus;
        }

        public void Export(IEnumerable<MaterialPlus> elements)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<MaterialPlus> Import()
        {
            throw new NotImplementedException();
        }

    }
}
