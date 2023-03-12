using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using IronMan.Revit.Toolkit.Extension;
using IronMan.Revit.Toolkit.Extension.Class;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace IronMan.Revit.Commands
{
    [Transaction(TransactionMode.Manual)]
    public class XRayCommand : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uiDoc = commandData.Application.ActiveUIDocument;
            Document doc = uiDoc.Document;
            Selection selection = uiDoc.Selection;
            Reference referenceRef = selection.PickObject(ObjectType.Element, "selection element");
            Element element = doc.GetElement(referenceRef);
            try
            {
                if (uiDoc.ActiveGraphicalView is View3D view)
                {
                    //构造射线
                    ReferenceIntersector intersector = new ReferenceIntersector(view);
                    intersector.SetTargetElementIds(doc.GetElementInstances(BuiltInCategory.OST_Walls).ToList().ConvertAll(w => w.Id));
                    intersector.SetFilter(new ElementCategoryFilter(BuiltInCategory.OST_Walls));
                    intersector.TargetType = FindReferenceTarget.Face;
                    //MessageBox.Show(doc.GetElementInstances(BuiltInCategory.OST_Walls).Count().ToString());
                    BoundingBoxXYZ boundingBox = element.get_BoundingBox(view);
                    XYZ centerPoint = (boundingBox.Max + boundingBox.Min) / 2;

                    //射线查询
                    ReferenceWithContext referenceWithContext = intersector.FindNearest(centerPoint, XYZ.BasisX);
                    if (referenceWithContext == null) return Result.Cancelled;
                    Line line = Line.CreateBound(centerPoint, referenceWithContext.GetReference().GlobalPoint);
                    Line line1 = Line.CreateBound(XYZ.Zero, XYZ.Zero);
                    doc.TransientDisplay(line1);
                    selection.SetElementIds(new List<ElementId>() { referenceWithContext.GetReference().ElementId });

                    IList<ReferenceWithContext> referenceWithContexts = intersector.Find(centerPoint, XYZ.BasisX);
                    ElementSet elementSet = new ElementSet();
                    string info = string.Empty;
                    foreach (var item in referenceWithContexts)
                    {
                        Element e = doc.GetElement(item.GetReference());
                        info += $"{e.Name}\t{e.Category.Name}\t{e.Id}\n";
                        elementSet.Insert(e);
                    }
                    MessageBox.Show(info);
                    IEnumerable<ElementId> elementIds = referenceWithContexts.ToList().ConvertAll(x => x.GetReference().ElementId);
                    if (referenceWithContexts.Count() == 0) return Result.Cancelled;
                    selection.SetElementIds(elementIds.ToList());
                    MessageBox.Show(referenceWithContexts.Count().ToString());
                    
                }
            }
            catch (Exception ex)
            {
               message = ex.Message;
            }
            return Result.Succeeded;
        }
    }
}
