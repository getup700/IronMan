///************************************************************************************
///   Author:Tony Stark
///   CretaeTime:2023/3/8 22:06:46
///   Mail:2609639898@qq.com
///   Github:https://github.com/getup700
///
///   Description:
///
///************************************************************************************

using Autodesk.Revit.UI;
using IronMan.Revit.Toolkit.Mvvm.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IronMan.Revit
{
    internal class RibbonUIManager
    {
        private readonly IUIProvider _uiProvider;
        private readonly string _tabName;
        private List<RibbonPanel> _ribbonPanels = new List<RibbonPanel>();

        public RibbonUIManager(IUIProvider uiProvider)
        {
            _uiProvider = uiProvider;
        }

        public void AddPanel(string panelName, Action<RibbonPanel> action)
        {
            RibbonPanel ribbonPanel = _uiProvider.GetUIApplication().CreateRibbonPanel(_tabName, panelName);
            _ribbonPanels.Add(ribbonPanel);
            action.Invoke(ribbonPanel);
        }

    }
}
