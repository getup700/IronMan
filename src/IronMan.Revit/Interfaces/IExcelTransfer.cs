using IronMan.Revit.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IronMan.Revit.Interfaces
{
    public interface IExcelTransfer<TElement> where TElement : class
    {
        IEnumerable<TElement> Import();

        void Export(IEnumerable<TElement> elements);
    }
}
