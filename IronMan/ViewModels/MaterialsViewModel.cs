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
using Toolkit.Extension;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight;
using System.Windows.Forms;
using IronMan.Revit.IServices;
using System.Net.NetworkInformation;
using IronMan.Revit.Services;

namespace IronMan.Revit.ViewModels
{
    public class MaterialsViewModel : ViewModelBase
    {
        public MaterialsViewModel(IMaterialService service,IProgressBarService progressBarService)
        {
            _service = service;
            _progressBarService = progressBarService;
            GetElements();
        }

        #region Properties

        private readonly IMaterialService _service;
        private readonly IProgressBarService _progressBarService;
        private ObservableCollection<MaterialPlus> _materialsPlusCol;
        private string _keyword;
        private RelayCommand _submitCommand;
        private RelayCommand<MaterialPlus> _editMaterialCommand;

        public string Keyword
        {
            get { return _keyword; }
            set { _keyword = value; }
        }

        /// <summary>
        /// 这里不用List而用ObservableCollection<>动态集合类
        /// 增加或删除元素数量INotifyCollectionChanged，修改元素属性INotifyPropertyChanged时会通知界面并进行变更
        /// </summary>
        public ObservableCollection<MaterialPlus> MaterialsPlusCol
        {
            get => _materialsPlusCol;
            set => Set(ref _materialsPlusCol, value);
        }

        #endregion


        #region RelayCommands

        /// <summary>
        /// 搜索材质命令
        /// </summary>
        public RelayCommand QueryElementsCommand { get => new RelayCommand(GetElements); }

        /// <summary>
        /// 创建材质命令
        /// </summary>
        public RelayCommand CreateElementCommand
        {
            get => new RelayCommand(() =>
            {
                MessengerInstance.Send(new NotificationMessageAction<MaterialPlus>(null,
                    new MaterialDialogViewModel(this._service),
                    "Create",
                    (e)=>MaterialsPlusCol.Insert(0,e)),
                    Contacts.Tokens.MaterialDialogWindow);
            });
        }

        /// <summary>
        /// 删除材质命令,IList转IEnumerable<MaterialPlus>
        /// </summary>
        public RelayCommand<IList> DeleteElementsCommand
        {
            get => new RelayCommand<IList>((selectedElements) =>
            {
                try
                {
                    _progressBarService.Start(selectedElements.Count);
                    _service.DeleteElements(selectedElements.Cast<MaterialPlus>());
                    GetElements();
                    this._progressBarService.Stop();
                }
                catch (Exception e)
                {
                    MessageBox.Show($"{e.Message}\n{e.StackTrace}");
                }
            });
        }

        /// <summary>
        /// 编辑材质命令
        /// </summary>
        public RelayCommand<MaterialPlus> EditMaterialCommand { get => _editMaterialCommand ??= new RelayCommand<MaterialPlus>(EditMaterial); }

        /// <summary>
        /// 确认提交命令
        /// </summary>
        public RelayCommand SubmitCommand { get => _submitCommand ??= new RelayCommand(Submit); }//_submitCommand如果为空的话就赋值

        public RelayCommand ExportExcelCommand
        {
            get => new RelayCommand(() =>
            {
                _service.Export(MaterialsPlusCol);
            });
        }

        #endregion


        #region Method

        private void GetElements()
        {
            MaterialsPlusCol = new ObservableCollection<MaterialPlus>(_service.GetElements(e => string.IsNullOrEmpty(Keyword) || e.Name.Contains(Keyword)));
        }

        private void EditMaterial(MaterialPlus materialPlus)
        {
            //MessageBox.Show("edit material command");
            //NotificationMessageAction可以携带信息及发送者，目标，并可做消息回调
            MessengerInstance.Send(new NotificationMessageAction<MaterialPlus>(materialPlus,new MaterialDialogViewModel(_service), "Edit", (e) =>
            {

            }), Contacts.Tokens.MaterialDialogWindow);
        }

        private void Submit()
        {
            MessengerInstance.Send(true, Contacts.Tokens.MaterialsWindow);
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

