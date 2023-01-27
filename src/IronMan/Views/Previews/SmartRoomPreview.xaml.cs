using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
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

namespace IronMan.Revit.Views.Previews
{
    /// <summary>
    /// SmartRoomPreview.xaml 的交互逻辑
    /// </summary>
    public partial class SmartRoomPreview : Window
    {
        public SmartRoomPreview(Document document)
        {
            InitializeComponent();
            PreviewControl previewControl = new PreviewControl(document, document.ActiveView.Id);
            this.grid.Children.Add(previewControl);
        }
    }
}
