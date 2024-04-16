using IronMan.Revit.Toolkit.Mvvm.IOC;
using IronMan.Revit.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
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
        private ThreadHook _hook;
        [DllImport("user32.dll")]
        public static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, uint dwExtraInfo);
        private const byte F1 = 0x70;
        private const byte F2 = 0x71;
        private const uint KEYEVENTF_KEYDOWN = 0x0000;
        private const uint KEYEVENTF_KEYUP = 0x0002;

        public ModeView()
        {
            InitializeComponent();
            Messenger.Default.Register<bool>(this, Revit.Contants.Tokens.CloseWindow, (obj) => { this.DialogResult = obj; }) ;
            this.Unloaded += (s, e) => Messenger.Default.Unregister(this);
            //_hook = SingletonIOC.Current.Container.GetInstance<ThreadHook>();
            //_hook.SetHook();
            //this.PreviewKeyDown += (s, e) =>
            //{
            //    if (e.Key == System.Windows.Input.Key.F1)
            //    {
            //        //MessageBox.Show("View catch F1");
            //        keybd_event(F2, 0, KEYEVENTF_KEYDOWN, 0);
            //    }
            //};
            keybd_event(F2, 0, KEYEVENTF_KEYDOWN, 0);
        }
        
        //private void CloseWindow(bool obj)
        //{
        //    this.DialogResult = obj;
        //    this.Close();
        //    _hook.UnHook();
        //}
    }
}
