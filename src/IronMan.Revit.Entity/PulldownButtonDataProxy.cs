using Autodesk.Revit.UI;
using IronMan.Revit.Entity.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace IronMan.Revit.Entity
{
    public class PullDownButtonDataProxy
    {
        private Type _type;
        public Type Type
        {
            get { return _type; }
            set
            {
                _type = value;
                if (Type != null)
                {
                    //NameSpace = Type.FullName;
                    //AssemplyName = Type.Assembly.Location;
                    //_buttonName = Type.GetAttributeByMethodName();
                    ToolTip = Type.GetMethodTooltipByAttribute();
                }
            }
        }

        public string LongDescription { get; set; }

        public string Name { get; set; }

        public string Text { get; set; }

        public string ToolTip { get; set; }

        public ImageSource Image { get; set; }

        public ImageSource LargeImage { get; set; }

        public ImageSource ToolTipImage { get; set; }


        public PulldownButtonData ConvertRevitButton()
        {
            PulldownButtonData pulldownButtonData;
            try
            {
                pulldownButtonData = new PulldownButtonData(this.Name, this.Text)
                {
                    LongDescription = this.LongDescription,
                    ToolTip = this.ToolTip,
                    Image = this.Image,
                    LargeImage = this.LargeImage,
                    ToolTipImage = this.ToolTipImage,
                };
            }
            catch (Exception e)
            {
                throw new Exception($"Error:Button add failure\t{e.Message}\t{e.StackTrace}");
            };
            return pulldownButtonData;
        }
    }
}
