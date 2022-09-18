using Autodesk.Revit.DB;
using IronMan.Revit.Entity;
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
using IronMan.Services;

namespace IronMan.Views
{
    /// <summary>
    /// Materials.xaml 的交互逻辑
    /// </summary>
    public partial class MaterialsWindow : Window
    {
        //private Document _doc;

        public MaterialsWindow(Document document)
        {
            InitializeComponent();
            this.DataContext = new MaterialsViewModel( new MaterialService(new DataContext(document)));
            //注册消息中心
            //this,接收方
            //硬编码：token用"Materials";软编码：Tokens类的MaterialsDialog;
            //CloseWindow,接收后的Action
            Messenger.Default.Register<bool>(this, Contacts.Tokens.MaterialsWindow, CloseWindow);
            Messenger.Default.Register<NotificationMessageAction<MaterialPlus>>(this, Contacts.Tokens.MaterialDialogWindow, ShowMaterialDialog);
            //取消注册消息中心
            this.Unloaded += MaterialsWindow_Unloaded;
        }

        private void ShowMaterialDialog(NotificationMessageAction<MaterialPlus> message)
        {
            MaterialDialogWindow dialogView = new MaterialDialogWindow();     
            MaterialDialogViewModel dialogViewModel = message.Target as MaterialDialogViewModel;
            dialogView.DataContext = dialogViewModel;
            dialogViewModel.Initial(message.Sender);
            
                dialogView.ShowDialog();
            message.Execute(dialogViewModel.MaterialPlus);

            //MessageBox.Show("edit element Materials");
        }

        //取消注册消息中心
        private void MaterialsWindow_Unloaded(object sender, RoutedEventArgs e)
        {
            Messenger.Default.Unregister(this);
        }

        private void CloseWindow(bool result)
        {
            this.DialogResult = result;
        }

    }
}
