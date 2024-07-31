using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using HandyControl.Themes;
using IronMan.Revit.Contants;
using IronMan.Revit.DTO;
using IronMan.Revit.Entity;
using IronMan.Revit.Enum;
using IronMan.Revit.IServices;
using IronMan.Revit.Toolkit.Extension;
using IronMan.Revit.Toolkit.Mvvm.Interfaces;
using IronMan.Revit.Toolkit.Mvvm.IOC;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;

namespace IronMan.Revit.ViewModels
{
    public class ParameterFilterLabelViewModel : ViewModelBase
    {
        #region Fields
        private ObservableCollection<ParameterFilterProxy> _filters;
        private ObservableCollection<ParameterFilterProxy> _modifiedFilterWithoutLabel;
        private ObservableCollection<ParameterFilterProxy> _allFilters;
        private Source _filterSource;
        private string _sourceName;
        private Autodesk.Revit.DB.View activeView => _dataContext.GetDocument().ActiveView;

        private readonly IDataContext _dataContext;
        private readonly IParameterFilterService _filterService;
        private readonly IParameterFilterLabelService _filterLabelService;
        #endregion

        public ParameterFilterLabelViewModel(IDataContext dataContext,
            IParameterFilterService filterService,
            IParameterFilterLabelService filterLabelService)
        {
            _dataContext = dataContext;
            _filterService = filterService;
            _filterLabelService = filterLabelService;
            Filters = new ObservableCollection<ParameterFilterProxy>(_filterService.GetElements());
            AllFilters = new ObservableCollection<ParameterFilterProxy>(_filterService.GetElements());
            Filters.OrderBy(x => x.Name);
            AllFilters.OrderBy(x => x.Name);
            ModifiedFilterWithoutLabel = new ObservableCollection<ParameterFilterProxy>();
            Filters.ToList().ForEach(x => x.SetActiveView(_dataContext.GetDocument().ActiveView));
            AllFilters.ToList().ForEach(x => x.SetActiveView(_dataContext.GetDocument().ActiveView));
            LabelLibrary = _filterLabelService.GetDocumentLabels(_dataContext.GetDocument()).Labels;
            SourceName = "所有过滤器";
            MessengerInstance.Register<IList<string>>(this, Contants.Tokens.CloseFilterLabelLibraryView, SaveLabelLibrary);
        }

        private void SaveLabelLibrary(IList<string> obj)
        {
            LabelLibrary = obj.ToList();
        }

        #region Properties
        /// <summary>
        /// 过滤器名称关键词
        /// </summary>
        public string FilterKeywords { get; set; }

        /// <summary>
        /// 标签查询关键词
        /// </summary>
        public string LabelKeywords { get; set; }

        /// <summary>
        /// 标签库
        /// </summary>
        public List<string> LabelLibrary { get; set; }

        /// <summary>
        /// 展示的Filter
        /// </summary>
        public ObservableCollection<ParameterFilterProxy> Filters
        {
            get => _filters;
            set
            {
                Set(ref _filters, value);
            }
        }

        /// <summary>
        /// Filter库
        /// </summary>
        public ObservableCollection<ParameterFilterProxy> AllFilters
        {
            get { return _allFilters; }
            set { _allFilters = value; }
        }

        /// <summary>
        /// 修改的Filter
        /// </summary>
        public ObservableCollection<ParameterFilterProxy> ModifiedFilterWithoutLabel
        {
            get { return _modifiedFilterWithoutLabel; }
            set
            {
                Set(ref _modifiedFilterWithoutLabel, value);
            }
        }

        public Sort OrderBy { get; set; }

        public Source FilterSource
        {
            get => _filterSource;
            set => Set(ref _filterSource, value);
        }

        public string SourceName
        {
            get { return _sourceName; }
            set { Set(ref _sourceName, value); }
        }

        #endregion

        #region Commands
        /// <summary>
        /// 根据过滤器名称搜索过滤器
        /// </summary>
        public RelayCommand<bool> QueryWithFilterCommand => new RelayCommand<bool>(flag =>
        {
            var filterKeywords = FilterKeywords.CommonSplit();
            if (filterKeywords == null) return;
            //var _filter = new ObservableCollection<ParameterFilterProxy>(_filterService.GetElements());
            if (flag)
            {
                this.Filters = new ObservableCollection<ParameterFilterProxy>(_filterService.FindByFilterOr(AllFilters, new DTO_Filter()
                {
                    Labels = filterKeywords
                }));
            }
            else
            {
                this.Filters = new ObservableCollection<ParameterFilterProxy>(_filterService.FindByFilterWith(AllFilters, new DTO_Filter()
                {
                    Labels = filterKeywords
                }));
            }
            //Refresh(OrderBy);

        });

        /// <summary>
        /// 根据过滤器标签查询过滤器
        /// </summary>
        public RelayCommand<bool> QueryWithLabelCommand => new RelayCommand<bool>(flag =>
        {
            var labelKeywords = LabelKeywords.CommonSplit();
            if (labelKeywords == null) return;
            if (labelKeywords.Count == 0)
            {
                Filters = AllFilters;
                return;
            }
            var result = new ObservableCollection<ParameterFilterProxy>();
            if (flag)
            {
                result = new ObservableCollection<ParameterFilterProxy>(_filterLabelService.FindByLabelsOr(AllFilters, new DTO_Filter()
                {
                    Labels = labelKeywords
                }));
            }
            else
            {
                result = new ObservableCollection<ParameterFilterProxy>(_filterLabelService.FindByLabelsWith(AllFilters, new DTO.DTO_Filter()
                {
                    Labels = labelKeywords
                }));
            }
            Refresh(result, OrderBy);
        });

        /// <summary>
        /// 编辑单个过滤器命令
        /// </summary>
        public RelayCommand<ParameterFilterProxy> EditFilterCommand => new RelayCommand<ParameterFilterProxy>(f =>
        {
            if (f.Document == null) return;
            var filterDialog = new FilterDialog(_dataContext.GetDocument(), f.Id);
            filterDialog.Show();
        });

        /// <summary>
        /// 设置颜色
        /// </summary>
        public RelayCommand<ParameterFilterProxy> OverrideColorCommand => new RelayCommand<ParameterFilterProxy>(f =>
        {
            if (f.Document == null) return;
            var colorDialog = new ColorDialog();
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                f.Color = colorDialog.Color.ConvertToRevitColor();
            }
            ModifiedFilterWithoutLabel.Add(f);
        });

        /// <summary>
        /// 打开标签库
        /// </summary>
        public RelayCommand OpenLabelLibraryCommand => new RelayCommand(() =>
        {
            MessengerInstance.Send<IList<string>>(LabelLibrary, Contants.Tokens.FilterLabelLibraryView);
        });

        /// <summary>
        /// 编辑单个过滤器标签命令
        /// </summary>
        public RelayCommand<ParameterFilterProxy> EditLabelCommand => new RelayCommand<ParameterFilterProxy>(filter =>
        {
            MessengerInstance.Send<ParameterFilterProxy>(filter, Contants.Tokens.FilterDialogView);
            var dialogViewModel = SingletonIOC.Current.Container.GetInstanceWithoutCaching<ParameterFilterLabelDialogViewModel>();
            dialogViewModel.LabelLibrary = LabelLibrary;
            MessengerInstance.Send(new NotificationMessageAction<ParameterFilterProxy>(
                filter,
                dialogViewModel,
                "EditFilter",
                e =>
                {
                    var index = Filters.IndexOf(filter);
                    Filters.Remove(filter);
                    Filters.Insert(index, e);
                    var allFilterIndex =_allFilters.Select(x=>x.Id).ToList().IndexOf(filter.Id);
                    if(allFilterIndex != -1)
                    {
                        _allFilters.RemoveAt(allFilterIndex);
                        _allFilters.Insert(allFilterIndex, e);
                    }
                }), Tokens.FilterDialogView);
        });

        /// <summary>
        /// 编辑多个过滤器标签命令
        /// </summary>
        public RelayCommand<IList> EditFiltersLabelCommand => new RelayCommand<IList>(args =>
        {
            var filters = args.Cast<ParameterFilterProxy>().ToList();
            var dialogViewModel = SingletonIOC.Current.Container.GetInstanceWithoutCaching<ParameterFilterLabelDialogViewModel>();
            dialogViewModel.LabelLibrary = LabelLibrary;
            MessengerInstance.Send(new NotificationMessageAction<IList<ParameterFilterProxy>>(
                filters,
                dialogViewModel,
                "EditFilters",
                e =>
                {
                    foreach (var filter in e)
                    {
                        var index = Filters.IndexOf(filter);
                        Filters.Remove(filter);
                        Filters.Insert(index, filter);
                        var allFilterIndex = _allFilters.Select(x => x.Id).ToList().IndexOf(filter.Id);
                        if(allFilterIndex != -1)
                        {
                            _allFilters.RemoveAt(allFilterIndex);
                            _allFilters.Insert(allFilterIndex, filter);
                        }
                    }
                }), Tokens.FilterDialogView);
        });

        /// <summary>
        /// 过滤器添加到当前视图
        /// </summary>
        public RelayCommand<IList> AddToViewCommand => new RelayCommand<IList>(args =>
        {
            var filters = args.Cast<ParameterFilterProxy>();
            if (filters.Count() == 0) return;
            foreach (var f in filters)
            {
                f.IsApplied = true;
                ModifiedFilterWithoutLabel.Add(f);
            }
        });

        /// <summary>
        /// 从当前视图移除过滤器
        /// </summary>
        public RelayCommand<IList> RemoveFromViewCommand => new RelayCommand<IList>(args =>
        {
            var filters = args.Cast<ParameterFilterProxy>();
            if (filters.Count() == 0) return;
            foreach (var item in filters)
            {
                item.IsApplied = false;
                ModifiedFilterWithoutLabel.Add(item);
            }
        });

        /// <summary>
        /// 提交命令
        /// </summary>
        public RelayCommand SubmitCommand => new RelayCommand(() =>
        {
            _filterLabelService.ReplaceDocumentLabels(_dataContext.GetDocument(), new DTO.DTO_Filter()
            {
                Labels = LabelLibrary.ToList()
            });
            _filterService.ReplaceApplyFiltersInView(ModifiedFilterWithoutLabel, activeView);
            _filterService.ReplaceVisivilityFiltersInView(ModifiedFilterWithoutLabel, activeView);
            //_filterService.ReplaceColorFilterInView(ModifiedFilterWithoutLabel, activeView);
            MessengerInstance.Send<bool>(true, Contants.Tokens.FilterView);
        });

        public RelayCommand SortCommand => new RelayCommand(() =>
        {
            if (SourceName == "所有过滤器")
            {
                Refresh(AllFilters, OrderBy);
            }
            else
            {
                Refresh(new ObservableCollection<ParameterFilterProxy>(AllFilters.Where(f => f.IsApplied == true)), OrderBy);
            }
        });

        /// <summary>
        /// 更改数据源
        /// </summary>
        public RelayCommand<bool> SourceChangeCommand => new RelayCommand<bool>(flag =>
        {
            if (flag)
            {
                Filters = AllFilters;
                SourceName = "所有过滤器";
            }
            else
            {
                Filters = new ObservableCollection<ParameterFilterProxy>(Filters.Where(f => f.IsApplied == true));
                SourceName = "当前视图过滤器";
            }
        });

        public RelayCommand<ParameterFilterProxy> AppliedCommand => new RelayCommand<ParameterFilterProxy>(filter =>
        {
            if (!filter.IsApplied)
            {
                filter.IsVisible = false;
            }
            else
            {
                filter.IsVisible = true;
            }
            ModifiedFilterWithoutLabel.Add(filter);
        });

        public RelayCommand<ParameterFilterProxy> VisibilityCommand => new RelayCommand<ParameterFilterProxy>(filter =>
        {
            ModifiedFilterWithoutLabel.Add(filter);
        });

        public RelayCommand<ParameterFilterProxy> ChangeColorCommand => new RelayCommand<ParameterFilterProxy>(filter =>
        {
            var dialog = new ColorDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                filter.Color = dialog.Color.ConvertToRevitColor();
            }
            ModifiedFilterWithoutLabel.Add(filter);
        });


        #endregion

        public void Refresh(IEnumerable<ParameterFilterProxy> filters, Sort order)
        {
            if (filters.Count() == 0)
            {
                Filters.Clear();
            }
            Filters.Clear();
            var result = new List<ParameterFilterProxy>();
            switch (order)
            {
                case Sort.Initial:
                    result = filters.ToList();
                    break;
                case Sort.FilterName:
                    result = filters.OrderBy(x => x.Name).ToList();
                    break;
                case Sort.LabelCount:
                    result = filters.OrderByDescending(x => x.Labels.Count()).ToList();
                    break;
            }
            Filters = new ObservableCollection<ParameterFilterProxy>(result);
        }

    }

}
