using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Mechanical;
using Autodesk.Revit.UI.Selection;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using IronMan.Revit.IServices;
using IronMan.Revit.Toolkit.Extension.Class;
using IronMan.Revit.Toolkit.Mvvm.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using IronMan.Revit.Toolkit.Extension;

namespace IronMan.Revit.ViewModels
{
    public class SizeConvertViewModel : ViewModelBase
    {
        private readonly IDataContext _dataContext;
        private readonly IMepService _mepService;
        private readonly IExternalEventService _externalEventService;

        public SizeConvertViewModel(IDataContext dataContext, IMepService mepService, IExternalEventService externalEventService)
        {
            _dataContext = dataContext;
            _mepService = mepService;
            _externalEventService = externalEventService;
            SelectMepCurve();
            Initialize();
        }



        public RelayCommand WidthCalculateCommand => new RelayCommand(() =>
        {
            if (Height == 0)
            {
                return;
            }
            Width = OldHeight * OldWidth / Height;
        });

        public RelayCommand HeightCalculateCommand => new RelayCommand(() =>
        {
            if (Width == 0)
            {
                return;
            }
            Height = OldHeight * OldWidth / Width;
        });

        public RelayCommand SubmitCommand => new RelayCommand(() =>
        {
            _externalEventService.Raise(uiapp =>
            {
                _mepService.Update(ReferenceMepCurve, new DTO.DTO_Mep() { Height = Height, Width = Width });
            });
        });

        private double _oldWidth;
        private double _oldHeight;
        private double _width;
        private double _height;

        public MEPCurve ReferenceMepCurve { get; set; }

        public double Height
        {
            get { return _height; }
            set { Set(ref _height, value); }
        }


        public double Width
        {
            get { return _width; }
            set { Set(ref _width, value); }
        }


        public double OldHeight
        {
            get { return _oldHeight; ; }
            set { Set(ref _oldHeight, value); }
        }


        public double OldWidth
        {
            get { return _oldWidth; }
            set { Set(ref _oldWidth, value); }
        }

        private void Initialize()
        {
            var mepCurve = ReferenceMepCurve;
            _oldWidth = mepCurve.get_Parameter(BuiltInParameter.RBS_CURVE_WIDTH_PARAM).AsDouble().ConvertToMilliMeters();
            _oldHeight = mepCurve.get_Parameter(BuiltInParameter.RBS_CURVE_HEIGHT_PARAM).AsDouble().ConvertToMilliMeters();
        }

        private void SelectMepCurve()
        {
            Selection selection = _dataContext.GetUIDocument().Selection;
            List<Element> elements = selection.GetElementIds().Select(x => x.ConvertElement(_dataContext.GetDocument())).ToList();
            if (elements.Count != 0 && elements.Where(x => typeof(Duct) == x.GetType()).Count() != 0)
            {
                MEPCurve duct = elements.Where(x => typeof(Duct) == x.GetType()).FirstOrDefault() as MEPCurve;
                ReferenceMepCurve = duct;
            }
            else
            {
                try
                {
                    Reference reference = selection.PickObject(ObjectType.Element, new SelectionFilter(typeof(Duct)), "选择要修改的风管");
                    MEPCurve mepCurve = _dataContext.GetDocument().GetElement(reference) as MEPCurve;
                    ReferenceMepCurve=mepCurve;
                }
                catch (OperationCanceledException)
                {
                    return;
                }
            }
        }

    }
}
