using Autodesk.Revit.DB.ExtensibleStorage;
using IronMan.Revit.Toolkit.Mvvm.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IronMan.Revit.Toolkit.Mvvm.Service.ExtensibleService
{
    public interface IDataStorage
    {
        Schema GetSchema<T>();

        Schema GetSchema(Type type);

        void ErasureSchema<T>();

        void Append<T>() where T : class;

    }
}
