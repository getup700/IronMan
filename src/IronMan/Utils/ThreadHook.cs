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
        Keyboard = 2   //键盘钩子类型
    }

    //定义委托,该委托的参数是固定这样写
    internal delegate IntPtr HookProc(int code, IntPtr wparam, IntPtr lparam);

    public class ThreadHook
    {
        //记录下一个Hook编号
        static IntPtr _nextHookPtr;

        //获取当前线程编号
        [DllImport("kernel32.dll")]
        static extern int GetCurrentThreadId();

        //卸载钩子
        [DllImport("User32.dll")]
        internal extern static void UnhookWindowsHookEx(IntPtr handle);

        //安装钩子
        [DllImport("User32.dll")]
        internal extern static IntPtr SetWindowsHookEx(int idHook, [MarshalAs(UnmanagedType.FunctionPtr)] HookProc Ipfn, IntPtr hanstance, int threadID);

        //获取下一个钩子
        [DllImport("User32.dll")]
        internal extern static IntPtr CallNextHookEx(IntPtr handle, int code, IntPtr wparam, IntPtr Iparam);

        //委托回调的方法
        IntPtr MyHookProc(int code, IntPtr wparam, IntPtr Iparam)
        {
            if (code < 0)
            {
                //让后面的程序处理该消息
                return CallNextHookEx(_nextHookPtr, code, wparam, Iparam);
            }
            //用户输入的是b或者B
            if (wparam.ToInt32() == 98 || wparam.ToInt32() == 66||wparam.ToInt32()==112)
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
            HookProc myhookProc = new HookProc(MyHookProc);
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
