using Autodesk.Revit.DB.Architecture;
using GalaSoft.MvvmLight;
using IronMan.Revit.IServices;
using IronMan.Revit.Toolkit.Extension;
using IronMan.Revit.Toolkit.Mvvm.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IronMan.Revit.ViewModels
{
    public class InformationExportViewModel : ViewModelBase
    {
        private readonly IDefinitionParameterService _definitionParameterService;
        private readonly IDataContext _dataContext;

        public InformationExportViewModel(IDefinitionParameterService definitionParameterService,IDataContext dataContext)
        {
            _definitionParameterService = definitionParameterService;
            _dataContext = dataContext;
            Initial();
        }

        private void Initial()
        {
            var rooms = _dataContext.GetDocument().GetElementsByElementFilter<Room>(new RoomFilter());
            _definitionParameterService.Export(rooms);
        }
    }
}
