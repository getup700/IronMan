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
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace IronMan.Revit.Views
{
    /// <summary>
    /// TestView.xaml 的交互逻辑
    /// </summary>
    public partial class ModeView : Window
    {
        public ModeView()
        {
            InitializeComponent();
            Messenger.Default.Register<bool>(this, Revit.Contants.Tokens.CloseWindow, CloseWindow);
            this.Unloaded += (s, e) => Messenger.Default.Unregister(this);
        }

        private void CloseWindow(bool obj)
        {
            this.DialogResult = obj;
            this.Close();
        }
    }
}
