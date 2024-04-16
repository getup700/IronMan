using Autodesk.Revit.DB;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using IronMan.Revit.IServices;
using IronMan.Revit.Toolkit.Extension.Class;
using IronMan.Revit.Toolkit.Mvvm.Interfaces;
using NPOI.OpenXmlFormats.Dml.Chart;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IronMan.Revit.ViewModels
{
    public class CopyWallVeiwModel : ViewModelBase
    {
        private IDataContext _dataContext;
        private IWallService _wallService;
        private IExternalEventService _externalEventService;

        public CopyWallVeiwModel(IDataContext dataContext, IWallService wallService, IExternalEventService externalEventService)
        {
            _dataContext = dataContext;
            _wallService = wallService;
            _externalEventService = externalEventService;
        }

        private ObservableCollection<WallType> _walltypeList;

        public ObservableCollection<WallType> WallTypeList
        {
            get { return _walltypeList; }
            set { Set(ref _walltypeList, value); }
        }

        public double Height { get; set; }

        public RelayCommand<WallType> SubmitCommand => new RelayCommand<WallType>((wallType) =>
        {
            _externalEventService.Raise((uiapp) =>
            {
                var reference = _dataContext.GetUIDocument().Selection.PickObject(Autodesk.Revit.UI.Selection.ObjectType.Element,
                    new SelectionFilter(typeof(Wall)), "选择要复制的墙");
                Wall wall = _dataContext.GetDocument().GetElement(reference) as Wall;
                var geometry = wall.get_Geometry(new Options() { });
                ElementTransformUtils.CopyElement(_dataContext.GetDocument(), wall.Id, new XYZ(0, 0, 1000));

            });

        });
    }
}
