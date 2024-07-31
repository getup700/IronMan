using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using IronMan.Revit.Toolkit.Mvvm;
using IronMan.Revit.Toolkit.Mvvm.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms.VisualStyles;
using System.Windows.Interop;

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
        public static TView Resolve<TView, TViewModel>(this IServiceProvider container, bool modeless = false) where TView : Window where TViewModel : class
        {
            //container.Register<TView, TViewModel>();
            UIApplication uiApplication = container.GetRequiredService<IDataContext>().GetUIApplication();
            //单例窗口or瞬时窗口
            //模态窗口为什么用瞬时IOC
            //瞬时ioc不会存储在内存中，相当于每次都是新的实例
            //请求窗口要求是瞬时
            var view = modeless ? container.GetService<TView>() : container.GetService<TView>();//非模态单例or模态瞬时
            //window start location
            //view.Topmost = true;
            if (modeless)
            {
                new WindowInteropHelper(view)
                {
                    //Owner = uiApplication.MainWindowHandle
                };

                //IntPtr revitWindow = Process.GetCurrentProcess().MainWindowHandle;
                //HwndSource hwndSource = HwndSource.FromHwnd(revitWindow);
                //Window revitOpenWindow = hwndSource.RootVisual as Window;
                //view.Owner = revitOpenWindow;

                view.Top = uiApplication.DrawingAreaExtents.Top + 22;
                view.Left = uiApplication.DrawingAreaExtents.Left - 4;
                view.Closing += (o, e) =>
                {
                    e.Cancel = true;
                    view.Hide();
                };

            }
            else
            {
                view.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            }
            //view.DataContext = container.GetInstance<TViewModel>();//瞬时
            view.DataContext = container.GetService<TViewModel>();//瞬时
            view.KeyDown += (o, e) =>
            {
                if ((!modeless) && e.Key == System.Windows.Input.Key.Escape)
                {
                    view.DialogResult = new bool?(false);
                }
                if (modeless && e.Key == System.Windows.Input.Key.Escape)
                {
                    view.Close();
                }
            };
            return view;
        }

        //public static SimpleIoc Register<TView, TViewModel>(this SimpleIoc container) where TView : FrameworkElement where TViewModel : class
        //{
        //    if (container.IsRegistered<TView>() == false)
        //    {
        //        container.Register<TView>();
        //    }
        //    if (container.IsRegistered<TViewModel>() == false)
        //    {
        //        container.Register<TViewModel>();
        //    }
        //    return container;
        //}
    }
}
