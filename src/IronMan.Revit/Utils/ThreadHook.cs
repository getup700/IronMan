using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IronMan.Revit.Utils
{
    public class ThreadHook
    {
        //低级别键盘钩子
        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x0100;
        private const int WM_SYSKEYDOWN = 0x0104;

        private LowLevelKeyboardProc _proc;
        private IntPtr _hookID = IntPtr.Zero;

        private GCHandle _gcHandle;

        /// <summary>
        /// 与 SetWindowsHookExA SetWindowsHookExW函数一起使用的应用程序定义或库定义的回调函数。
        /// 每当新的键盘输入事件即将发布到线程输入队列时，系统都会调用此函数。
        /// </summary>
        /// <param name="nCode">挂钩过程用于确定如何处理消息的代码。
        /// 如果 nCode 小于零，则挂钩过程必须将消息传递给 CallNextHookEx 函数，而无需进一步处理，并且应返回 CallNextHookEx 返回的值。</param>
        /// <param name="wParam">键盘消息的标识符</param>
        /// <param name="lParam">指向 KBDLLHOOKSTRUCT 结构的指针</param>
        /// <returns></returns>
        public delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

        public void Run(LowLevelKeyboardProc lowLevelKeyboardProc)
        {
            _hookID = SetHook(lowLevelKeyboardProc);
            MSG msg;
            while (GetMessage(out msg, IntPtr.Zero, 0, 0))
            {
                TranslateMessage(ref msg);
                DispatchMessage(ref msg);
            }
        }

        public void RunExample()
        {
            _proc = PrintKeyValue;
            _gcHandle = GCHandle.Alloc(_proc);
            _hookID = SetHook(_proc);
            //MSG msg;
            //while (GetMessage(out msg, IntPtr.Zero, 0, 0))
            //{
            //    TranslateMessage(ref msg);dddddd
            //    DispatchMessage(ref msg);
            //}
        }

        /// <summary>
        /// 安装钩子de
        /// </summary>
        /// <param name="proc"></param>
        /// <returns></returns>
        private IntPtr SetHook(LowLevelKeyboardProc proc)
        {
            using Process curProcess = Process.GetCurrentProcess();
            using ProcessModule curModule = curProcess.MainModule;
            uint revitMainThreadId = 0;

            foreach (ProcessThread thread in curProcess.Threads)
            {
                if (thread.ThreadState == ThreadState.Running)
                {
                    revitMainThreadId = (uint)thread.Id;
                    break;
                }
            }
            var hMod = GetModuleHandle(curModule.ModuleName);
            return SetWindowsHookExA(WH_KEYBOARD_LL, proc, hMod, revitMainThreadId);
        }

        /// <summary>
        /// 按键处理回调。示例：打印按键值，并附带hook字符。
        /// </summary>
        /// <param name="nCode"></param>
        /// <param name="wParam"></param>
        /// <param name="lParam"></param>
        /// <returns></returns>
        public IntPtr PrintKeyValue(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0 && (wParam == (IntPtr)WM_KEYDOWN))
            {
                var vkCode = Marshal.ReadInt32(lParam);
                var key = (Keys)vkCode;
                Console.WriteLine($"{key}hook");
                MessageBox.Show(key.ToString());
            }
            return CallNextHookEx(_hookID, nCode, wParam, lParam);
        }

        /// <summary>
        /// 卸载钩子
        /// </summary>
        public void UninstallHook()
        {
            UnhookWindowsHookEx(_hookID);
            if (_gcHandle.IsAllocated)
            {
                _gcHandle.Free();
            }
        }

        /// <summary>
        /// 将应用程序定义的挂钩过程安装到挂钩链中。 你将安装挂钩过程来监视系统的某些类型的事件。
        /// 这些事件与特定线程或与调用线程位于同一桌面中的所有线程相关联。
        /// </summary>
        /// <param name="idHook">要安装的挂钩过程的类型</param>
        /// <param name="lpfn">指向挂钩过程的指针。 如果 dwThreadId 参数为零或指定由其他进程创建的线程的标识符， 
        /// 则 lpfn 参数必须指向 DLL 中的挂钩过程。否则， lpfn 可以指向与当前进程关联的代码中的挂钩过程。</param>
        /// <param name="hMod">DLL 的句柄，其中包含 lpfn 参数指向的挂钩过程。 如果 dwThreadId 参数指定当前进程创建的线程，
        /// 并且挂钩过程位于与当前进程关联的代码中，则必须将 hMod 参数设置为 NULL。</param>
        /// <param name="dwThreadId">要与挂钩过程关联的线程的标识符。 
        /// 对于桌面应用，如果此参数为零，则挂钩过程与调用线程在同一桌面中运行的所有现有线程相关联。</param>
        /// <returns>如果函数成功，则返回值是挂钩过程的句柄。</returns>
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookExA(int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

        /// <summary>
        /// 删除 SetWindowsHookEx 函数安装在挂钩链中的挂钩过程。
        /// </summary>
        /// <param name="hhk"></param>
        /// <returns></returns>
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        /// <summary>
        /// 将挂钩信息传递给当前挂钩链中的下一个挂钩过程。 挂钩过程可以在处理挂钩信息之前或之后调用此函数。
        /// </summary>
        /// <param name="hhk"></param>
        /// <param name="nCode">传递给当前挂钩过程的挂钩代码。 下一个挂钩过程使用此代码来确定如何处理挂钩信息。</param>
        /// <param name="wParam">传递给当前挂钩过程的 wParam 值。 此参数的含义取决于与当前挂钩链关联的挂钩类型。</param>
        /// <param name="lParam">传递给当前挂钩过程的 lParam 值。 此参数的含义取决于与当前挂钩链关联的挂钩类型。</param>
        /// <returns></returns>
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        /// <summary>
        /// 从调用线程的消息队列中检索消息。 函数调度传入的已发送消息，直到已发布的消息可供检索。
        /// GetMessage 函数类似于 PeekMessage，但是， GetMessage 会阻止，直到发布消息后再返回。
        /// </summary>
        /// <param name="lpMsg">指向 MSG 结构的指针，该结构从线程的消息队列接收消息信息。</param>
        /// <param name="hWnd">要检索其消息的窗口的句柄。 窗口必须属于当前线程。</param>
        /// <param name="wMsgFilterMin">要检索的最低消息值的整数值。 使用 WM_KEYFIRST (0x0100) 指定第一条键盘消息， 
        /// 或使用WM_MOUSEFIRST (0x0200) 指定第一条鼠标消息。</param>
        /// <param name="wMsgFilterMax">要检索的最高消息值的整数值。 使用 WM_KEYLAST 指定最后一条键盘消息，
        /// WM_MOUSELAST 指定最后一条鼠标消息。</param>
        /// <returns>如果函数检索 WM_QUIT以外的消息，则返回值为非零值。</returns>
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool GetMessage(out MSG lpMsg, IntPtr hWnd, uint wMsgFilterMin, uint wMsgFilterMax);

        /// <summary>
        /// 将虚拟密钥消息转换为字符消息。 字符消息将发布到调用线程的消息队列，以便下次线程调用 GetMessage 或 PeekMessage 函数时读取。
        /// </summary>
        /// <param name="lpMsg"></param>
        /// <returns></returns>
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool TranslateMessage(ref MSG lpMsg);

        /// <summary>
        /// 将消息调度到窗口过程。 它通常用于调度 GetMessage 函数检索到的消息。
        /// </summary>
        /// <param name="lpMsg"></param>
        /// <returns></returns>
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr DispatchMessage(ref MSG lpMsg);

        /// <summary>
        /// 检索指定模块的模块句柄。 模块必须已由调用进程加载。
        /// </summary>
        /// <param name="lpModuleName"></param>
        /// <returns></returns>
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);

        //public void Dispose()
        //{
        //    UninstallHook();
        //}

        private struct MSG
        {
            public IntPtr hwnd;
            public uint message;
            public IntPtr wParam;
            public IntPtr lParam;
            public uint time;
            public POINT pt;
        }

        private struct POINT
        {
            public int x;
            public int y;
        }

    }
}
