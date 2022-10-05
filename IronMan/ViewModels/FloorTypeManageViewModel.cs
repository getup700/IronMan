using Autodesk.Revit.DB;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using IronMan.Revit.Entity;
using IronMan.Revit.IServices;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IronMan.Revit.ViewModels
{
    public class FloorTypeManageViewModel : ViewModelBase
    {
        public FloorTypeManageViewModel(IFloorTypeManageService floorTypeManageService)
        {
            _service = floorTypeManageService;
            _floorTypeProxyList = new ObservableCollection<FloorTypeProxy>(_service.GetFloorTypes());
        }

        private IFloorTypeManageService _service;
        private ObservableCollection<FloorTypeProxy> _floorTypeProxyList;
        private string _offset;

        public string Offset
        {
            get { return _offset; }
            set { Set(ref _offset, value); }
        }



        public ObservableCollection<FloorTypeProxy> FloorTypeProxyList
        {
            get { return _floorTypeProxyList; }
            set { _floorTypeProxyList = value; }
        }

        public RelayCommand<FloorTypeProxy> EditCommand
        {
            get => new RelayCommand<FloorTypeProxy>((floorType) =>
                {
                    _service.MoveFloors(floorType.FloorType, Convert.ToInt32(Offset));
                });
        }

        public RelayCommand SubmitCommand
        {
            get => new RelayCommand(() =>
            {
                MessengerInstance.Send<bool>(true, Contacts.Tokens.FloorTypeManageView);
            });
        }



    }
}
