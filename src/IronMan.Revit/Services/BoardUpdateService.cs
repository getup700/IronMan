using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;

namespace IronMan.Revit.Services
{
    internal class BoardUpdateService
    {
        HwndSource hwndSource;
        IntPtr hwnd;
        int WM_CLIPBOARDUPDATE = 0x031D;

        [DllImport("user32.dll")]
        private static extern int AddClipFormatListener(IntPtr inPtr);

        [DllImport("user32.dll")]
        private static extern int RemoveClipFormatListener(IntPtr inPtr);

        /// <summary>
        /// 开始监听
        /// </summary>
        /// <param name="window"></param>
        public void Add(Window window)
        {
            hwnd = new WindowInteropHelper(window).Handle;
            hwndSource = HwndSource.FromHwnd(hwnd);
            hwndSource.AddHook(PressMethod);
            AddClipFormatListener(hwnd);
        }

        /// <summary>
        /// 结束监听
        /// </summary>
        public void Remove()
        {
            RemoveClipFormatListener(hwnd);
            hwndSource.RemoveHook(PressMethod);
        }

        /// <summary>
        /// 窗口消息的事件处理程序
        /// </summary>
        /// <param name="hwnd"></param>
        /// <param name="msg"></param>
        /// <param name="wParam"></param>
        /// <param name="lParam"></param>
        /// <param name="handled"></param>
        /// <returns></returns>
        private IntPtr PressMethod(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            //剪贴板的内容发生更改时发送
            if (msg == WM_CLIPBOARDUPDATE)
            {
                if (Clipboard.ContainsText())
                {
                    //获取监听期间剪贴板的数据
                    var text = Clipboard.GetText();
                    //写入数据到剪贴板
                    Clipboard.SetDataObject("66666");
                }
            }
           return IntPtr.Zero;
        }
    }
}
