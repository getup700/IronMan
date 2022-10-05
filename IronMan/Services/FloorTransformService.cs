using Autodesk.Revit.DB;
using IronMan.Revit.Entity;
using IronMan.Revit.IServices;
using IronMan.Revit.Toolkit.Mvvm.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IronMan.Revit.Services
{
    internal class FloorTransformService : IFloorTransformService
    {
        public FloorTransformService(IDataContext dataContext)
        {
            _dataContext = dataContext;
        }
        private readonly IDataContext _dataContext;
        public FloorProxy CreateElement(string name)
        {
            throw new NotImplementedException();
        }

        public void DeleteElement(FloorProxy element)
        {
            throw new NotImplementedException();
        }

        public void DeleteElements(IEnumerable<FloorProxy> elements)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<FloorProxy> GetElements(Func<FloorProxy, bool> predicate = null)
        {
            List<Floor> elements = new FilteredElementCollector(_dataContext.GetDocument()).OfClass(typeof(Floor))
            .WhereElementIsNotElementType().ToList().ConvertAll(t => t as Floor);
            return elements.ToList().Select(x => new FloorProxy(x));
        }
    }
}
