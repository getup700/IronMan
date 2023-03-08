using Autodesk.Revit.UI;
using GalaSoft.MvvmLight.Messaging;
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
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace IronMan.Revit.Views
{
    /// <summary>
    /// CadToRevitView.xaml 的交互逻辑
    /// </summary>
    public partial class CadToRevitView : Window
    {
        //public static CadToRevitView Current = new CadToRevitView();
        public CadToRevitView()
        {
            InitializeComponent();
            //this.Unloaded += CadToRevitView_Unloaded;
            Messenger.Default.Register<bool>(this, CloseWindow);
            //Button button = new Button();
            //DependencyObject dependencyObject = button as DependencyObject;
            //DependencyObject dependencyObject = new DependencyObject();
            //Button button = (Button)dependencyObject;
        }

        private void CloseWindow(bool obj)
        {
            this.DialogResult = true;
        }

        //private void CadToRevitView_Unloaded(object sender, RoutedEventArgs e)
        //{
        //    this.DialogResult = true;
        //}
    }
}
