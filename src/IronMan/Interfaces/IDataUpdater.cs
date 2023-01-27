using Org.BouncyCastle.Utilities.Encoders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IronMan.Revit.Interfaces
{
    public interface IDataUpdater<TElement, T> where TElement : class where T : class
    {
        TElement Update(TElement element, T dto);
    }
}
