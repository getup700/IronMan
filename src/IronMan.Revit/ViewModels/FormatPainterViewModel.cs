using Autodesk.Revit.DB;
using GalaSoft.MvvmLight;
using IronMan.Revit.DTO;
using IronMan.Revit.IServices;
using IronMan.Revit.Services;
using IronMan.Revit.Toolkit.Mvvm.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace IronMan.Revit.ViewModels
{
    public class FormatPainterViewModel : ViewModelBase
    {
        private readonly IDataContext _dataContext;
        private readonly IExternalEventService _externalEventService;
        private readonly MepInfomationService _mepInfomationService;

        public FormatPainterViewModel(IDataContext dataContext, IExternalEventService externalEventService, MepInfomationService mepInfomationService)
        {
            _dataContext = dataContext;
            _externalEventService = externalEventService;
            _mepInfomationService = mepInfomationService;
            Initial();
        }

        private ObservableCollection<DTO_KeyValue> _dto_KeyValue;

        public ObservableCollection<DTO_KeyValue> Dto_KeyValueList
        {
            get { return _dto_KeyValue; }
            set { Set(ref _dto_KeyValue, value); }
        }


        private void Initial()
        {
            Element element = _dataContext.GetDocument()
                .GetElement(_dataContext.GetUIDocument()
                .Selection.PickObject(Autodesk.Revit.UI.Selection.ObjectType.Element));

            //DTO.DTO_KeyValue dictionaryInfo = _mepInfomationService.GetInformation(element);
            ////_propertyList = new ObservableCollection<Dictionary<string, string>>();
            //DataTableList = dictionaryInfo.KeyValue;
            //Dto_KeyValue= new ObservableCollection<DTO_KeyValue>(dictionaryInfo.KeyValue)
            var list = _mepInfomationService.GetInformation(element);
            _dto_KeyValue = new ObservableCollection<DTO_KeyValue>(list);
            _externalEventService.Raise(app =>
            {
                //while (true)
                //{
                    List<Element> elements=new List<Element>();
                    try
                    {
                        var references = _dataContext.GetUIDocument().Selection.PickObjects(Autodesk.Revit.UI.Selection.ObjectType.Element);
                        foreach (var reference in references)
                        {
                            elements.Add(_dataContext.GetDocument().GetElement(reference));
                        }
                    }
                    catch (OperationCanceledException)
                    {
                        //break;
                        throw new Exception("User canceled selection");
                    }
                    var elementType = typeof(Element);
                    foreach (var ele in elements)
                    {
                        _mepInfomationService.Update(ele, Dto_KeyValueList);
                    }
                //}
            });

        }
    }
}
