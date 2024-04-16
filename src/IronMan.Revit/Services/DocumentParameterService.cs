using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;
using Autodesk.Revit.UI;
using IronMan.Revit.Entity.Profiles;
using IronMan.Revit.Toolkit.Extension;
using IronMan.Revit.Toolkit.Mvvm.Interfaces;
using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IronMan.Revit.Services
{
    public class DocumentParameterService
    {
        private IDataContext _dataContext;

        public DocumentParameterService(IDataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public void GetParameters()
        {
            BindingMap bindingMap = _dataContext.GetDocument().ParameterBindings;
            //var iterator = bindingMap.GetEnumerator();
            //while (iterator.MoveNext())
            //{
            //    var item = iterator.Current as Definition;
            //}
            string info = string.Empty;
            int count = 0;
            DefinitionBindingMapIterator iterator = bindingMap.ReverseIterator();
            while (iterator.MoveNext())
            {
                count++;
                Definition definition = iterator.Key;
                info += definition.Name + "\n";
                InstanceBinding instanceBinding = iterator.Current as InstanceBinding;
                if (instanceBinding != null)
                {
                    CategorySet categorySet = instanceBinding.Categories;
                }
            }
            TaskDialog.Show("title", info.Insert(0, count.ToString() + "\n"));
        }

        public bool CheckParameter()
        {
            var rooms = _dataContext.GetDocument().GetElementsByCategoryId<Room>(new ElementId(-2000160), null);
            if (rooms.Count() == 0)
            {
                return false;
            }
            DecorationParameterProfile decoration = new DecorationParameterProfile();
            Room room = rooms.FirstOrDefault();
            var parameters = new List<Parameter>()
            {
                room.GetParameters(decoration.RoomName.Name).ToList().FirstOrDefault(x => decoration.RoomName.Equal(x.Definition)),
                room.GetParameters(decoration.RoomCode.Name).ToList().FirstOrDefault(x => decoration.RoomCode.Equal(x.Definition)),
                room.GetParameters(decoration.GroundMethodCode.Name).ToList().FirstOrDefault(x => decoration.GroundMethodCode.Equal(x.Definition)),
                room.GetParameters(decoration.GroundThickness.Name).ToList().FirstOrDefault(x => decoration.GroundThickness.Equal(x.Definition)),
                room.GetParameters(decoration.InteriorWallMethodCode.Name).ToList().FirstOrDefault(x => decoration.InteriorWallMethodCode.Equal(x.Definition)),
                room.GetParameters(decoration.CeilingMethodCode.Name).ToList().FirstOrDefault(x => decoration.CeilingMethodCode.Equal(x.Definition)),
                room.GetParameters(decoration.CeilingHeight.Name).ToList().FirstOrDefault(x => decoration.CeilingHeight.Equal(x.Definition)),
                room.GetParameters(decoration.BaseBoardMethodCode.Name).ToList().FirstOrDefault(x => decoration.BaseBoardMethodCode.Equal(x.Definition)),
                room.GetParameters(decoration.BaseBoardHeight.Name).ToList().FirstOrDefault(x => decoration.BaseBoardHeight.Equal(x.Definition))
            };

            if (parameters.Any(x => x == null))
            {
                MessageBox.Show("房间的项目参数或参数类型不匹配，请检查后重试");
                return false;
            }

            return true;
        }

        public void ImportExcel()
        {
            string fileName = string.Empty;
            OpenFileDialog dlg = new OpenFileDialog()
            {
                Title = "请选择导入的信息",
                Filter = "Excel|*.xls|所有文档|*.*",
                Multiselect = false
            };

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                fileName = dlg.FileName;
            }

        }
    }
}
