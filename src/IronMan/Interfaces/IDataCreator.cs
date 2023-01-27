using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IronMan.Revit.Interfaces
{
    public interface IDataCreator<TElement,TDto> where TElement :class 
    {
        TElement CreateElement(TDto dto);
        IEnumerable<TElement> CreateElements(TDto dto);
    }
}
