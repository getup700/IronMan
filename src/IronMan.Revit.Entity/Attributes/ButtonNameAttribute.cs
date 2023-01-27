using Autodesk.Internal.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IronMan.Revit.Entity.Attributes
{
    public class ButtonNameAttribute : Attribute
    {
        public ButtonNameAttribute(string name, string toolTip = null, string description = null)
        {
            Name = name;
            ToolTip = toolTip != null ? toolTip : "Default ToolTip";
            Description = description != null ? description : "Default Description";
        }

        /// <summary>
        /// 功能名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 提示
        /// </summary>
        public string ToolTip { get; set; }

        /// <summary>
        /// 详细描述
        /// </summary>
        public string Description { get; set; }

    }
}
