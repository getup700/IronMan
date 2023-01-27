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
    /// TransparentView.xaml 的交互逻辑
    /// </summary>
    public partial class TransparentView : Window
    {
        public TransparentView()
        {
            InitializeComponent();
            //Messenger.Default.Register<Rect>(this, Contants.Tokens.MainWindowRect, (rect) =>
            //{
            //    this.Top = rect.Top;
            //    this.Left = rect.Left;
            //    this.Width = rect.Width;
            //    this.Height = rect.Height;
            //});
            this.Closing += (o, e) =>
            {
                e.Cancel = true;
                this.Hide();
                Messenger.Default.Unregister(this);
            };
            //this.Unloaded += (o, e) => Messenger.Default.Unregister(this);
            //new WindowInteropHelper(this)
            //{
            //    Owner = uiApplication.MainWindowHandle
            //};
        }
    }
}
