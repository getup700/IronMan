///************************************************************************************
///   Author:Tony Stark
///   CreateTime:2023/5/24 星期三 18:16:13
///   Mail:2609639898@qq.com
///   Github:https://github.com/getup700
///
///   Description:全局处理器，通过故障访问器读取当前的异常，并返回故障处理结果。
///   需要全局注册：application.RegisterFailuresProcessor(IFailuresProcessor processor)
///
///************************************************************************************

using Autodesk.Revit.DB;
using System;

namespace IronMan.Revit.Plugin
{
    internal class FailuresProcessor : IFailuresProcessor
    {
        public void Dismiss(Document document)
        {
            throw new NotImplementedException();
        }

        public FailureProcessingResult ProcessFailures(FailuresAccessor data)
        {
            throw new NotImplementedException();
        }
    }
}
