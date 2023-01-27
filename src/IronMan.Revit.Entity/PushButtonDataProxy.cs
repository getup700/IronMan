using Autodesk.Internal.Windows;
using Autodesk.Revit.UI;
using IronMan.Revit.Entity.Extensions;
using IronMan.Revit.Entity.Properties;
using IronMan.Revit.Toolkit.Extension;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace IronMan.Revit.Entity
{
    public class PushButtonDataProxy
    {
        private Type _type;
        private string _inName = Guid.NewGuid().ToString();
        private string _buttonName = "buttonName";
        private string _nameSpace;
        private string _assemblyName;
        private string _tooltip;
        private Bitmap _imageName = Resources.PushButton32;
        private Bitmap _stackedImageName = Resources.PushButton16;
        private Bitmap _tooltipImage =Resources.PushButton16;
        private string _description;

        public PushButtonDataProxy(Type type)
        {
            Type = type;
        }

        #region Properties
        public Type Type
        {
            get { return _type; }
            set
            {
                _type = value;
                if (Type != null)
                {
                    NameSpace = Type.FullName;
                    AssemplyName = Type.Assembly.Location;
                    ButtonName = Type.GetMethodNameByAttribute();
                    ToolTip = Type.GetMethodTooltipByAttribute();
                }
            }
        }

        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        public string InName
        {
            get { return _inName; }
            set { _inName = value; }
        }

        public string ButtonName
        {
            get { return _buttonName; }
            set { _buttonName = value; }
        }

        public string NameSpace
        {
            get { return _nameSpace; }
            private set { _nameSpace = value; }
        }

        public string AssemplyName
        {
            get { return _assemblyName; }
            private set { _assemblyName = value; }
        }

        public string ToolTip
        {
            get { return _tooltip; }
            set { _tooltip = value; }
        }

        public Bitmap ImageName
        {
            get { return _imageName; }
            set { _imageName = value; }
        }

        public Bitmap StackedImageName
        {
            get { return _stackedImageName; }
            set { _stackedImageName = value; }
        }

        public Bitmap TooltipImage
        {
            get { return _tooltipImage; }
            set { _tooltipImage = value; }
        }

        #endregion

        #region Methods
        public PushButtonData ConvertRevitButton()
        {
            var buttonData = new PushButtonData(this.InName, this.ButtonName, this.AssemplyName, this.NameSpace);
            try
            {
                buttonData.ToolTip = this.ToolTip;
                buttonData.LongDescription = this._description;
                buttonData.SetContextualHelp(new ContextualHelp(ContextualHelpType.Url, @"www.baidu.com"));
                if (ImageName != null)
                {
                    buttonData.LargeImage = this.ImageName.ConvertToBitmapSource();
                }
                if(StackedImageName != null)
                {
                    buttonData.Image = this.StackedImageName.ConvertToBitmapSource();
                }
                if(TooltipImage != null)
                {
                    buttonData.ToolTipImage = this._tooltipImage.ConvertToBitmapSource();
                }
            }
            catch (Exception e)
            {
                throw new Exception($"Error:Button add failure\t{e.Message}\t{e.StackTrace}");
            }
            
            return buttonData;
        }

        #endregion
    }
}
