using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;
using IronMan.Revit.Entity;
using IronMan.Revit.Interfaces;
using IronMan.Revit.IServices;
using IronMan.Revit.Toolkit.Mvvm.Interfaces;
using Microsoft.Win32;
using NPOI.POIFS.Crypt.Dsig;
using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IronMan.Revit.Toolkit.Extension;

namespace IronMan.Revit.Services
{
    public class CollectService : ICollectService, IExcelTransfer<RoomProxy>
    {
        private readonly IDataContext _dataContext;

        public CollectService(IDataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public double GetLength(RoomProxy room)
        {
            double result = 0;
            if (room == null) return result;
            var loops = room.BoundarySements;
            if (loops == null) return result;
            foreach (BoundarySegment segament in loops)
            {
                Wall wall = _dataContext.GetDocument().GetElement(segament.ElementId) as Wall;
                if (wall != null)
                {
                    result += segament.GetCurve().Length;
                }
            }
            return result.ConvertToMilliMeters()/1000;
        }

        [Obsolete("Import", false)]
        public IEnumerable<RoomProxy> Import()
        {
            throw new NotImplementedException();
        }

        public void Export(IEnumerable<RoomProxy> elements)
        {
            using (System.Windows.Forms.SaveFileDialog saveFileDialog = new System.Windows.Forms.SaveFileDialog())
            {
                saveFileDialog.Filter = "txt files(*.txt)|*.txt|All files(*.*)|*.*";
                saveFileDialog.FileName = "房间墙体长度统计";
                saveFileDialog.RestoreDirectory = true;
                if (saveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    DateTime exportDateTime = DateTime.Now;
                    string filePath = saveFileDialog.FileName;
                    FileStream fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write);
                    StreamWriter streamWriter = new StreamWriter(fileStream);
                    streamWriter.WriteLine(exportDateTime);
                    foreach (var item in elements)
                    {
                        streamWriter.WriteLine(item.Level?.Name + "\t" + item.Name + "\t" + item.Id + "\t" + item.Length + "\t" + item.Areas);
                    }
                    streamWriter.Flush();
                    streamWriter.Close();
                }
            }
        }
    }
}
