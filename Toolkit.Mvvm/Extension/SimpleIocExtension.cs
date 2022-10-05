using Autodesk.Revit.DB;
using GalaSoft.MvvmLight.Ioc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace IronMan.Revit.Toolkit.Mvvm.Extension
{
    public static class SimpleIocExtension
    {
        /// <summary>
        /// Get View 
        /// </summary>
        /// <typeparam name="TView"></typeparam>
        /// <typeparam name="TViewModel"></typeparam>
        /// <param name="container"></param>
        /// <param name="modeless">非模态的可选参数，true为非模态窗口，false为模态窗口</param>
        /// <returns></returns>
        public static TView Resolve<TView, TViewModel>(this SimpleIoc container, bool modeless = false) where TView : Window where TViewModel : class
        {
            //单例窗口or瞬时窗口
            var view = modeless ? container.GetInstance<TView>() : container.GetInstanceWithoutCaching<TView>();//非模态单例or模态瞬时
            view.DataContext = container.GetInstanceWithoutCaching<TViewModel>();//瞬时
            return view;
        }
    }
}
