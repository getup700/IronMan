using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IronMan.Revit.Entity
{
    public class WindowProxy : ElementProxy
    {
        public WindowProxy(Element element) : base(element)
        {

        }

        public string TypeName { get; set; }

        public XYZ Location { get; set; }

        public Wall Host { get; set; }
    }
}
