///************************************************************************************
///   Author:Tony Stark
///   CretaeTime:2023/3/1 10:54:30
///   Mail:2609639898@qq.com
///   Github:https://github.com/getup700
///
///   Description:
///
///************************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IronMan.Revit.Utils
{
    internal enum HookType
    {
        Keyboard = 13   //键盘钩子类型
    }

    //定义委托,该委托的参数是固定这样写
    internal delegate IntPtr HookProc(int code, IntPtr wparam, IntPtr lparam);

    public class ThreadHook1
    {
        //记录下一个Hook编号
        private static IntPtr _nextHookPtr = IntPtr.Zero;

        //获取当前线程编号
        [DllImport("kernel32.dll")]
        private static extern int GetCurrentThreadId();

        //卸载钩子
        [DllImport("User32.dll")]
        private extern static void UnhookWindowsHookEx(IntPtr handle);

        /// <summary>
        /// 安装钩子
        /// </summary>
        /// <param name="idHook">指定钩子类型</param>
        /// <param name="Ipfn">定义钩子的回调逻辑</param>
        /// <param name="hanstance">包含钩子过程所在模块的句柄。如果将钩子过程与应用程序的当前模块相关联，
        /// 则这个参数应该是模块的句柄（通常通过 GetModuleHandle 获取）。
        /// 对于全局钩子，此参数必须是非 NULL 值。</param>
        /// <param name="threadID">指定与安装的钩子过程相关联的线程的标识符。如果此参数为 0，则钩子过程与所有线程相关联，即全局钩子。</param>
        /// <returns></returns>
        [DllImport("User32.dll")]
        private extern static IntPtr SetWindowsHookEx(int idHook, [MarshalAs(UnmanagedType.FunctionPtr)] HookProc Ipfn, IntPtr hanstance, int threadID);

        //获取下一个钩子
        [DllImport("User32.dll")]
        private extern static IntPtr CallNextHookEx(IntPtr handle, int code, IntPtr wparam, IntPtr Iparam);

        //
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);

        //委托回调的方法
        public IntPtr MyHookProc(int code, IntPtr wparam, IntPtr Iparam)
        {
            if (code < 0)
            {
                //让后面的程序处理该消息
                return CallNextHookEx(_nextHookPtr, code, wparam, Iparam);
            }
            //用户输入的是b或者B
            if (wparam.ToInt32() == 98 || wparam.ToInt32() == 66 || wparam.ToInt32() == 112)
            {
                //设置文本输入框为a
                MessageBox.Show("我拦截到B或者b");
                //该消息结束
                return (IntPtr)112;
            }
            else
            {
                //让后面的程序处理该消息
                return IntPtr.Zero;
            }

        }

        /// <summary>
        /// 从外部调用设置钩子
        /// </summary>
        public void SetHook()
        {
            //已经设置过钩子了，不能重复设置
            if (_nextHookPtr != IntPtr.Zero)
            {
                return;
            }
            //设置钩子委托回调函数(委托方法注册)
            var myhookProc = new HookProc(MyHookProc);
            //把该钩子加到Hook链中
            _nextHookPtr = SetWindowsHookEx((int)HookType.Keyboard, myhookProc, IntPtr.Zero, GetCurrentThreadId());
        }

        /// <summary>
        /// 从外部调用卸载钩子
        /// </summary>
        public void UnHook()
        {
            if (_nextHookPtr != IntPtr.Zero)
            {
                //从Hook链中取消
                UnhookWindowsHookEx(_nextHookPtr);
            }
            _nextHookPtr = IntPtr.Zero;
        }
    }

}
