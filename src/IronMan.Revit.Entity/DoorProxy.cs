using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IronMan.Revit.Entity
{
    public class DoorProxy
    {
        public string TypeName { get;set;}

        public Wall Host { get;set; }

        public bool FilpHand { get; set; }

        public bool FilpFace { get; set; }
    }
}
