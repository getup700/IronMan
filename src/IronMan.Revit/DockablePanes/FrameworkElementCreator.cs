using Autodesk.Revit.UI;
using IronMan.Revit.Toolkit.Mvvm.IOC;
using IronMan.Revit.ViewModels;
using IronMan.Revit.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace IronMan.Revit.DockablePanes
{
    public class FrameworkElementCreator : IFrameworkElementCreator
    {
        private DockablePaneControlViewModel _viewModel;
        private DockablePaneControlView _view;

        public FrameworkElementCreator()
        {
            _viewModel = SingletonIOC.Current.Container.GetInstance<DockablePaneControlViewModel>();
            _view = SingletonIOC.Current.Container.GetInstance<DockablePaneControlView>();
        }

        public FrameworkElement CreateFrameworkElement()
        {
            DockablePaneControlView dockablePaneControl = _view;
            _view.DataContext = _viewModel;
            //{
            //    DataContext = _viewModel
            //};
            return dockablePaneControl;
        }
    } 
}
