using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using Org.BouncyCastle.Bcpg.OpenPgp;
using System;
using System.Collections.Generic;
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
        /// 创建普通按钮
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="panel"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static RibbonPanel CreateButton<T>(this RibbonPanel panel, Action<PushButtonData> action) where T : IExternalCommand
        {
            Type type = typeof(T);
            string name = $"btn_{type.Name}";
            PushButtonData pushButtonData = new PushButtonData(name, name, type.Assembly.Location, type.FullName);

            //判断命令是否实现了IExternalCommandAvailability接口
            if (type.GetInterface("IExternalCommandAvailability")!=null)
            {
                pushButtonData.AvailabilityClassName = type.FullName;
            }

            action.Invoke(pushButtonData);
            RibbonItem ribbonItem = panel.AddItem(pushButtonData);
            return panel;
        }



        /// <summary>
        /// 创建竖向紧凑布置的堆叠按钮
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <param name="panel"></param>
        /// <param name="action1"></param>
        /// <param name="action2"></param>
        /// <param name="action3"></param>
        /// <returns></returns>
        public static RibbonPanel CreateStackedItems<T1, T2, T3>(this RibbonPanel panel, Action<PushButtonData> action1, Action<PushButtonData> action2, Action<PushButtonData> action3)
            where T1 : IExternalCommand where T2 : IExternalCommand where T3 : IExternalCommand
        {
            Type type1 = typeof(T1);
            string name1 = $"stackedBtn_{type1.Name}";
            PushButtonData pushButtonData1 = new PushButtonData(name1, name1, type1.Assembly.Location, type1.FullName);
            action1.Invoke(pushButtonData1);

            Type type2 = typeof(T2);
            string name2 = $"stackedBtn_{type2.Name}";
            PushButtonData pushButtonData2 = new PushButtonData(name2, name2, type2.Assembly.Location, type2.FullName);
            action2.Invoke(pushButtonData2);

            Type type3 = typeof(T3);
            string name3 = $"stackedBtn_{type3.Name}";
            PushButtonData pushButtonData3 = new PushButtonData(name3, name3, type3.Assembly.Location, type3.FullName);
            action3.Invoke(pushButtonData3);

            panel.AddStackedItems(pushButtonData1, pushButtonData2, pushButtonData3);
            return panel;
        }

        /// <summary>
        /// 创建普通按钮
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="action"></param>
        /// <returns></returns>
        public static PushButtonData CreatePushButtonData<T>(Action<PushButtonData> action)
        {
            Type type = typeof(T);
            string name = $"btn_{type.Name}";
            PushButtonData pushButtonData = new PushButtonData(name, name, type.Assembly.Location, type.FullName);
            action.Invoke(pushButtonData);
            return pushButtonData;
        }

        /// <summary>
        /// 创建下拉按钮
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="panel"></param>
        /// <param name="action">construct a pullDownButtonData</param>
        /// <returns></returns>
        public static PulldownButton CreatePulldownButton(this RibbonPanel panel, Action<PulldownButtonData> action)
        {
            var data = new PulldownButtonData("default","default");
            //Type type = typeof(T);
            //string name = "pullDownBtn_" + type.Name;
            action.Invoke(data);
            var pulldownButton = panel.AddItem(data) as PulldownButton;
            
            //foreach (var pushButtonData in pushButtonDatas)
            //{
            //    pulldownButton.AddPushButton(pushButtonData);
            //}
            return pulldownButton;
        }

        /// <summary>
        /// 创建SplitButton
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="panel"></param>
        /// <param name="function"></param>
        /// <returns></returns>
        public static SplitButton CreateSplitButton<T>(this RibbonPanel panel, Action<SplitButtonData> action) where T : IExternalCommand
        {
            var data = new SplitButtonData("default", "default");
            action.Invoke(data);

            SplitButton splitButton = panel.AddItem(data) as SplitButton;
            return splitButton;
        }

        /// <summary>
        /// 创建TextBox
        /// </summary>
        /// <param name="panel"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static TextBox CreateTextBox(this RibbonPanel panel, Action<TextBoxData> action)
        {
            TextBoxData textBoxData = new TextBoxData("TextBox");
            action.Invoke(textBoxData);
            TextBox textBox = panel.AddItem(textBoxData) as TextBox;
            return textBox;
        }

        /// <summary>
        /// 创建ComboBox
        /// </summary>
        /// <param name="panel"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static ComboBox CreateComboBox(this RibbonPanel panel, Action<ComboBoxData> action)
        {
            ComboBoxData comboBoxData = new ComboBoxData("ComboBox");
            action.Invoke(comboBoxData);
            ComboBox comboBox = panel.AddItem(comboBoxData) as ComboBox;
            return comboBox;
        }

        public static RadioButtonGroup CreateRadioButtonGroup(this RibbonPanel panel)
        {
            RadioButtonGroupData radioButtonGroupData = new RadioButtonGroupData("RadioButtonGroup");
            RadioButtonGroup radioButtonGroup = panel.AddItem(radioButtonGroupData) as RadioButtonGroup;
            return radioButtonGroup;
        }

        public static RadioButtonGroup AddToggleButtonData<T>(this RadioButtonGroup radioButtonGroup, Action<ToggleButtonData> action) where T : IExternalCommand
        {
            Type type = typeof(T);
            var data = new ToggleButtonData("default", "DefaultName", type.Assembly.Location, type.FullName);
            action.Invoke(data);
            var result = radioButtonGroup.AddItem(data);
            return radioButtonGroup;
        }

        public static ToggleButton CreateToggleButton<T>(this RibbonPanel panel, Action<ToggleButtonData> action) where T : IExternalCommand
        {
            Type type = typeof(T);
            string uniqueName = "ToggleButton" + type.Name;
            ToggleButtonData toggleButtonData = new ToggleButtonData(uniqueName, "ButtonName", type.Assembly.Location, type.FullName);
            action.Invoke(toggleButtonData);
            ToggleButton toggleButton = panel.AddItem(toggleButtonData) as ToggleButton;
            return toggleButton;
        }

        public static void AddPanel(this UIControlledApplication uiApp, string tabName, string panelName, Action<RibbonPanel> action)
        {
            RibbonPanel ribbonPanel = uiApp.CreateRibbonPanel(tabName, panelName);
            action.Invoke(ribbonPanel);
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
            var bitmapImage = new BitmapImage();
            using (MemoryStream stream = new MemoryStream())
            {
                image.Save(stream, ImageFormat.Png);
                stream.Position = 0;
                bitmapImage.BeginInit();
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.UriSource = null;
                bitmapImage.StreamSource = stream;
                bitmapImage.EndInit();
            }
            return bitmapImage;
        }
    }
}
