using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IronMan.Revit.Toolkit.Mvvm.Interfaces
{
    public interface IApplicationData
    {
        string GetName();

        string Assembly { get; }

        Guid ClientId { get; }

        string FullClassName { get; }

        string VendorId { get; }

        string VendorDescription { get; }

    }
}
