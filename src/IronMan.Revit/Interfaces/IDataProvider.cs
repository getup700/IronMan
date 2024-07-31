using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IronMan.Revit.Interfaces
{
    public interface IDataProvider<TElement> where TElement : class
    {
        IEnumerable<TElement> GetElements(Func<TElement, bool> predicate = null);
    }
}
