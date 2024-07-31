using Autodesk.Revit.DB;
using IronMan.Revit.DTO;
using IronMan.Revit.Entity;
using IronMan.Revit.IServices;
using IronMan.Revit.Toolkit.Mvvm.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using IronMan.Revit.Toolkit.Extension;

namespace IronMan.Revit.Services
{
    public class FloorService : IFloorService
    {
        private readonly IDataContext _dataContext;

        public FloorService(IDataContext dataContext)
        {
            _dataContext = dataContext;
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

        public FloorProxy Update(FloorProxy floor, DTO_Floor dto)
        {
            if (dto.Offset == 0 || floor.Id.IntegerValue == 0) return null;
            FilteredElementCollector collector = new FilteredElementCollector(_dataContext.GetDocument());
            List<Floor> elements = _dataContext.GetDocument().GetElements<Floor>(x => x.Id.IntegerValue == floor.Id.IntegerValue).ToList();
            if (elements.Count() == 0) return null;
            _dataContext.GetDocument().NewTransaction(() =>
            {
                foreach (var floor in elements)
                {
                    Parameter height = floor.get_Parameter(BuiltInParameter.FLOOR_HEIGHTABOVELEVEL_PARAM);
                    double newHeight = Convert.ToDouble(height.AsValueString()) + dto.Offset;
                    height.SetValueString(Convert.ToString(newHeight));
                }
            });
            return null;
        }
    }
}
