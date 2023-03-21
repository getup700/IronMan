///************************************************************************************
///   Author:Tony Stark
///   CretaeTime:2023/3/14 11:32:55
///   Mail:2609639898@qq.com
///   Github:https://github.com/getup700
///
///   Description:
///
///************************************************************************************

using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Mechanical;
using Autodesk.Revit.UI;
using IronMan.Revit.Toolkit.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IronMan.Revit.Commands
{
    [Transaction(TransactionMode.Manual)]
    [Journaling(JournalingMode.NoCommandData)]
    [Regeneration(RegenerationOption.Manual)]
    internal class MEPCurveConnectCommand : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uiDoc = commandData.Application.ActiveUIDocument;
            Document document = uiDoc.Document;
            Duct curve1;
            Duct curve2;
            try
            {
                Reference reference1 = uiDoc.Selection.PickObject(Autodesk.Revit.UI.Selection.ObjectType.Element);
                Reference reference2 = uiDoc.Selection.PickObject(Autodesk.Revit.UI.Selection.ObjectType.Element);
                curve1 = document.GetElement(reference1) as Duct;
                curve2 = document.GetElement(reference2) as Duct;
            }
            catch (Exception)
            {
                throw;
            }
            LocationCurve locationCurve1 = curve1.Location as LocationCurve;
            LocationCurve locationCurve2 = curve2.Location as LocationCurve;
            var line1 = locationCurve1.Curve as Line;
            var line2 = locationCurve2.Curve as Line;
            var intersectPoint = line1.GetIntersection(line2);





            return Result.Succeeded;

        }
        public XYZ GetNearestConnector(Reference reference, MEPCurve mEPCurve)
        {
            var connectors = mEPCurve.ConnectorManager.Connectors;
            XYZ result = XYZ.Zero;

            return result;
        }
    }
}
