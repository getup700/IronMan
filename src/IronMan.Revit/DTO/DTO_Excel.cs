using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IronMan.Revit.DTO
{
    public class DTO_Excel
    {
        public string FileName { get; set; } = "IronMan Export Excel";

        public string SheetName { get; set; } = "Default Name";

        public int StartRow { get; set; } = 0;

        public int RowCount { get; set; } = 1;


    }
}
