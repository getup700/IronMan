using Autodesk.Revit.DB;
using Autodesk.Revit.DB.ExtensibleStorage;
using Autodesk.Revit.UI;
using IronMan.Revit.Entity;
using IronMan.Revit.Entity.Profiles;
using IronMan.Revit.IServices;
using IronMan.Revit.Toolkit.Extension;
using IronMan.Revit.Toolkit.Mvvm.Interfaces;
using IronMan.Revit.Toolkit.Mvvm.IOC;
using IronMan.Revit.Toolkit.Mvvm.Service.ExtensibleService;
using NPOI.OpenXmlFormats.Dml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IronMan.Revit.Services
{
    public class SheetTitleService : ISheetTitleServicve
    {
        private IDataContext _dataContext;
        private IDataStorage _dataStorage;

        public SheetTitleService(IDataContext dataContext, IDataStorage schemaInfo)
        {
            _dataContext = dataContext;
            _dataStorage = schemaInfo;
        }

        public void CreateSheetTitls(Viewport viewport)
        {
            if (viewport.SheetId == ElementId.InvalidElementId) return;

            var family = _dataContext.GetDocument().GetElements<Family>(x => x.Name == "IronMan_ViewTitle").FirstOrDefault();
            if (family != null)
            {
                TaskDialog.Show("IronMan", "没有指定的族类型，请载入族名称为“IronMan_ViewTitle”的注释族");
                return;
            }
            try
            {
                _dataContext.GetDocument().NewSubTransaction(() =>
                {
                    var symbol = _dataContext.GetDocument().GetElement(family.GetFamilySymbolIds().FirstOrDefault()) as FamilySymbol;
                    FamilyInstance familyInstance = _dataContext.GetDocument().Create.NewFamilyInstance(GetSheetTitleLocation(viewport), 
                        symbol, _dataContext.GetDocument().GetElement(viewport.OwnerViewId) as View);
                    SetSheetTitleValue(familyInstance, viewport.get_Parameter(Contants.ParameterIds.ViewName).AsString());

                    Schema schema = _dataStorage.GetSchema(typeof(DocumentProxy));
                    Autodesk.Revit.DB.ExtensibleStorage.Entity entity = new Autodesk.Revit.DB.ExtensibleStorage.Entity(schema);
                    entity.Set(nameof(DocumentProxy.SheetTitle), viewport.Id);
                    familyInstance.SetEntity(entity);
                });
            }
            catch (Exception e)
            {
                TaskDialog.Show("IronMan", e.Message);
            }
        }

        private void SetSheetTitleValue(Element element, string value)
        {
            var parameter = element.LookupParameter(nameof(DocumentProxy.SheetTitle));
            parameter?.Set(value);
        }

        private XYZ GetSheetTitleLocation(Viewport viewport)
        {
            var element = GetSheetTitle(viewport.Id);
            if (element != null && element is AnnotationSymbol annotation)
            {
                return (annotation.Location as LocationPoint).Point;

            }
            return ViewportLocation(viewport);
        }

        private XYZ ViewportLocation(Viewport viewport)
        {
            return new XYZ(viewport.GetBoxCenter().X, viewport.GetBoxOutline().MinimumPoint.Y - 0.1, 0);
        }

        private Element GetSheetTitle(ElementId viewportId)
        {
            Element element = null;
            FilteredElementCollector elements = new FilteredElementCollector(_dataContext.GetDocument())
                .WherePasses(new ExtensibleStorageFilter(_dataStorage.GetSchema(typeof(DocumentProxy)).GUID));
            foreach (var symbol in elements)
            {
                var entity = symbol.GetEntity(_dataStorage.GetSchema(typeof(DocumentProxy)));
                if(entity.Schema!=null)
                {
                    ElementId elementId = entity.Get<ElementId>(nameof(DocumentProxy.SheetTitle));
                    if(elementId==viewportId)
                    {
                        element=symbol; 
                    }
                }
            }
            return element;
        }

        public void DeleteSheetTitle()
        {
            var element = GetSheetTitle(ElementId.InvalidElementId);
            if (element != null)
            {
                _dataContext.GetDocument().NewSubTransaction(() =>
                {
                    _dataContext.GetDocument().Delete(element.Id);
                });
            }
        }

        public void UpdateSheetTitle(Viewport viewport)
        {
            ModifyTitleLocation(viewport);
            ModifyTitleValue(viewport);
        }

        private void ModifyTitleValue(Viewport viewport)
        {
            var title = GetSheetTitle(viewport.Id);
            if(title!=null&&title is AnnotationSymbol annotation)
            {
                XYZ point =(title.Location as LocationPoint).Point;
                XYZ location = ViewportLocation(viewport);
                _dataContext.GetDocument().NewSubTransaction(() =>
                {
                    annotation.Location.Move(location - point);
                });
            }
        }

        private void ModifyTitleLocation(Viewport viewport)
        {
            Element element = GetSheetTitle(viewport.Id);
            if (element != null)
            {
                SetSheetTitleValue(element,viewport.get_Parameter(Contants.ParameterIds.ViewName).AsString());
            }
        }
    }
}
