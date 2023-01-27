using Autodesk.Revit.DB;
using IronMan.Revit.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IronMan.Revit.Interfaces
{
    public interface IDataCleanup<TElement> where TElement : class
    {
        void DeleteElement(TElement element);

        void DeleteElements(IEnumerable<TElement> elements);

        
    }
}
