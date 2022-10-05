using Autodesk.Revit.DB;
using IronMan.Revit.Entity;
using IronMan.Revit.IServices;
using IronMan.Revit.Toolkit.Mvvm;
using IronMan.Revit.Toolkit.Mvvm.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using Toolkit.Extension;

namespace IronMan.Revit.Services
{
    public class FloorTypeManageService : IFloorTypeManageService
    {
        public FloorTypeManageService(IDataContext dataContext)
        {
            _dataContext = dataContext;
        }

        private readonly IDataContext _dataContext;

        public ObservableCollection<FloorType> GetFloorType()
        {
            return null;
        }

        public IEnumerable<FloorTypeProxy> GetFloorTypes()
        {
            var types = new FilteredElementCollector(_dataContext.GetDocument()).OfCategory(BuiltInCategory.OST_Floors).WhereElementIsElementType().ToElements();

            return types.ToList().ConvertAll(x => new FloorTypeProxy(x as FloorType));
        }

        public void MoveFloors(FloorType floorType, int offset)
        {
            if (offset == 0 || floorType.Id.IntegerValue == 0) return;
            FilteredElementCollector collector = new FilteredElementCollector(_dataContext.GetDocument());
            IEnumerable<Floor> elements = collector.OfCategory(BuiltInCategory.OST_Floors).WhereElementIsNotElementType()
                .ToElements().Select(x => x as Floor).Where(x => x.FloorType.Id.IntegerValue == floorType.Id.IntegerValue);
            if (elements.Count() == 0) return;
            _dataContext.GetDocument().NewTransaction(() =>
            {
                foreach (var floor in elements)
                {
                    //Floor floor = (Floor)item;
                    //if(floor.FloorType.Id != floorType.Id)continue;
                    Parameter height = floor.get_Parameter(BuiltInParameter.FLOOR_HEIGHTABOVELEVEL_PARAM);
                    int newHeight = Convert.ToInt32(height.AsValueString()) + offset;
                    height.SetValueString(Convert.ToString(newHeight));
                }
            });
        }
    }
}
