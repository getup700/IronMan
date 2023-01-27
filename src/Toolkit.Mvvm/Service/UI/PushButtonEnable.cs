using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using IronMan.Revit.Toolkit.Mvvm.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IronMan.Revit.Toolkit.Mvvm.Service.UI
{
    internal class PushButtonEnable : IExternalCommandAvailability
    {
        private IUIProvider _uiProvider;

        public PushButtonEnable(IUIProvider uiProvider)
        {
            _uiProvider = uiProvider;
        }

        public bool IsCommandAvailable(UIApplication applicationData, CategorySet selectedCategories)
        {
            return true;
        }
    }
}
