using Autodesk.Revit.DB;
using RevitColor = Autodesk.Revit.DB.Color;
using IronMan.Revit.Entity;
using IronMan.Revit.Interfaces;
using IronMan.Revit.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Toolkit.Extension;
using IronMan.Revit.Toolkit.Mvvm.Interfaces;
using GalaSoft.MvvmLight.Messaging;
using System.Xml.Linq;
using System.Windows;
using Autodesk.Revit.UI;
using Microsoft.Win32;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System.Drawing;

namespace IronMan.Revit.Services
{
    public class MaterialService : IMaterialService
    {
        private readonly IDataContext _dataContext;

        public MaterialService(IDataContext dataContext)
        {
            this._dataContext = dataContext;
        }

        public MaterialPlus CreateElement(string name)
        {
            Element element = null;
            _dataContext.Document.NewTransaction(() =>
            {
                ElementId id = Material.Create(_dataContext.Document, name);
                element = _dataContext.Document.GetElement(id);
            }, "创建材质");
            return new MaterialPlus(element as Material);
        }

        public void DeleteElement(MaterialPlus element)
        {
            if (element == null) return;
            _dataContext.Document.NewTransaction(() =>
            {
                _dataContext.Document.Delete(element.Id);
            }, "删除材质");
        }

        public void DeleteElements(IEnumerable<MaterialPlus> elements)
        {
            if (elements.Count() == 0) return;
            _dataContext.Document.NewTransaction(() =>
            {
                foreach (var element in elements)
                {
                    Messenger.Default.Send<string>(element.Name, Contacts.Tokens.ProgressBarTitle);
                    _dataContext.Document.Delete(element.Id);
                    _dataContext.GetDocument().Regenerate();//传入每个实例//不加也行//事务完成后会进行全部生成，最后会卡一下
                }
            }, "删除材质");
        }

        public IEnumerable<MaterialPlus> GetElements(Func<MaterialPlus, bool> predicate = null)
        {
            FilteredElementCollector elements = new FilteredElementCollector(_dataContext.Document).OfClass(typeof(Material));
            IEnumerable<MaterialPlus> materialPlus = elements.ToList().ConvertAll(x => new MaterialPlus(x as Material));
            if (predicate != null)
            {
                materialPlus = materialPlus.Where(predicate);
            }
            return materialPlus;
        }

        public void Export(IEnumerable<MaterialPlus> elements)
        {
            var dg = new SaveFileDialog(); 
            dg.Filter = "*.xls|*.xls";
            dg.FileName = "C:\\User\\Administrator\\Desktop\\MaterialProxy";
            bool? result = dg.ShowDialog();
            string fileName;
            if(result == true)
            {
                fileName = dg.FileName;
            }
            HSSFWorkbook workbook = new HSSFWorkbook();
            string worksheetName = "MaterialProxy";
            ISheet sheet1 = workbook.CreateSheet(worksheetName);
            //TitleName
            var row0 = sheet1.CreateRow(0);
            row0.CreateCell(0).SetCellValue("MaretialProxy");
            sheet1.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(0, 0, 0, 2));
            sheet1.CreateFreezePane(1, 2);
            ICellStyle titleStyle =workbook.CreateCellStyle();
            row0.Height = 25;
            //ColumnName
            var row1 = sheet1.CreateRow(1);
            row1.CreateCell(0).SetCellValue("材质名称");
            row1.CreateCell(1).SetCellValue("颜色");
            row1.CreateCell(2).SetCellValue("外观颜色");
            ICellStyle columnTitleStyle = workbook.CreateCellStyle();
            columnTitleStyle.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Center;

            //write Content
            for (int i = 0; i < 3; i++)
            {

            }
        }

        public IEnumerable<MaterialPlus> Import()
        {
            throw new NotImplementedException();
        }

        public void Save(MaterialPlus materialPlus, string name, RevitColor color, RevitColor appearanceColor)
        {
            if (name == null) return;
            if (color == null) color = new RevitColor(255, 255, 255);
            if (appearanceColor == null) appearanceColor = new RevitColor(255, 255, 255);
            if (materialPlus.Name != name)
            {
                materialPlus.Name = name; ;
                _dataContext.Document.NewTransaction(() => materialPlus.Material.Name = name);
            }
            if (materialPlus.Color != color)
            {
                materialPlus.Color = color;
                _dataContext.Document.NewTransaction(() => materialPlus.Material.Color = color);
            }
            if (materialPlus.AppearanceColor != appearanceColor)
            {
                materialPlus.AppearanceColor = appearanceColor;
                _dataContext.Document.NewTransaction(() => materialPlus.SetAppearanceColor(appearanceColor));
            }
            MessageBox.Show("nb");
        }

    }
}
