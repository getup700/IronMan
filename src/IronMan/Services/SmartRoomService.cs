using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;
using Autodesk.Revit.DB.Mechanical;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using Autodesk.RevitAddIns;
using IronMan.Revit.Entity;
using IronMan.Revit.Interfaces;
using IronMan.Revit.IServices;
using IronMan.Revit.Toolkit.Extension;
using IronMan.Revit.Toolkit.Mvvm.Interfaces;
using IronMan.Revit.Toolkit.Mvvm.IOC;
using IronMan.Revit.Updaters;
using NPOI.SS.Formula.Functions;
using Org.BouncyCastle.Crypto.Tls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace IronMan.Revit.Services
{
    public class SmartRoomService
    {
        private IDataContext _dataContext;
        private IExternalEventService _externalEventService;
        private IUIProvider _uiProvider;
        public SmartRoomService(IDataContext dataContext, IExternalEventService externalService, IUIProvider uiProvider)
        {
            _dataContext = dataContext;
            _externalEventService = externalService;
            _uiProvider = uiProvider;
        }

        private Document Document => _dataContext.GetDocument();

        public void CreateRoomSeparation()
        {
            //GetModelLineDMU updater = new GetModelLineDMU(new AddInId(new Guid("104AE7B9-0A9C-4875-B67D-48B2020DE40B")));
            //if(UpdaterRegistry.IsUpdaterRegistered(updater.GetUpdaterId(),Document))
            //{
            //    UpdaterRegistry.UnregisterUpdater(updater.GetUpdaterId(), Document);
            //}
            //UpdaterRegistry.RegisterUpdater(updater, Document, true);
            GetModelLineDMU updater = SingletonIOC.Current.Container.GetInstance<GetModelLineDMU>();
            ElementFilter elementFilter = new ElementCategoryFilter(BuiltInCategory.OST_RoomSeparationLines);
            UpdaterRegistry.AddTrigger(updater.GetUpdaterId(), elementFilter, Element.GetChangeTypeElementAddition());

            // _uiProvider.GetUIApplication().Idling += SmartRoomService_Idling;
            RevitCommandId id = RevitCommandId.LookupCommandId("ID_OBJECTS_AREA_SEPARATION");
            if (id != null && _dataContext.GetUIApplication().CanPostCommand(id))
            {
                //_dataContext.GetUIApplication().PostCommand(id);
                _dataContext.GetUIApplication().CreateAddInCommandBinding(RevitCommandId.LookupPostableCommandId(PostableCommand.Move));
            }
            //var reference1 = _dataContext.GetUIDocument().Selection.PickObject(Autodesk.Revit.UI.Selection.ObjectType.Element);
            UpdaterRegistry.RemoveDocumentTriggers(Contants.IronManID.GetModelLineUpdaterId,
            SingletonIOC.Current.Container.GetInstance<IDataContext>().GetDocument());

        }

        private void SmartRoomService_Idling(object sender, Autodesk.Revit.UI.Events.IdlingEventArgs e)
        {
            MessageBox.Show("IdlingEvent has OnClick");
            //_dataContext.GetDocument().NewTransaction(() =>
            //{
            //    MessageBox.Show("This si IdlingEvent");
            //});
            _uiProvider.GetUIApplication().Idling -= SmartRoomService_Idling;
        }

        public RoomProxy CreateRoomProxy()
        {
            _externalEventService.Raise((uiapp) =>
            {
                Document.NewTransaction(() =>
                {
                    MessageBox.Show("ExternalEvent");
                    //List<Curve> curves = new List<Curve>();
                    //List<Curve> separationLines = _roomSeparationLines.Select(x => (Document.GetElement(x) as ModelLine).GeometryCurve).ToList();
                    //List<XYZ> centers = new List<XYZ>();
                    //for (int i = separationLines.Count() - 1; i >= 0; i++)
                    //{
                    //    ModelLine modelLine = Document.GetElement(_roomSeparationLines[i]) as ModelLine;
                    //    curves.Add(modelLine.GeometryCurve);
                    //    CurveArray curveArray = GetConnectedLine(modelLine.GeometryCurve, separationLines);
                    //    centers.Add(GetComputeCentroid(curveArray));
                    //    //Delete Used Curve
                    //    RemoveOverlapLine(separationLines[i], separationLines);
                    //}
                    //XYZ center = centers.FirstOrDefault();
                    //Room room = Document.Create.NewRoom(Document.ActiveView.GenLevel, new UV(center.X, center.Y));
                    //roomProxy = new RoomProxy(room);
                });
            });
            return null;
        }

        private XYZ GetComputeCentroid(CurveArray curveArray)
        {
            XYZ p1 = curveArray.get_Item(0).GetEndPoint(0);
            XYZ p2 = curveArray.get_Item(2).GetEndPoint(0);
            return (p1 + p2) * 0.5;
        }

        private void RemoveOverlapLine(Curve curve, List<Curve> curves)
        {
            foreach (var item in curves)
            {
                IntersectionResultArray array;
                SetComparisonResult result = curve.Intersect(item, out array);
                if (result == SetComparisonResult.Overlap)
                {
                    curves.Remove(item);
                }
            }
        }

        private CurveArray GetConnectedLine(Curve curve, List<Curve> curves)
        {
            CurveArray curveArray = new CurveArray();
            foreach (var item in curves)
            {
                IntersectionResultArray array;
                SetComparisonResult result = curve.Intersect(item, out array);
                if (array.Size == 1 && result != SetComparisonResult.Overlap)
                {
                    curveArray.Append(item);
                }
            }
            return curveArray;
        }

        private void SmartRoomService_DocumentChanged(object sender, Autodesk.Revit.DB.Events.DocumentChangedEventArgs e)
        {
            MessageBox.Show("SmartRoomService_DocumentChanged");
            //_roomSeparationLines = e.GetAddedElementIds().ToList();
            //_uiProvider.GetApplication().DocumentChanged -= new EventHandler<Autodesk.Revit.DB.Events.DocumentChangedEventArgs>(SmartRoomService_DocumentChanged);
        }

        public List<XYZ> GetPosition()
        {
            PickedBox pickedBox = _dataContext.GetUIDocument().Selection.PickBox(Autodesk.Revit.UI.Selection.PickBoxStyle.Enclosing);
            XYZ point1 = pickedBox.Min;
            XYZ point3 = pickedBox.Max;
            XYZ point2 = new XYZ(point3.X, point1.Y, point1.Z);
            XYZ point4 = new XYZ(point1.X, point3.Y, point1.Z);
            Line.CreateBound(point1, point2);
            Line.CreateBound(point2, point3);
            List<XYZ> points = new List<XYZ>();
            points.Add(point1);
            points.Add(point3);
            points.Add(point2);
            points.Add(point4);
            return points;
        }

        //public void RevitCommand()
        //{
        //    _externalEventService.Raise((uiapp) =>
        //    {
        //        var temp = _dataContext.GetUIDocument().Selection.PickPoint(ObjectSnapTypes.Intersections);
        //        var temp1 = _dataContext.GetUIDocument().Selection.PickPoint();
        //        Document.NewTransaction(() =>
        //        {
        //            RevitCommandId id = RevitCommandId.LookupCommandId("ID_OBJECTS_FILLED_REGION");
        //            if (_dataContext.GetUIApplication().CanPostCommand(id))
        //            {
        //                _dataContext.GetUIApplication().PostCommand(id);
        //            }
        //            foreach (var item in _roomSeparationLines)
        //            {
        //                ModelLine modelLine = Document.GetElement(item) as ModelLine;
        //                modelLine.GeometryCurve.
        //            }
        //        });
        //    });
        //}

    }
}
