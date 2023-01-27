using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;
using IronMan.Revit.Entity.Profiles;
using IronMan.Revit.Interfaces;
using IronMan.Revit.IServices;
using IronMan.Revit.Toolkit.Extension;
using IronMan.Revit.Toolkit.Mvvm;
using IronMan.Revit.Toolkit.Mvvm.Interfaces;
using IronMan.Revit.Toolkit.Mvvm.IOC;
using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;
using NPOI.OpenXmlFormats.Dml;
using NPOI.POIFS.FileSystem;
using NPOI.SS.Formula.Functions;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;

namespace IronMan.Revit.Services
{
    public class DefinitionParameterService : IDefinitionParameterService, IExcelTransfer<Room>
    {
        private readonly IDataContext _dataContext;
        private readonly IUIProvider _uiProvider;
        public DefinitionParameterService(IDataContext dataContext, IUIProvider uiProvider)
        {
            _dataContext = dataContext;
            _uiProvider = uiProvider;
        }

        public void BindingElement(CategorySet categorySet, IDefinition definitionProxy)
        {
            var definitionFile = _uiProvider.GetApplication().GetDefinitionFile();
            var group = definitionFile.GetDefinitionGroup("Decoration");
            var definition = group.GetDefinition(definitionProxy.Name, definitionProxy.ParameterType);
            InstanceBinding binding = _uiProvider.GetApplication().Create.NewInstanceBinding(categorySet);
            _dataContext.GetDocument().NewTransaction(() =>
            {
                _dataContext.GetDocument().ParameterBindings.Insert(definition, binding, definitionProxy.ParameterGroup);
            }, "Bingding ProjectParameter");
        }

        public List<Parameter> GetDecoraParameter(DecorationParameterProfile decorationProfile)
        {
            var rooms = _dataContext.GetDocument().GetElementsByElementFilter<Room>(new RoomFilter()).ToList();
            if (rooms.Count == 0) return null;
            var firstRoom = rooms.FirstOrDefault();
            var result = new List<Parameter>();
            foreach (var parameter in decorationProfile.Parameters)
            {
                var categorySet = new CategorySet();
                categorySet.Insert(firstRoom.Category);
                Parameter revitParameter = firstRoom.GetParameters(parameter.Name)
                    .Where(x => parameter.Equal(x.Definition)).FirstOrDefault();
                if (revitParameter is null)
                {
                    BindingElement(categorySet, parameter);
                    Parameter newRevitParameter = firstRoom.GetParameters(parameter.Name)
                        .Where(x => parameter.Equal(x.Definition)).FirstOrDefault();
                    result.Add(newRevitParameter);

                }
                else
                {
                    result.Add(revitParameter);
                }
            }

            //List<string> title = new List<string>
            //{
            //    firstRoom.GetParameters(decorationProfile.RoomCode.Name).Where(x => decorationProfile.RoomCode.Equal(x.Definition)).FirstOrDefault().Definition.Name,
            //    firstRoom.GetParameters(decorationProfile.RoomName.Name).Where(x => decorationProfile.RoomName.Equal(x.Definition)).FirstOrDefault().Definition.Name,
            //    firstRoom.GetParameters(decorationProfile.Floor.Name).Where(x => decorationProfile.Floor.Equal(x.Definition)).FirstOrDefault().Definition.Name,
            //    firstRoom.GetParameters(decorationProfile.GroundMethodCode.Name).Where(x => decorationProfile.GroundMethodCode.Equal(x.Definition)).FirstOrDefault().Definition.Name,
            //    firstRoom.GetParameters(decorationProfile.GroundThickness.Name).Where(x => decorationProfile.GroundThickness.Equal(x.Definition)).FirstOrDefault().Definition.Name,
            //    firstRoom.GetParameters(decorationProfile.InteriorWallMethodCode.Name).Where(x => decorationProfile.InteriorWallMethodCode.Equal(x.Definition)).FirstOrDefault().Definition.Name,
            //    firstRoom.GetParameters(decorationProfile.CeilingMethodCode.Name).Where(x => decorationProfile.CeilingMethodCode.Equal(x.Definition)).FirstOrDefault().Definition.Name,
            //    firstRoom.GetParameters(decorationProfile.CeilingHeight.Name).Where(x => decorationProfile.CeilingHeight.Equal(x.Definition)).FirstOrDefault().Definition.Name,
            //    firstRoom.GetParameters(decorationProfile.BaseBoardMethodCode.Name).Where(x => decorationProfile.BaseBoardMethodCode.Equal(x.Definition)).FirstOrDefault().Definition.Name,
            //    firstRoom.GetParameters(decorationProfile.BaseBoardHeight.Name).Where(x => decorationProfile.BaseBoardHeight.Equal(x.Definition)).FirstOrDefault().Definition.Name
            //};
            //if (title.Where(x => x is null).Count() > 0)
            //{
            //    MessageBox.Show("模型信息部分丢失，请重新关联项目参数后重试");
            //    return null;
            //}
            return result;
        }

        public IEnumerable<Room> Import()
        {
            string filePath = string.Empty;
            System.Windows.Forms.OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog();
            openFileDialog.Filter = "Excel文件|*.xls";
            openFileDialog.RestoreDirectory = true;
            openFileDialog.FilterIndex = 1;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                filePath = openFileDialog.FileName;
            }
            //try
            //{
            //    if (File.Exists(filePath))
            //    {
            //        File.Delete(filePath);
            //    }
            //}
            //catch (Exception)
            //{
            //    //there is a exception that will be caught by transaction,so this statement is invalid
            //    //throw new Exception("dkaslfdkasd");
            //    MessageBox.Show("文档被占用，请关闭后重试");
            //    return null;
            //}
            FileStream fileStream = null;
            try
            {
                fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            }
            catch (Exception)
            {
                MessageBox.Show("File is in use,close it and try again please!");
            }
            IWorkbook workbook = null;
            string extension = Path.GetExtension(filePath);
            if (extension == ".xls")
            {
                workbook = new HSSFWorkbook(fileStream);
            }
            else
            {
                workbook = new XSSFWorkbook(filePath);
            }
            ISheet sheet = workbook.GetSheetAt(0);
            int startRow = 1;
            int startColumn = 1;
            var decorationProfile = SingletonIOC.Current.Container.GetInstanceWithoutCaching<DecorationParameterProfile>();
            GetDecoraParameter(decorationProfile);
            List<DecorationParameterProfile> profiles = new List<DecorationParameterProfile>();
            System.Collections.IEnumerator enumerator = sheet.GetRowEnumerator();

            //后边补标题匹配
            //var title = new List<string>();
            //for (int i = 0; i < sheet.LastRowNum; i++)
            //{
            //     title.Add(sheet.GetRow(0).GetCell(i).ToString());
            //}

            //write content
            var rooms = _dataContext.GetDocument().GetElementsByElementFilter<Room>(new RoomFilter());
            for (int i = startRow; i < sheet.LastRowNum; i++)
            {
                string code = sheet.GetRow(i).GetCell(1).ToString();

                var room = rooms.Where(x => x.GetParameters(decorationProfile.RoomCode.Name).Any(y => y.AsString() == code)).FirstOrDefault();
                if (room != null)
                {
                    _dataContext.GetDocument().NewTransaction(() =>
                    {

                        try
                        {
                            room.GetParameters(decorationProfile.RoomName.Name).FirstOrDefault()?.Set(sheet.GetRow(i).GetCell(2).ToString());
                            room.GetParameters(decorationProfile.Floor.Name).FirstOrDefault()?.Set(sheet.GetRow(i).GetCell(3).ToString());
                            room.GetParameters(decorationProfile.GroundMethodCode.Name).FirstOrDefault()?.Set(sheet.GetRow(i).GetCell(4).ToString());
                            room.GetParameters(decorationProfile.GroundThickness.Name).FirstOrDefault()?.Set(double.Parse(sheet.GetRow(i).GetCell(5).ToString()));
                            room.GetParameters(decorationProfile.InteriorWallMethodCode.Name).FirstOrDefault()?.Set(sheet.GetRow(i).GetCell(6).ToString());
                            room.GetParameters(decorationProfile.CeilingMethodCode.Name).FirstOrDefault()?.Set(sheet.GetRow(i).GetCell(7).ToString());
                            room.GetParameters(decorationProfile.CeilingHeight.Name).FirstOrDefault()?.Set(double.Parse(sheet.GetRow(i).GetCell(8).ToString()));
                            room.GetParameters(decorationProfile.BaseBoardMethodCode.Name).FirstOrDefault()?.Set(sheet.GetRow(i).GetCell(9).ToString());
                            room.GetParameters(decorationProfile.BaseBoardHeight.Name).FirstOrDefault()?.Set(double.Parse(sheet.GetRow(i).GetCell(10).ToString()));
                        }
                        catch (Exception)
                        {
                        }
                    });
                }
            }
            fileStream.Close();
            return rooms;
        }

        public void Export(IEnumerable<Room> elements)
        {
            #region PrepareData
            var rooms = _dataContext.GetDocument().GetElementsByElementFilter<Room>(new RoomFilter()).ToList();
            if (rooms.Count == 0) return;
            #endregion

            #region OpenFile
            System.Windows.Forms.SaveFileDialog saveFileDialog = new System.Windows.Forms.SaveFileDialog()
            {
                //Filter = "txt files(*.txt)|*.txt|All files(*.*)|*.*",
                Filter = "Excel|*.xls",
                FileName = "智成BIM数据导出",
                RestoreDirectory = true
            };
            if (saveFileDialog.ShowDialog() != DialogResult.OK) return;
            string filePath = saveFileDialog.FileName;
            // Data to be written
            //int rowCount = elements.FirstOrDefault().RowCount;
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
            ISheet sheet = workbook.CreateSheet("模型信息");
            #endregion

            #region SetStyle

            sheet.CreateFreezePane(0, 1);
            CellRangeAddress cellRangeAddress = CellRangeAddress.ValueOf("A1:K1");
            sheet.SetAutoFilter(cellRangeAddress);
            ICellStyle titleRowStyle = workbook.CreateCellStyle();
            titleRowStyle.FillForegroundColor = HSSFColor.LightGreen.Index;
            titleRowStyle.FillPattern = NPOI.SS.UserModel.FillPattern.SolidForeground;
            titleRowStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
            IFont font = workbook.CreateFont();
            font.IsBold = true;
            titleRowStyle.SetFont(font);

            ICellStyle contentStyle = workbook.CreateCellStyle();
            contentStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;

            ICellStyle nummericalStyle = workbook.CreateCellStyle();
            nummericalStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
            nummericalStyle.DataFormat = HSSFDataFormat.GetBuiltinFormat("0.00");
            //IDataFormat dataFormat= workbook.CreateDataFormat();
            //nummericalStyle.DataFormat  = dataFormat.GetFormat("0.00");
            //workbook.Write(fileStream);
            for (int i = 0; i < 11; i++)
            {
                if (i < 4)
                {
                    sheet.SetColumnWidth(i, 10 * 256);
                }
                else
                {
                    sheet.SetColumnWidth(i, 18 * 256);
                }
            }
            #endregion

            #region Content

            #region TitleRow
            var decorationProfile = SingletonIOC.Current.Container.GetInstance<DecorationParameterProfile>();
            var title = GetDecoraParameter(decorationProfile);
            IRow titleRow = sheet.CreateRow(0);
            titleRow.CreateCell(0).SetCellValue("");
            for (int i = 0; i < title.Count; i++)
            {
                titleRow.CreateCell(i + 1).SetCellValue(title.ElementAt(i).Definition.Name);
                titleRow.GetCell(i + 1).CellStyle = titleRowStyle;
            }
            #endregion

            #region WriteContent
            //DataTable dataTable = new DataTable();
            //foreach (var room in rooms)
            //{
            //    dataTable.Rows.Add(new string[]
            //    {
            //         room.GetParameters(decorationProfile.RoomCode.Name).Where(x => decorationProfile.RoomCode.Equal(x.Definition)).FirstOrDefault()?.AsString(),
            //         room.GetParameters(decorationProfile.RoomName.Name).Where(x => decorationProfile.RoomName.Equal(x.Definition)).FirstOrDefault()?.AsString(),
            //         room.GetParameters(decorationProfile.Floor.Name).Where(x => decorationProfile.Floor.Equal(x.Definition)).FirstOrDefault()?.AsString(),
            //         room.GetParameters(decorationProfile.GroundMethodCode.Name).Where(x => decorationProfile.GroundMethodCode.Equal(x.Definition)).FirstOrDefault()?.AsString(),
            //         room.GetParameters(decorationProfile.GroundThickness.Name).Where(x => decorationProfile.GroundThickness.Equal(x.Definition)).FirstOrDefault()?.AsValueString(),
            //         room.GetParameters(decorationProfile.InteriorWallMethodCode.Name).Where(x => decorationProfile.InteriorWallMethodCode.Equal(x.Definition)).FirstOrDefault()?.AsString(),
            //         room.GetParameters(decorationProfile.CeilingMethodCode.Name).Where(x => decorationProfile.CeilingMethodCode.Equal(x.Definition)).FirstOrDefault()?.AsString(),
            //         room.GetParameters(decorationProfile.CeilingHeight.Name).Where(x => decorationProfile.CeilingHeight.Equal(x.Definition)).FirstOrDefault()?.AsValueString(),
            //         room.GetParameters(decorationProfile.BaseBoardMethodCode.Name).Where(x => decorationProfile.BaseBoardMethodCode.Equal(x.Definition)).FirstOrDefault()?.AsString(),
            //         room.GetParameters(decorationProfile.BaseBoardHeight.Name).Where(x => decorationProfile.BaseBoardHeight.Equal(x.Definition)).FirstOrDefault()?.AsValueString()
            //    });
            //}
            //Create content
            //var content = elements.ToList();

            for (int i = 0; i < rooms.Count(); i++)
            {
                var row = sheet.CreateRow(i + 1);
                //var datarow = datatable.rows[i];
                var room = rooms.ElementAt(i);
                var value0 = room.GetParameters(decorationProfile.RoomCode.Name).Where(x => decorationProfile.RoomCode.Equal(x.Definition)).FirstOrDefault()?.AsString();
                var value1 = room.GetParameters(decorationProfile.RoomName.Name).Where(x => decorationProfile.RoomName.Equal(x.Definition)).FirstOrDefault()?.AsString();
                var value2 = room.GetParameters(decorationProfile.Floor.Name).Where(x => decorationProfile.Floor.Equal(x.Definition)).FirstOrDefault()?.AsString();
                var value3 = room.GetParameters(decorationProfile.GroundMethodCode.Name).Where(x => decorationProfile.GroundMethodCode.Equal(x.Definition)).FirstOrDefault()?.AsString();
                var value4 = room.GetParameters(decorationProfile.GroundThickness.Name).Where(x => decorationProfile.GroundThickness.Equal(x.Definition)).FirstOrDefault()?.AsValueString();
                var value5 = room.GetParameters(decorationProfile.InteriorWallMethodCode.Name).Where(x => decorationProfile.InteriorWallMethodCode.Equal(x.Definition)).FirstOrDefault()?.AsString();
                var value6 = room.GetParameters(decorationProfile.CeilingMethodCode.Name).Where(x => decorationProfile.CeilingMethodCode.Equal(x.Definition)).FirstOrDefault()?.AsString();
                var value7 = room.GetParameters(decorationProfile.CeilingHeight.Name).Where(x => decorationProfile.CeilingHeight.Equal(x.Definition)).FirstOrDefault()?.AsValueString();
                var value8 = room.GetParameters(decorationProfile.BaseBoardMethodCode.Name).Where(x => decorationProfile.BaseBoardMethodCode.Equal(x.Definition)).FirstOrDefault()?.AsString();
                var value9 = room.GetParameters(decorationProfile.BaseBoardHeight.Name).Where(x => decorationProfile.BaseBoardHeight.Equal(x.Definition)).FirstOrDefault()?.AsValueString();
                row.CreateCell(0).SetCellValue(i + 1);
                row.CreateCell(1).SetCellValue(value0);
                row.CreateCell(2).SetCellValue(value1);
                row.CreateCell(3).SetCellValue(value2);
                row.CreateCell(4).SetCellValue(value3);
                row.CreateCell(5).SetCellValue(value4);
                row.CreateCell(6).SetCellValue(value5);
                row.CreateCell(7).SetCellValue(value6);
                row.CreateCell(8).SetCellValue(value7);
                row.CreateCell(9).SetCellValue(value8);
                row.CreateCell(10).SetCellValue(value9);
                row.Where(x => x.ColumnIndex == 5 || x.ColumnIndex == 9 || x.ColumnIndex == 10).Select(x => x.CellStyle = nummericalStyle);
                foreach (var cell in row)
                {
                    if (cell.ColumnIndex == 5 || cell.ColumnIndex == 8 || cell.ColumnIndex == 10)
                    {
                        cell.CellStyle = nummericalStyle;
                    }
                    else
                    {
                        cell.CellStyle = contentStyle;
                    }
                }
            }
            #endregion
            workbook.Write(fileStream);
            fileStream.Close();
            #endregion
        }

    }
}

