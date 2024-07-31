using GalaSoft.MvvmLight;
using IronMan.Revit.Toolkit.Mvvm;
using IronMan.Revit.Toolkit.Mvvm.Interfaces;
using IronMan.Revit.Toolkit.Mvvm.IOC;
using IronMan.Revit.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace IronMan.Revit.ViewModels
{
    public class TransparentViewModel : ViewModelBase
    {
        private readonly IUIProvider _uiProvider;
        private IDataContext _dataContext;

        public TransparentViewModel(IUIProvider uiProvider, IDataContext dataContext)
        {
            _uiProvider = uiProvider;
            _dataContext = dataContext;
            Initial();
        }

        private void Initial()
        {
            //IntPtr intPtr = _uiProvider.GetWindowHandle();
            //Rect rect = new Rect();
            //GetWindowRect(intPtr, ref rect);
            var transparentView = SingletonIOC.Current.Container.GetInstance<TransparentView>();
            transparentView.Show();
        }

        //[DllImport("User32.dll")]
        //[return: MarshalAs(UnmanagedType.Bool)]
        //static extern bool GetWindowRect(IntPtr intPtr, ref Rect rect);
    }
}
