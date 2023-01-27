using Autodesk.Revit.UI;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;

namespace IronMan.Revit.Toolkit.Extension
{
    public static class UIExtension
    {
        /// <summary>
        /// 创建按钮
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="panel"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static RibbonPanel CreateButton<T>(this RibbonPanel panel, Action<PushButtonData> action) where T: IExternalCommand
        {
            Type type = typeof(T);
            string name = $"btn_{type.Name}";
            PushButtonData pushButtonData = new PushButtonData(name, name, type.Assembly.Location, type.FullName);
            action.Invoke(pushButtonData);
            panel.AddItem(pushButtonData);
            return panel;
        }

        /// <summary>
        /// 创建下拉按钮
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="panel"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static RibbonPanel CreatePulldownButton<T>(this RibbonPanel panel, Action<PulldownButtonData> action) where T: IExternalCommand
        {
            Type type = typeof(T);
            string name = "pullDownBtn_" + type.Name;
            PulldownButtonData data = new PulldownButtonData(name, name);
            action.Invoke(data);
            panel.AddItem(data);
            return panel;
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

        public static BitmapSource ConvertToBSource(this Image image)
        {
            var bitmapImage=new BitmapImage();
            using (MemoryStream stream = new MemoryStream())
            {
                image.Save(stream, ImageFormat.Png);
                stream.Position = 0;
                bitmapImage.BeginInit();
                bitmapImage.CacheOption= BitmapCacheOption.OnLoad;
                bitmapImage.UriSource = null;
                bitmapImage.StreamSource= stream;
                bitmapImage.EndInit();
            }
            return bitmapImage;
        }
    }
}
