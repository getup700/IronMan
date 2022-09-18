using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IronMan.Interfaces
{
    public interface IDataService<TElement>
    {
        IEnumerable<TElement> GetElements(Func<TElement, bool> predicate = null);

        void DeleteElement(TElement element);

        void DeleteElements(IEnumerable<TElement> elements);

        TElement CreateElement(string name);
    }
}
