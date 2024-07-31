using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IronMan.Revit.Geometry
{
    public class WS
    {
        public Line BL1, BL2;
        public List<int> KnotIndexs;
        public double X = 0;
        public double Y = 0;

        public WS()
        {
            KnotIndexs = new List<int>();
        }

        public WS(Line BL1, Line BL2)
        {
            this.BL1 = BL1;
            this.BL2 = BL2;
            KnotIndexs = new List<int>();
        }
    }
}
