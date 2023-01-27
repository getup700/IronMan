using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;
using Autodesk.Revit.UI;
using GalaSoft.MvvmLight.Messaging;
using IronMan.Revit.Entity;
using IronMan.Revit.Toolkit.Mvvm.Interfaces;
using IronMan.Revit.Views.Previews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using IronMan.Revit.Toolkit.Extension;

namespace IronMan.Revit.Updaters
{
    public class GetModelLineDMU : IUpdater
    {
        private readonly IUIProvider _uiProvider;
        private readonly AddInId _addInId = Contants.IronManID.IronMan;

        public GetModelLineDMU(IUIProvider uIProvider)
        {
            _uiProvider = uIProvider;
        }

        public void Execute(UpdaterData data)
        {
            Document document = data.GetDocument();
            var addedIds = data.GetAddedElementIds();
            if (addedIds.Count != 4) return;
            List<XYZ> added = new List<XYZ>();
            //左到右，逆时针；右到左，顺时针
            //第一根线永远是上或下
            foreach (var id in addedIds)
            {
                ModelLine modelLine = document.GetElement(id) as ModelLine;
                added.Add((modelLine.GeometryCurve.GetEndPoint(0) + modelLine.GeometryCurve.GetEndPoint(1)) / 2);
            }
            XYZ center = XYZ.Zero;
            for (int i = 0; i < added.Count; i++)
            {
                center += added[i];
            }
            document.NewSubTransaction(() =>
            {
                Room room = document.Create.NewRoom(document.ActiveView.GenLevel, new UV(center.X / 4, center.Y / 4));
                RoomProxy roomProxy = new RoomProxy(room);
                Messenger.Default.Send<RoomProxy>(roomProxy, Contants.Tokens.SmartRoomService);
            });
            //SmartRoomPreview preview = new SmartRoomPreview(document);
            //preview.ShowDialog();
            
            ////DMU cannot excute select command
            //var reference = new UIDocument(document).Selection.PickObject(Autodesk.Revit.UI.Selection.ObjectType.Edge, "Select");
            //var curve = (document.GetElement(reference) as ModelLine)?.GeometryCurve;
            MessageBox.Show($"{added[0]}\n{added[1]}\n{added[2]}\n{added[3]}\n");
        }

        public string GetAdditionalInformation()
        {
            return "STARK INDUSTRIES";
        }

        public ChangePriority GetChangePriority()
        {
            return ChangePriority.Annotations;
        }

        public UpdaterId GetUpdaterId()
        {
            return Contants.IronManID.GetModelLineUpdaterId;
        }

        public string GetUpdaterName()
        {
            return nameof(GetModelLineDMU);
        }
    }
}
