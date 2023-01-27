using Autodesk.Revit.DB;
using IronMan.Revit.DTO;
using IronMan.Revit.Entity;
using IronMan.Revit.Interfaces;
using IronMan.Revit.IServices;
using IronMan.Revit.Toolkit.Mvvm.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IronMan.Revit.Toolkit.Extension;

namespace IronMan.Revit.Services
{
    public class FamilyService : IFamilyService
    {
        private IDataContext _dataContext;
        private Document Document => _dataContext.GetDocument();

        public FamilyService(IDataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public Family LoadFamily(string name)
        {
            //var dllPath = typeof(App).Assembly.Location;
            //var filePath = $"\\{Path.GetDirectoryName(dllPath)}\\Family\\{name}.rfa";
            //bool loadSuccess = Document.LoadFamily(filePath, out Family f);
            //Family family = loadSuccess ? f : null;
            //return family;
            return null;
        }

        public FamilyProxy CreateElement(DTO_Family dto)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<FamilyProxy> CreateElements(DTO_Family dtoFamily)
        {
            if (dtoFamily.Level == null || dtoFamily.Name == null) return null;
            Family family = Document.GetElements<Family>((x) =>
            {
                if (x.Name == dtoFamily.Name)
                    return true;
                return false;
            }).FirstOrDefault() ?? LoadFamily(dtoFamily.Name);

            var familySymbol = Document.GetElement(family.GetFamilySymbolIds().FirstOrDefault()) as FamilySymbol;
            List<FamilyProxy> result = new List<FamilyProxy>();

            Document.NewTransaction(() =>
            {
                foreach (var p in dtoFamily.LocationPoints)
                {
                    FamilyInstance familyInstance = Document.Create.NewFamilyInstance(p, familySymbol, dtoFamily.Level, Autodesk.Revit.DB.Structure.StructuralType.NonStructural);
                    familyInstance.get_Parameter(BuiltInParameter.INSTANCE_FREE_HOST_OFFSET_PARAM).Set((dtoFamily.BaseOffset - 100).ConvertToFeet());
                    result.Add(new FamilyProxy(familyInstance));
                }
            },"Create FamilyInstance");

            return result;
        }
    }
}
