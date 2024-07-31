using Autodesk.Revit.DB;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using IronMan.Revit.Entity;
using IronMan.Revit.IServices;
using IronMan.Revit.Toolkit.Mvvm.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IronMan.Revit.Toolkit.Extension;

namespace IronMan.Revit.ViewModels
{
    public class FloorTypeManageViewModel : ViewModelBase
    {
        private IFloorService _floorService;
        private IDataContext _dataContext;

        public FloorTypeManageViewModel(IFloorService floorService, IDataContext dataContext)
        {
            _floorService = floorService;
            _dataContext = dataContext;
        }

        private ObservableCollection<FloorTypeProxy> _floorTypeProxyList;
        private double _offset;

        public double Offset
        {
            get { return _offset; }
            set { Set(ref _offset, value); }
        }

        public ObservableCollection<FloorTypeProxy> FloorTypeProxyList
        {
            get
            {
                List<FloorType> floorTypes = _dataContext.GetDocument().GetElementTypes<FloorType>().ToList();
                return new ObservableCollection<FloorTypeProxy>(floorTypes.ConvertAll(x => new FloorTypeProxy(x)));
            }
            set { }
        }

        public RelayCommand<FloorTypeProxy> EditCommand
        {
            get => new RelayCommand<FloorTypeProxy>((floorType) =>
                {
                    List<FloorProxy> floors = _dataContext.GetDocument().GetElements<Floor>(x => x.FloorType.Id.IntegerValue == floorType.Id.IntegerValue).Select(x => new FloorProxy(x)).ToList();
                    floors.ForEach(x => _dataContext.GetDocument().NewTransaction(() =>
                    {
                        _floorService.Update(x, new DTO.DTO_Floor{ Offset = Offset });
                    }));
                });
        }

        public RelayCommand SubmitCommand
        {
            get => new RelayCommand(() =>
            {
                MessengerInstance.Send<bool>(true, Contants.Tokens.FloorTypeManageView);
            });
        }

    }
}
