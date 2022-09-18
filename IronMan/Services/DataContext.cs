using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using IronMan.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IronMan.Services
{
    public class DataContext : IDataContext
    {
        public Document Document { get; set; }

        public DataContext(Document document)
        {
            this.Document = document;
        }

        public UIDocument GetUIDocument()
        {
            return new UIDocument(this.Document);
        }
        
        public UIApplication GetUIApplication()
        {
            return GetUIDocument().Application;
        }

    }
}
