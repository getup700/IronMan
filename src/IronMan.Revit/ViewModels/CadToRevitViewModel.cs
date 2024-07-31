using Autodesk.Revit.DB;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using IronMan.Revit.Services;
using IronMan.Revit.Toolkit.Extension.Class;
using IronMan.Revit.Toolkit.Mvvm.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace IronMan.Revit.ViewModels
{
    public class CadToRevitViewModel : ViewModelBase
    {
        private CadToRevitService _service;
        private IDataContext _dataContext;
        public CadToRevitViewModel(CadToRevitService cadToRevitService, IDataContext dataContext)
        {
            _service = cadToRevitService;
            _dataContext = dataContext;
            var reference = _dataContext.GetUIDocument().Selection.PickObject(Autodesk.Revit.UI.Selection.ObjectType.Edge);
            MessageBox.Show(_dataContext.GetDocument().GetElement(reference).Name);
            //reference = _service.SelectReference();
            //_service.SelectCad(reference);
        }

        private Reference reference = null;

    }
}
