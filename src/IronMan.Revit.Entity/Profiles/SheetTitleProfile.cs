using Autodesk.Revit.DB;
using IronMan.Revit.Toolkit.Mvvm.Service.ExtensibleService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IronMan.Revit.Entity.Profiles
{
    public class SheetTitleProfile:FieldProfile
    {
        public SheetTitleProfile()
        {
            Fields.Add(Title);
        }

        public FieldInfo Title => CreateInfo<ElementId>();
    }
}
