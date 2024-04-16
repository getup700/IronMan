using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Messaging;
using IronMan.Revit.IServices;
using IronMan.Revit.Toolkit.Mvvm.Extension;
using IronMan.Revit.Toolkit.Mvvm.IOC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IronMan.Revit.Services
{
    public class ProgressBarService : IProgressBarService
    {
        Views.ProgressBarDialog _dialog = null;
        public void Start(int maximum)
        {
            //var dialog = SingletonIOC.Current.Container.GetInstanceWithoutCaching<Views.ProgressBarDialog>();
            // dialog.DataContext = SingletonIOC.Current.Container.GetInstanceWithoutCaching<ViewModels.ProgerssBarDialogViewModel>();
            _dialog = SingletonIOC.Current.Container.Resolve<Views.ProgressBarDialog, ViewModels.ProgerssBarDialogViewModel>();
            _dialog.Show();
            Messenger.Default.Send<int>(maximum,Contants.Tokens.ProgressBarMaximum);
        }

        public void Stop()
        {
            Messenger.Default.Unregister(_dialog.DataContext);
            _dialog.Close();
        }
    }
}
