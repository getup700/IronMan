using GalaSoft.MvvmLight.Messaging;
using IronMan.Revit.Entity.Profiles;
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
    /// FilterLabelLibrary.xaml 的交互逻辑
    /// </summary>
    public partial class FilterLabelLibrary : Window
    {
        public FilterLabelLibrary()
        {
            InitializeComponent();
            Messenger.Default.Register<IList<string>>(this, Contants.Tokens.CloseFilterLabelLibraryView, CloseWindow);
            this.Unloaded += (o, e) =>
            {
                Messenger.Default.Unregister(this);
            };
            
        }

        private void CloseWindow(IList<string> obj)
        {
            this.DialogResult = true;
            this.Close();
        }
    }
}
