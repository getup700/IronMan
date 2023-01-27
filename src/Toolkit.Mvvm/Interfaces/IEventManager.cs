using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IronMan.Revit.Toolkit.Mvvm.Interfaces
{
    /// <summary>
    /// 注册事件
    /// 每个插件的UI和事件管理都不一样，故在功能中实现
    /// </summary>
    public interface IEventManager
    {
        void Subscribe();

        void Unsubscribe();
    }
}
