using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IronMan.Revit.Entity
{
    public class LevelProxy : ElementProxy
    {
        private Level _level;
        public LevelProxy(Element element) : base(element)
        {
            _level = element as Level;
        }


    }
}
