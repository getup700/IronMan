using Autodesk.Revit.DB;
using GalaSoft.MvvmLight.Messaging;
using IronMan.Revit.Contants;
using IronMan.Revit.Entity;
using IronMan.Revit.Toolkit.Mvvm.IOC;
using IronMan.Revit.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace IronMan.Revit.Views
{
    /// <summary>
    /// FilterView.xaml 的交互逻辑
    /// </summary>
    public partial class FilterView : Window
    {
        public FilterView()
        {
            InitializeComponent();
            Messenger.Default.Register<bool>(this, Tokens.FilterView, CloseWindow);
            Messenger.Default.Register<IList<string>>(this, Tokens.FilterLabelLibraryView, ShowLabelLibraryDialog);
            //向Dialog发送filter和LabelLibrary
            Messenger.Default.Register<NotificationMessageAction<ParameterFilterProxy>>(this, Tokens.FilterDialogView, ShowFilterDialogView);
            //向Dialog发送filters和LabelLibrary
            Messenger.Default.Register<NotificationMessageAction<IList<ParameterFilterProxy>>>(this, Tokens.FilterDialogView, ShowFiltersDialogView);
            this.Unloaded += (o, e) =>
            {
                Messenger.Default.Unregister(this);
                Messenger.Default.Unregister(this.DataContext);
            };
        }

        private void ShowLabelLibraryDialog(IList<string> obj)
        {
            var labelLibrary = SingletonIOC.Current.Container.GetInstanceWithoutCaching<FilterLabelLibrary>();
            var viewmodel = SingletonIOC.Current.Container.GetInstanceWithoutCaching<ParameterFilterLabelLibraryViewModel>();
            viewmodel.Initial(obj);
            labelLibrary.DataContext = viewmodel;
            labelLibrary.ShowDialog();
        }

        private void ShowFiltersDialogView(NotificationMessageAction<IList<ParameterFilterProxy>> obj)
        {
            FilterDialogView filterDialogView = SingletonIOC.Current.Container.GetInstanceWithoutCaching<FilterDialogView>();
            var viewmodel = obj.Target as ParameterFilterLabelDialogViewModel;
            viewmodel.Filters = (obj.Sender as IList<ParameterFilterProxy>).ToList();
            filterDialogView.DataContext = viewmodel;
            filterDialogView.ShowDialog();
            obj.Execute(viewmodel.Filters);
        }

        private void ShowFilterDialogView(NotificationMessageAction<ParameterFilterProxy> obj)
        {
            FilterDialogView filterDialogView = SingletonIOC.Current.Container.GetInstanceWithoutCaching<FilterDialogView>();
            var viewmodel = obj.Target as ParameterFilterLabelDialogViewModel;
            viewmodel.InitialFilter(obj.Sender as ParameterFilterProxy);
            filterDialogView.DataContext = viewmodel;
            filterDialogView.ShowDialog();
            obj.Execute(viewmodel.Filters.FirstOrDefault());
        }

        private void CloseWindow(bool obj)
        {
            this.DialogResult = obj;
        }
    }
}
