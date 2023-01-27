using IronMan.Revit.Toolkit.Mvvm.Interfaces;
using IronMan.Revit.Toolkit.Mvvm.Service.ExtensibleService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IronMan.Revit.Entity.Profiles
{
    public class SubschemaProfile : FieldProfile
    {
        private readonly IUIProvider _uiProvider;
        public SubschemaProfile(IUIProvider uiProvider)
        {
            _uiProvider = uiProvider;
        }
    }
}
