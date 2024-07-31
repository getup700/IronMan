using Autodesk.Revit.DB;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using IronMan.Revit.Entity;
using IronMan.Revit.IServices;
using IronMan.Revit.Services;
using IronMan.Revit.Toolkit.Mvvm.Interfaces;
using IronMan.Revit.Toolkit.Mvvm.IOC;
using IronMan.Revit.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using IronMan.Revit.Toolkit.Extension;

namespace IronMan.Revit.ViewModels
{
    public class SmartRoomViewModel : ViewModelBase
    {
        private SmartRoomService _smartRoomService;
        private RoomBuildService _roomBuildService;
        private IDataContext _dataContext;
        private IUIProvider _uiProvider;
        private IExternalEventService _externalEventService;

        public SmartRoomViewModel(SmartRoomService smartRoomService,
            RoomBuildService roomBuildService,
            IDataContext dataContext,
            IUIProvider uIProvider,
            IExternalEventService externalEventService)
        {
            _smartRoomService = smartRoomService;
            _roomBuildService = roomBuildService;
            _dataContext = dataContext;
            _uiProvider = uIProvider;
            _externalEventService = externalEventService;
        }

        #region Properties
        public RoomProxy RoomProxy { get; set; }
        private ObservableCollection<FamilySymbol> _doorSymbolList;
        private ObservableCollection<WallType> _wallTypeList;
        private ObservableCollection<FamilySymbol> _toiletSymbolList;
        private ObservableCollection<FamilySymbol> _toiletRollHolderSymbolList;
        private ObservableCollection<Level> _levelList;
        private double _baseOffset;

        public double BaseOffset
        {
            get => _baseOffset;
            set => _baseOffset = value;
        }

        public ObservableCollection<Level> LevelList
        {
            get => new ObservableCollection<Level>(
                _dataContext.GetDocument().GetElements<Level>().Cast<Level>());
            set { Set(ref _levelList, value); }
        }


        public ObservableCollection<FamilySymbol> ToiletRollHolderSymbolList
        {
            get => new ObservableCollection<FamilySymbol>(
                _dataContext.GetDocument().GetElementTypes<FamilySymbol>(f => f.FamilyName.Contains("手纸") || f.FamilyName.Contains("厕纸")));
            set { Set(ref _toiletRollHolderSymbolList, value); }
        }
        public FamilySymbol ToiletRollHolderSymbol { get; set; }

        public ObservableCollection<FamilySymbol> ToiletSymbolList
        {
            get => new ObservableCollection<FamilySymbol>(
                _dataContext.GetDocument().GetElementTypes<FamilySymbol>(f => f.Category.Id.IntegerValue == -2001160));
            set { Set(ref _toiletSymbolList, value); }
        }

        public FamilySymbol ToiletSymbol { get; set; }


        public ObservableCollection<WallType> WallTypeList
        {
            get => new ObservableCollection<WallType>(
                _dataContext.GetDocument().GetElementTypes<WallType>(f => f.Category.Id.IntegerValue == -2000011));
            set { Set(ref _wallTypeList, value); }
        }

        public WallType ThickWallType
        {
            get
            {
                WallType type = WallTypeList.Where(x => x.Name.Contains("30")).FirstOrDefault();
                if (type == null)
                {
                    return WallTypeList.FirstOrDefault();
                }
                else
                {
                    return type;
                }
            }
            set { }

        }

        public WallType ThinWallType
        {
            get => WallTypeList[0];

            set { }
        }


        public ObservableCollection<FamilySymbol> DoorSymbolList
        {
            get => new ObservableCollection<FamilySymbol>(
                _dataContext.GetDocument().GetElementTypes<FamilySymbol>((f) => f.Category.Id.IntegerValue == -2000023));
            set { Set(ref _doorSymbolList, value); }
        }

        public FamilySymbol DoorSymbol
        {
            get
            {
                FamilySymbol fs = _dataContext.GetDocument().GetElementTypes<FamilySymbol>(x => x.Name.Contains("0717")).FirstOrDefault();
                return false ? fs : _dataContext.GetDocument().GetElementTypes<FamilySymbol>(f => f.Category.Id.IntegerValue == -2000023).FirstOrDefault();
            }
            set { }
        }


        #endregion

        #region Commands

        public RelayCommand SubmitCommand => new RelayCommand(() =>
        {
            //_externalEventService.Raise((uiapp) =>
            //{
            //    _smartRoomService.CreateRoomSeparation();
            //});
            while (true)
            {
                try
                {
                    _externalEventService.Raise((uiapp) =>
                    {
                        _roomBuildService.BuildRoom(Major.All, RoomType.WashBasin);
                        //_roomBuildService.Build(RoomProxy, ThickWallType, ThinWallType);
                    });
                }
                catch (OperationCanceledException)
                {
                    break;
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            }
        });
        #endregion
    }
}
