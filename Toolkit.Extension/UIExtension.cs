using Autodesk.Revit.UI;
using System;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Toolkit.Extension
{
    public static class UIExtension
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="panel"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static RibbonPanel CreateButton<T>(this RibbonPanel panel,Action<PushButtonData>action)
        {
            Type type = typeof(T);
            string name =$"btn_{type.Name}";
            PushButtonData pushButtonData = new PushButtonData(name,name,type.Assembly.Location,type.FullName);
            action.Invoke(pushButtonData);
            panel.AddItem(pushButtonData);
            return panel;
        }
        public static void CreatePulldownButton<T>(this RibbonPanel panel, Action<PushButtonData> action)
        {
            Type type = typeof(T);
            string name = "btn_" + type.Name;
            PulldownButtonData data = new PulldownButtonData(name, name);
        }
        /// <summary>
        /// 把位图扩展为BitmapSource
        /// </summary>
        /// <param name="bitmap"></param>
        /// <returns></returns>
        public static BitmapSource ConvertToBitmapSource(this System.Drawing.Bitmap bitmap)
        {
            return System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                bitmap.GetHbitmap(),
                IntPtr.Zero,
                Int32Rect.Empty,
                BitmapSizeOptions.FromEmptyOptions());
        }
    }
}
