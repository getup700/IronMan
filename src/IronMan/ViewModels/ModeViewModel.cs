using Autodesk.Revit.DB;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using IronMan.Revit.Contants;
using IronMan.Revit.Entity;
using IronMan.Revit.IServices;
using IronMan.Revit.Services;
using IronMan.Revit.Toolkit.Mvvm.Interfaces;
using NPOI.OpenXmlFormats.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace IronMan.Revit.ViewModels
{
    public class ModeViewModel : ViewModelBase
    {
        private readonly IDataContext _dataContext;
        private readonly IExternalEventService _externalEventService;

        private ElementId _elementId;
        private Reference _reference;

        public ModeViewModel(IDataContext dataContext, IExternalEventService externalEventService)
        {
            _dataContext = dataContext;
            _externalEventService = externalEventService;
        }

        public ElementId ElementId
        {
            get { return _elementId; }
            set { _elementId = value; RaisePropertyChanged(); }
        }

        /// <summary>
        /// 使用模态窗口阻塞线程，选择元素也是阻塞线程。
        /// 选择命令执行后必须窗口关闭，线程继续到选择元素阻塞，再执行后续处理。
        /// </summary>
        public RelayCommand SubmitCommand => new RelayCommand(() =>
        {
            _externalEventService.Raise((uiApp) =>
            {
                _reference = _dataContext.GetUIDocument().Selection.PickObject(Autodesk.Revit.UI.Selection.ObjectType.Element, "Select Element");
                ElementId = _reference.ElementId;
            });
        });


    }
}
