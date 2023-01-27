using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using IronMan.Revit.DTO;
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
using System.Runtime.Serialization;
using IronMan.Revit.Toolkit.Extension.Class;

namespace IronMan.Revit.ViewModels
{
    public class QuicklyWallViewModel : ViewModelBase
    {
        private readonly IWallService _wallService;
        private readonly IDataContext _dataContext;
        private readonly IExternalEventService _externalEventService;
        private readonly IFamilyService _familyService;

        public QuicklyWallViewModel(
            IWallService service,
            IDataContext dataContext,
            IExternalEventService externalEventService,
            IFamilyService familyService)
        {
            _wallService = service;
            _dataContext = dataContext;
            _externalEventService = externalEventService;
            _familyService = familyService;
        }

        public double BaseOffset
        {
            get => Properties.Settings.Default.BaseOffset;
            set => Properties.Settings.Default.BaseOffset = value;
        }

        public Level BaseLevelConstraint
        {
            get => LevelList[Properties.Settings.Default.SelectIndex] ?? LevelList.FirstOrDefault();
            set => Properties.Settings.Default.SelectIndex = LevelList.IndexOf(value);
        }

        public List<Level> LevelList
        {
            get => _dataContext.GetDocument().GetElements<Level>().Cast<Level>().ToList();
            set { }
        }

        private WallProxy _wallProxy;

        public RelayCommand<Level> SelectCommand
        {
            get => new RelayCommand<Level>((level) =>
            {
                UIDocument UIDocument = _dataContext.GetUIDocument();
                _externalEventService.Raise((uiapp) =>
                {
                    while (true)
                    {
                        try
                        {
                            Element element = UIDocument.Document.GetElement(
                                UIDocument.Selection.PickObject(Autodesk.Revit.UI.Selection.ObjectType.Element,
                                new SelectionFilter(x =>x.GetType() == typeof(Wall)),
                                "请选择要修改的墙"));
                            _wallProxy = new WallProxy(element as Wall);
                            var dto = new DTO_Wall()
                            {
                                BaseLevelId = level.Id,
                                BaseOffset = BaseOffset
                            };
                            _wallService.Update(_wallProxy, dto);

                        }
                        catch (OperationCanceledException)
                        {
                            break;
                        }
                        catch (Exception)
                        {
                            //MessageBox.Show($"{e.StackTrace}+\n+{e.Message}", "Caption");
                            break;
                        }

                        try
                        {
                            List<XYZ> points = _wallService.CreateLocationPoint(_wallProxy, 70);
                            _familyService.CreateElements(new DTO_Family()
                            {
                                Name = "隔板支撑",
                                Level = level,
                                BaseOffset = BaseOffset,
                                LocationPoints = points
                            });
                        }
                        catch (Exception)
                        {
                            if(_wallProxy!=null)
                            {
                                throw new Exception("项目未加载族“隔板支撑”，请加载后重试");
                            }
                        }
                    }
                    Properties.Settings.Default.Save();
                });



            });
        }
    }
}
