using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using IronMan.Revit.Entity;
using IronMan.Revit.IServices;
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
    public class DataCollectViewModel : ViewModelBase
    {
        private readonly IDataContext _dataContext;
        private readonly ICollectService _collectService;
        private readonly IRoomLengthExcelService _excelService;
        public DataCollectViewModel(IDataContext dataContext, ICollectService collectService, IRoomLengthExcelService excelService)
        {
            this._dataContext = dataContext;
            _collectService = collectService;
            _excelService = excelService;
            Initial();
        }

        public List<RoomProxy> RoomList { get; set; }

        public void Initial()
        {
            RoomList = new FilteredElementCollector(_dataContext.GetDocument())
                .WherePasses(new RoomFilter())
                .WhereElementIsNotElementType()
                .Cast<Room>()
                .ToList()
                .ConvertAll<RoomProxy>(x => new RoomProxy(x));
            if (RoomList.Count() == 0)
            {
                MessageBox.Show("未检测到房间，请重试");
                return;
            }
            var result = MessageBox.Show($"已统计到项目共有{RoomList.Count}个房间，是否导出房间信息", "Tips", MessageBoxButton.YesNo);
            if (result != MessageBoxResult.Yes) return;

            foreach (var item in RoomList)
            {
                try
                {
                    item.Length = _collectService.GetLength(item);
                }
                catch
                {
                    continue;
                }
            }
            //_collectService.Export(RoomList);
            _excelService.Export(RoomList);

        }
    }
}
