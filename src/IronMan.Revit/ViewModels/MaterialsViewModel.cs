using Autodesk.Revit.DB;
using IronMan.Revit.Entity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using System.Collections;
using IronMan.Revit.Toolkit.Extension;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight;
using System.Windows.Forms;
using IronMan.Revit.IServices;
using System.Net.NetworkInformation;
using IronMan.Revit.Services;
using IronMan.Revit.Toolkit.Mvvm.Interfaces;

namespace IronMan.Revit.ViewModels
{
    public class MaterialsViewModel : ViewModelBase
    {
        private readonly IMaterialService _materialService;
        private readonly IProgressBarService _progressBarService;
        private readonly IUIProvider _uiProvider;
        private readonly IDataContext _dataContext;

        public MaterialsViewModel(IMaterialService materialService, IProgressBarService progressBarService, IUIProvider uIProvider, IDataContext dataContext)
        {
            _materialService = materialService;
            _progressBarService = progressBarService;
            _uiProvider = uIProvider;
            _dataContext = dataContext;

            GetElements();
        }

        #region Properties
        private ObservableCollection<MaterialPlus> _materialsPlusCol;
        private string _keyword;

        public string Keyword
        {
            get { return _keyword; }
            set { _keyword = value; }
        }

        public ObservableCollection<MaterialPlus> MaterialsPlusCol
        {
            get => _materialsPlusCol;
            set => Set(ref _materialsPlusCol, value);
        }

        #endregion

        #region RelayCommands

        public RelayCommand QueryElementsCommand { get => new RelayCommand(GetElements); }

        public RelayCommand CreateElementCommand
        {
            get => new RelayCommand(() =>
            {
                //sender的作用是什么，能不能在消息被接收后改自己的值。
                MessengerInstance.Send(new NotificationMessageAction<MaterialPlus>(
                    null,
                    new MaterialDialogViewModel(this._materialService),
                    "Create",
                    (materialPlus) => MaterialsPlusCol.Insert(0, materialPlus)), Contants.Tokens.MaterialDialogWindow);
            });
        }

        public RelayCommand<IList> DeleteElementsCommand
        {
            get => new RelayCommand<IList>((selectedElements) =>
            {
                try
                {
                    _progressBarService.Start(selectedElements.Count);
                    _materialService.DeleteElements(selectedElements.Cast<MaterialPlus>());
                    GetElements();
                    this._progressBarService.Stop();
                }
                catch (Exception e)
                {
                    MessageBox.Show($"{e.Message}\n{e.StackTrace}");
                }
            });
        }

        public RelayCommand<MaterialPlus> EditMaterialCommand
        {
            get => new RelayCommand<MaterialPlus>((materialPlus) =>
            {
                MessengerInstance.Send(new NotificationMessageAction<MaterialPlus>(
                    materialPlus,
                    new MaterialDialogViewModel(_materialService),
                    "Edit",
                    (e) => { }), Contants.Tokens.MaterialDialogWindow);
            });
        }

        public RelayCommand SubmitCommand
        {
            get => new RelayCommand(() =>
            {
                MessengerInstance.Send(true, Contants.Tokens.MaterialsWindow);
            });
        }

        public RelayCommand ExportExcelCommand
        {
            get => new RelayCommand(() =>
            {
                _materialService.Export(MaterialsPlusCol);
            });
        }

        #endregion

        #region Method

        private void GetElements()
        {
            //MaterialsPlusCol = new ObservableCollection<MaterialPlus>(_materialService.GetElements(e => string.IsNullOrEmpty(Keyword) || e.Name.Contains(Keyword)));
            MaterialsPlusCol = new ObservableCollection<MaterialPlus>(
                _dataContext.GetDocument().GetElements<Material>(
                    e => string.IsNullOrEmpty(Keyword) || e.Name.Contains(Keyword)).
                    Select(x => new MaterialPlus(x)));
            var adds = new List<ObservableCollection<MaterialPlus>>();
            for (int i = 0; i < 10000; i++)
            {
                var a = new ObservableCollection<MaterialPlus>(
                  _dataContext.GetDocument().GetElements<Material>(
                      e => string.IsNullOrEmpty(Keyword) || e.Name.Contains(Keyword)).
                      Select(x => new MaterialPlus(x)));
                adds.Add(a);
            }
        }

        ///// <summary>
        ///// 根据IList索引删除元素
        ///// </summary>
        ///// <param name="selectedElements">IList索引</param>
        //private void DeleteElements(IList selectedElements)
        //{
        //    _doc.NewTransaction(() =>
        //    {
        //        for (int i = selectedElements.Count - 1; i >= 0; i--)
        //        {
        //            MaterialPlus materialsPlus = selectedElements[i] as MaterialPlus;
        //            _doc.Delete(materialsPlus.Material.Id);
        //            MaterialsPlusCol.Remove(materialsPlus);
        //        }
        //    });
        //}


        //拖放用mvvmlight
        //private void QueryElements()
        //{
        //    MaterialsPlusCol.Clear();
        //    FilteredElementCollector collection = new FilteredElementCollector(_doc).OfClass(typeof(Material));
        //    var _materials = collection.ToList()
        //        .ConvertAll(x => new MaterialPlus(x as Material))
        //        .Where(e => string.IsNullOrEmpty(Keyword) || e.Name.Contains(Keyword));
        //    foreach (var item in _materials)
        //    {
        //        MaterialsPlusCol.Add(item);
        //    }
        //    //Materials = new ObservableCollection<Materials_DB>(_materials);
        //}

        #endregion

    }

}

