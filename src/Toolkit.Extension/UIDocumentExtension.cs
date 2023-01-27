using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Mechanical;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using IronMan.Revit.Toolkit.Extension.Class;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IronMan.Revit.Toolkit.Extension
{
    public static class UIDocumentExtension
    {
        public static IEnumerable<Element> SelectElements(this UIDocument uiDocument,
            ObjectType objectType = ObjectType.Element,
            ISelectionFilter filter = null,
            Func<Element, bool> predicate = null)
        {
            Selection selection = uiDocument.Selection;
            Document doc = uiDocument.Document;
            var elements = selection.GetElementIds().ToList().Select(x => doc.GetElement(x)).ToList();
            IEnumerable<Element> results = null;
            //if select elements first
            if (elements.Count != 0)
            {
                results = predicate == null ? elements : elements.Where(predicate);
            }
            else
            {
                var ele = selection.PickObjects(objectType, filter, "选择要修改的风管")
                      .Select(x => doc.GetElement(x) as MEPCurve).ToList();
                results = predicate == null ? ele : ele.Where(predicate);
            }
            return results;

        }
    }
}
