using Autodesk.Revit.DB;
using IronMan.Revit.Entity;
using IronMan.Revit.Interfaces;
using IronMan.Revit.IServices;
using IronMan.Revit.Toolkit.Extension;
using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using CellType = NPOI.SS.UserModel.CellType;
using FillPattern = NPOI.SS.UserModel.FillPattern;

namespace IronMan.Revit.Services
{
    public class RoomLengthExcelService : IRoomLengthExcelService, IExcelTransfer<RoomProxy>
    {
        public void Export(IEnumerable<RoomProxy> elements)
        {
            System.Windows.Forms.SaveFileDialog saveFileDialog = new System.Windows.Forms.SaveFileDialog()
            {
                //Filter = "txt files(*.txt)|*.txt|All files(*.*)|*.*",
                Filter = "Excel|*.xls",
                FileName = "房间墙体长度统计",
                RestoreDirectory = true
            };

            if (saveFileDialog.ShowDialog() != DialogResult.OK) return;
            string filePath = saveFileDialog.FileName;
            // Data to be written
            //int rowCount = elements.FirstOrDefault().RowCount;
            int columnCount = elements.Count();
            try
            {
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
            }
            catch (Exception)
            {
                //there is a exception that will be caught by transaction,so this statement is invalid
                //throw new Exception("dkaslfdkasd");
                MessageBox.Show("文档被占用，请关闭后重试");
                return;
            }
            FileStream fileStream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Write);
            //IWorkbook workbook = WorkbookFactory.Create(fileStream);
            //IWorkbook workbook = new XSSFWorkbook();
            IWorkbook workbook = new HSSFWorkbook();
            ISheet sheet = workbook.CreateSheet("房间信息");
            //Table style
            IRow titleRow = sheet.CreateRow(0);

            #region Style
            sheet.CreateFreezePane(0, 1);
            CellRangeAddress cellRangeAddress = CellRangeAddress.ValueOf("A1:E1");
            sheet.SetAutoFilter(cellRangeAddress);
            ICellStyle titleRowStyle = workbook.CreateCellStyle();
            titleRowStyle.FillForegroundColor = HSSFColor.LightGreen.Index;
            titleRowStyle.FillPattern = FillPattern.SolidForeground;
            titleRowStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
            IFont font = workbook.CreateFont();
            font.IsBold = true;
            titleRowStyle.SetFont(font);

            ICellStyle contentStyle = workbook.CreateCellStyle();
            contentStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;

            ICellStyle nummericalStyle = workbook.CreateCellStyle();
            nummericalStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
            nummericalStyle.DataFormat = HSSFDataFormat.GetBuiltinFormat("0.00");
            #endregion

            #region Content

            titleRow.CreateCell(0).SetCellValue("");
            titleRow.CreateCell(1).SetCellValue("ID");
            titleRow.CreateCell(1).SetCellValue("房间编号");
            titleRow.CreateCell(2).SetCellValue("房间名称");
            titleRow.CreateCell(3).SetCellValue("面积(㎡)");
            titleRow.CreateCell(4).SetCellValue("长度(m)");
            titleRow.GetCell(0).CellStyle = titleRowStyle;
            titleRow.GetCell(1).CellStyle = titleRowStyle;
            titleRow.GetCell(2).CellStyle = titleRowStyle;
            titleRow.GetCell(3).CellStyle = titleRowStyle;
            titleRow.GetCell(4).CellStyle = titleRowStyle;
            //Create content
            var content = elements.ToList();
            for (int i = 1; i < elements.Count() + 1; i++)
            {
                IRow row = sheet.CreateRow(i);
                row.RowStyle = contentStyle;
                row.CreateCell(0).SetCellValue(i);
                row.CreateCell(1).SetCellValue(content[i - 1].Id.IntegerValue);
                row.CreateCell(2).SetCellValue(content[i - 1].Name);
                row.CreateCell(3).SetCellValue(content[i - 1].Areas);
                row.CreateCell(4).SetCellValue(content[i - 1].Length);
                row.GetCell(0).CellStyle = contentStyle;
                row.GetCell(1).CellStyle = contentStyle;
                row.GetCell(2).CellStyle = contentStyle;
                row.GetCell(3).CellStyle = nummericalStyle;
                row.GetCell(4).CellStyle = nummericalStyle;
            }

            workbook.Write(fileStream,true);
            workbook.GetSheetAt(0).AutoSizeColumn(0);
            workbook.GetSheetAt(0).AutoSizeColumn(1);
            workbook.GetSheetAt(0).AutoSizeColumn(2);
            workbook.GetSheetAt(0).AutoSizeColumn(3);
            workbook.GetSheetAt(0).AutoSizeColumn(4);
            fileStream.Close();

            #endregion
        }

        public IEnumerable<RoomProxy> Import()
        {
            throw new NotImplementedException();
        }

        public List<CurveLoop> GetRoomCureveLoops(RoomProxy roomProxy)
        {
            List<CurveLoop> loops = new List<CurveLoop>();
            var options = new SpatialElementBoundaryOptions();
            IList<IList<BoundarySegment>> segmentsLists = roomProxy._room.GetBoundarySegments(options);
            foreach (IList<BoundarySegment> segment in segmentsLists)
            {
                CurveLoop curveList = new CurveLoop();
                foreach (BoundarySegment s in segment)
                {
                    curveList.Append(s.GetCurve());
                }
                loops.Add(curveList);
            }
            return loops;
        }


    }
}
