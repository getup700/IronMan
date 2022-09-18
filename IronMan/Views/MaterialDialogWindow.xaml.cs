using GalaSoft.MvvmLight.Messaging;
using IronMan.IServices;
using IronMan.ViewModels;
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

namespace IronMan.Views
{
    /// <summary>
    /// MaterialDialog.xaml 的交互逻辑
    /// </summary>
    public partial class MaterialDialogWindow : Window
    {
        
        public MaterialDialogWindow( )
        {
            InitializeComponent();
            Messenger.Default.Register<bool>(this, Contacts.Tokens.MaterialDialogWindow, CloseWindow);
            this.Unloaded += (o, e) => { Messenger.Default.Unregister(this); };
        }

        private void CloseWindow(bool obj)
        {
            this.DialogResult = obj;
            this.Close();
        }
    }
}
