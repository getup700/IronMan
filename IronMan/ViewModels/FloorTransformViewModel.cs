using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
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
    public class FloorTransformViewModel:ViewModelBase
    {
        public FloorTransformViewModel(IFloorTransformService service)
        {
            _floorService = service;
            FloorProxyList = new ObservableCollection<FloorProxy>(service.GetElements());
        }

        #region Properties
        private IFloorTransformService _floorService;
        private ObservableCollection<FloorProxy> _floorProxyList;
        private string _keyword;


        public string Keyword
        {
            get { return _keyword; }
            set { _keyword = value; }
        }

        public ObservableCollection<FloorProxy> FloorProxyList
        {
            get { return _floorProxyList; }
            set { Set(ref _floorProxyList, value); }
        }

        #endregion

        #region Commands
        public RelayCommand DeleteElementsCommand
        {
            get => new RelayCommand(() =>
            {
                _floorService.DeleteElements(FloorProxyList);
            });
        }

        public RelayCommand DeleteElementCommand
        {
            get => new RelayCommand(() =>
            {
                _floorService.DeleteElement(FloorProxyList.FirstOrDefault());
            });
        }

        public RelayCommand EditCommand
        {
            get => new RelayCommand(() =>
            {

            });
        }

        public RelayCommand SubmitCommand
        {
            get => new RelayCommand(() =>
            {

            });
        }
        #endregion

        #region Methods


        #endregion
    }
}
