///************************************************************************************
///   Author:Tony Stark
///   CretaeTime:2023/3/9 12:32:17
///   Mail:2609639898@qq.com
///   Github:https://github.com/getup700
///
///   Description:
///
///************************************************************************************

using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IronMan.Revit
{
    public interface IRibbonButton
    {
        string Text { get; }
        string LongDescription { get; }
        string ToolTip { get; }
        Bitmap Image { get; }
        Bitmap LargeImage { get; }
        Bitmap ToolTipImage { get; }
        ContextualHelp ContextualHelp { get; }
    }
}
