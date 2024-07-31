using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;
using GalaSoft.MvvmLight;
using IronMan.Revit.Entity.Profiles;
using IronMan.Revit.IServices;
using IronMan.Revit.Services;
using IronMan.Revit.Toolkit.Extension;
using IronMan.Revit.Toolkit.Mvvm.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UIFramework.PropertyGrid;

namespace IronMan.Revit.ViewModels
{
    public class InformationImportViewModel : ViewModelBase
    {
        private IDataContext _dataContext;
        private readonly IDefinitionParameterService _definitionParameterService;

        public InformationImportViewModel(IDataContext dataContext, IDefinitionParameterService definitionParameterService)
        {
            _dataContext = dataContext;
            _definitionParameterService = definitionParameterService;
            Initial();
        }

        private void Initial()
        {
            _definitionParameterService.Import();

        }
    }
}
