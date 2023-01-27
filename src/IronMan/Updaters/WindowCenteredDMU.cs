using Autodesk.Revit.DB;
using IronMan.Revit.Toolkit.Mvvm.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace IronMan.Revit.Updaters
{
    /// <summary>
    /// 文档修改才会触发DMU，execute本身包含在事务内
    /// 如果dmu因为异常被禁用，则无法通过EnableUpdater重新启用，禁用DisableUpdater
    /// </summary>
    /// <param name="data"></param>
    public class WindowCenteredDMU : IUpdater
    {
        private readonly IUIProvider _uiProvider;

        public WindowCenteredDMU(IUIProvider uiProvider)
        {
            _uiProvider = uiProvider;
        }

        public void Execute(UpdaterData data)
        {
            Document document = data.GetDocument();
            var modifiedIds = data.GetModifiedElementIds();
            foreach (ElementId id in modifiedIds)
            {
                //  Wall aWall = rvtDoc.get_Element(id) as Wall; // For 2012
                Wall aWall = document.GetElement(id) as Wall; // For 2013
                if (aWall == null) break;
                CenterWindowDoor(document, aWall);

                //Get the wall solid. 

                //Options opt = new Options();
                //opt.ComputeReferences = false;

                //Solid wallSolid = null;
                //GeometryElement geoElem = aWall.get_Geometry(opt);
                //foreach (GeometryObject geoObj in geoElem.)
                //{
                //    wallSolid = geoObj as Solid;
                //    if (wallSolid != null)
                //    {
                //        if (wallSolid.Faces.Size > 0)
                //        {
                //            break;
                //        }
                //    }
                //}
                //XYZ ptCenter = wallSolid.ComputeCentroid();        
            }
        }

        private void CenterWindowDoor(Document rvtDoc, Wall aWall)
        {
            if (aWall == null) return;
            // Find a winow or a door on the wall. 
            FamilyInstance e = FindWindowDoorOnWall(rvtDoc, aWall);
            if (e == null) return;
            
            // Move the element (door or window) to the center of the wall. 

            // Center of the wall 
            LocationCurve wallLocationCurve = aWall.Location as LocationCurve;
            XYZ pt1 = wallLocationCurve.Curve.GetEndPoint(0);
            XYZ pt2 = wallLocationCurve.Curve.GetEndPoint(1);
            XYZ midPt = (pt1 + pt2) * 0.5;

            LocationPoint loc = e.Location as LocationPoint;
            loc.Point = new XYZ(midPt.X, midPt.Y, loc.Point.Z);
        }
        private FamilyInstance FindWindowDoorOnWall(Document rvtDoc, Wall aWall)
        {
            // Collect the list of windows and doors 
            // No object relation graph. So going hard way. 
            // List all the door instances 
            var windowDoorCollector = new FilteredElementCollector(rvtDoc);
            windowDoorCollector.OfClass(typeof(FamilyInstance));

            ElementCategoryFilter windowFilter = new ElementCategoryFilter(BuiltInCategory.OST_Windows);
            ElementCategoryFilter doorFilter = new ElementCategoryFilter(BuiltInCategory.OST_Doors);
            LogicalOrFilter windowDoorFilter = new LogicalOrFilter(windowFilter, doorFilter);

            windowDoorCollector.WherePasses(windowDoorFilter);
            IList<Element> windowDoorList = windowDoorCollector.ToElements();

            // This is really bad in a large model!
            // You might have ten thousand doors and windows.
            // It would make sense to add a bounding box containment or intersection filter as well.

            // Check to see if the door or window is on the wall we got. 
            foreach (FamilyInstance e in windowDoorList)
            {
                if (e.Host.Id.Equals(aWall.Id))
                {
                    return e;
                }
            }

            // If you come here, you did not find window or door on the given wall. 

            return null;
        }

        /// <summary>
        /// DMU出错时弹出
        /// </summary>
        /// <returns></returns>
        public string GetAdditionalInformation() => "Keep Revit Window Centered also";

        public ChangePriority GetChangePriority()=>ChangePriority.Annotations;

        public UpdaterId GetUpdaterId() => new UpdaterId(_uiProvider.GetAddInId(), new Guid("0C310C0F-A91C-4369-8045-98301E720D2B"));

        /// <summary>
        /// DMU出错时弹出
        /// </summary>
        /// <returns></returns>
        public string GetUpdaterName() => nameof(WindowCenteredDMU);
    }
}
