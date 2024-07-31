using Autodesk.Revit.DB;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using IronMan.Revit.Contants;
using IronMan.Revit.Entity;
using IronMan.Revit.IServices;
using IronMan.Revit.Services;
using IronMan.Revit.Toolkit.Extension;
using IronMan.Revit.Toolkit.Mvvm.Interfaces;
using NPOI.OpenXmlFormats.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace IronMan.Revit.ViewModels
{
    public class ModeViewModel : ViewModelBase
    {
        private readonly IDataContext _dataContext;
        private readonly IExternalEventService _externalEventService;
        private readonly IFamilyLoadOptions _familyLoadOptions;

        private ElementId _elementId;
        private Reference _reference;

        public ModeViewModel(IDataContext dataContext
            , IFamilyLoadOptions familyLoadOptions
            , IExternalEventService externalEventService)
        {
            _dataContext = dataContext;
            _familyLoadOptions = familyLoadOptions;
            _externalEventService = externalEventService;
        }

        public ElementId ElementId
        {
            get { return _elementId; }
            set { _elementId = value; RaisePropertyChanged(); }
        }

        /// <summary>
        /// 使用模态窗口阻塞线程，选择元素也是阻塞线程。
        /// 选择命令执行后必须窗口关闭，线程继续到选择元素阻塞，再执行后续处理。
        /// </summary>
        public RelayCommand SubmitCommand => new RelayCommand(() =>
        {
            _externalEventService.Raise((uiApp) =>
            {
                try
                {
                    var projectDocument = _dataContext.GetDocument();
                    var reference = _dataContext.GetUIDocument().Selection.PickObject(Autodesk.Revit.UI.Selection.ObjectType.Element);
                    var familyInstance = _dataContext.GetDocument().GetElement(reference) as FamilyInstance;
                    var symbol = familyInstance.Symbol;
                    for (int i = 0; i < 3; i++)
                    {
                        FamilyInstance instance = null;
                        _dataContext.GetDocument().NewTransaction(() =>
                        {
                            instance = _dataContext.GetDocument().Create.NewFamilyInstance(XYZ.Zero + new XYZ(i * 10, 0, 0), symbol, Autodesk.Revit.DB.Structure.StructuralType.NonStructural);
                        }, "Create Instance");
                        var instanceFamily = instance.Symbol.Family;
                        var familyDoc = projectDocument.EditFamily(instanceFamily);
                        familyDoc.NewTransaction(() =>
                        {
                            var familyManager = familyDoc?.FamilyManager;
                            var familyParameters = familyManager?.Parameters?.ToList();
                            if (familyParameters == null)
                            {
                                throw new ArgumentNullException("no family parameters");
                            }

                            familyManager.RemoveParameter(familyParameters.FirstOrDefault(x => x.Definition.Name == i.ToString()));

                        }, "Delete Parameter");
                        familyDoc.LoadFamily(projectDocument, _familyLoadOptions);
                    }

                }
                catch (Exception)
                {

                    throw;
                }
            });
        });


    }
}
