using Autodesk.Revit.DB;
using RevitColor = Autodesk.Revit.DB.Color;
using IronMan.Revit.Entity;
using IronMan.Revit.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using IronMan.Revit.Toolkit.Extension;
using IronMan.Revit.Toolkit.Mvvm.Interfaces;
using GalaSoft.MvvmLight.Messaging;
using System.Windows;
using Microsoft.Win32;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System.Drawing;
using IronMan.Revit.DTO;

namespace IronMan.Revit.Services
{
    public class MaterialService : IMaterialService
    {
        private readonly IDataContext _dataContext;

        public MaterialService(IDataContext dataContext)
        {
            this._dataContext = dataContext;
        }

        public MaterialPlus CreateElement(DTO_Material dto)
        {
            Element element = null;
            _dataContext.GetDocument().NewTransaction(() =>
            {
                ElementId id = Material.Create(_dataContext.GetDocument(), dto.Name);
                element = _dataContext.GetDocument().GetElement(id);
            }, "创建材质");
            return new MaterialPlus(element as Material);
        }

        public IEnumerable<MaterialPlus> CreateElements(DTO_Material dto)
        {
            throw new NotImplementedException();
        }

        public void DeleteElement(MaterialPlus element)
        {
            if (element == null) return;
            _dataContext.GetDocument().NewTransaction(() =>
            {
                _dataContext.GetDocument().Delete(element.Id);
            }, "删除材质");
        }

        public void DeleteElements(IEnumerable<MaterialPlus> elements)
        {
            if (elements.Count() == 0) return;
            _dataContext.GetDocument().NewTransaction(() =>
            {
                foreach (var element in elements)
                {
                    Messenger.Default.Send<string>(element.Name, Contants.Tokens.ProgressBarTitle);
                    _dataContext.GetDocument().Delete(element.Id);
                    _dataContext.GetDocument().Regenerate();//传入每个实例//不加也行//事务完成后会进行全部生成，最后会卡一下
                }
            }, "删除材质");
        }

        public void Export(IEnumerable<MaterialPlus> elements)
        {
            var dg = new SaveFileDialog();
            dg.Filter = "*.xls|*.xls";
            dg.FileName = "C:\\User\\Administrator\\Desktop\\MaterialProxy";
            bool? result = dg.ShowDialog();
            string fileName;
            if (result == true)
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
            ICellStyle titleStyle = workbook.CreateCellStyle();
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

        public void Save(MaterialPlus materialPlus, MaterialPlus dtoMaterial)
        {
            if (dtoMaterial.Name == null) return;
            if (dtoMaterial.Color == null) dtoMaterial.Color = new RevitColor(255, 255, 255);
            if (dtoMaterial.AppearanceColor == null) dtoMaterial.AppearanceColor = new RevitColor(255, 255, 255);
            if (materialPlus.Name != dtoMaterial.Name)
            {
                materialPlus.Name = dtoMaterial.Name; 
                _dataContext.GetDocument().NewTransaction(() => materialPlus.Material.Name = dtoMaterial.Name);
            }
            if (materialPlus.Color != dtoMaterial.Color)
            {
                materialPlus.Color = dtoMaterial.Color;
                _dataContext.GetDocument().NewTransaction(() => materialPlus.Material.Color = dtoMaterial.Color);
            }
            if (materialPlus.AppearanceColor != dtoMaterial.AppearanceColor)
            {
                materialPlus.AppearanceColor = dtoMaterial.AppearanceColor;
                _dataContext.GetDocument().NewTransaction(() => materialPlus.SetAppearanceColor(dtoMaterial.AppearanceColor));
            }
            MessageBox.Show("nb");
        }

    }
}
