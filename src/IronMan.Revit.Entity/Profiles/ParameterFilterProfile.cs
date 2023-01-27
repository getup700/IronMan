using IronMan.Revit.Toolkit.Mvvm.Interfaces;
using IronMan.Revit.Toolkit.Mvvm.Service.ExtensibleService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IronMan.Revit.Entity.Profiles
{
    public class ParameterFilterProfile : FieldProfile
    {
        private readonly IUIProvider _uiProvider;

        public ParameterFilterProfile(IUIProvider uiProvider)
        {
            _uiProvider = uiProvider;
            Fields.Add(Label);
        }

        public FieldInfo Label => CreateInfo<IList<string>>(documetation: "标签");
    }
}
