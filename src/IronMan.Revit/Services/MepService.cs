using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Mechanical;
using IronMan.Revit.DTO;
using IronMan.Revit.Interfaces;
using IronMan.Revit.IServices;
using IronMan.Revit.Toolkit.Extension;
using IronMan.Revit.Toolkit.Mvvm.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IronMan.Revit.Services
{
    public class MepService : IMepService, IDataUpdater<MEPCurve, DTO_Mep>
    {
        private readonly IDataContext _dataContext;

        public MepService(IDataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public MEPCurve Update(MEPCurve mepCurve, DTO_Mep dto)
        {
            if (mepCurve.GetType() != typeof(Duct)) return null;
            if (dto.Width == 0 || dto.Height == 0) return null;
            Duct duct = mepCurve as Duct;
            _dataContext.GetDocument().NewTransaction(() =>
            {
                duct.get_Parameter(BuiltInParameter.RBS_CURVE_WIDTH_PARAM)?.Set(dto.Width.ConvertToFeet());
                duct.get_Parameter(BuiltInParameter.RBS_CURVE_HEIGHT_PARAM)?.Set(dto.Height.ConvertToFeet());
            }, "SizeUpdate");
            //_dataContext.GetDocument().Regenerate();
            return mepCurve;
        }
    }
}
