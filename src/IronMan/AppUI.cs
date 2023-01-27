using Autodesk.Revit.UI;
using IronMan.Revit.Toolkit.Extension;
using System;
using System.Collections.Generic;
using System.Text;
using IronMan.Revit.Toolkit.Mvvm.Interfaces;
using System.Windows.Forms.PropertyGridInternal;
using IronMan.Revit.Commands;
using Autodesk.Revit.DB;
using IronMan.Revit.Properties;
using NPOI.SS.Formula.Functions;
using System.Linq;
using IronMan.Revit.Commands.PushButtons;
using IronMan.Revit.Entity;
using IronMan.Revit.Commands.OfficalCommand;

namespace IronMan.Revit
{
    public class AppUI : IApplicationUI
    {
        private readonly IUIProvider _uiProvider;

        public AppUI(IUIProvider uiProvider)
        {
            _uiProvider = uiProvider;
        }

        public Result Initial()
        {
            const string _tab = "IronMan";
            _uiProvider.GetUIApplication().CreateRibbonTab(_tab);

            RibbonPanel panel1 = _uiProvider.GetUIApplication().CreateRibbonPanel(_tab, "模态窗口");
            RibbonPanel panel2 = _uiProvider.GetUIApplication().CreateRibbonPanel(_tab, "非模态窗口");
            RibbonPanel panel3 = _uiProvider.GetUIApplication().CreateRibbonPanel(_tab, "Other");
            RibbonPanel panel0 = _uiProvider.GetUIApplication().CreateRibbonPanel(_tab, "Develop");

            #region Panel1
            panel1.CreateButton<MaterialsCommand>((b) =>
            {
                b.Text = "材质管理";
                b.LargeImage = Resources.Materials.ConvertToBitmapSource();
                b.ToolTip = "模态窗口、对话框";
            });
            panel1.CreateButton<FloorTypeManageCommand>((b) =>
            {
                b.Text = "楼板类型";
                b.LargeImage = Resources.Materials.ConvertToBitmapSource();
                b.ToolTip = "模态窗口";
            });

            #endregion

            #region Panel2
            panel2.CreateButton<QuicklyWallCommand>((b) =>
            {
                b.Text = "QuicklyWall";
                b.LargeImage = Resources.Materials.ConvertToBitmapSource();
                b.ToolTip = "非模态窗口、外部事件";
            });
            panel2.CreateButton<SmartRoomCommand>((b) =>
            {
                b.Text = "SmartRoom";
                b.LargeImage = Resources.Materials.ConvertToBitmapSource();
                b.ToolTip = "非模态窗口、外部事件、DMU";
            });
            panel2.CreateButton<FormatPainterCommand>((b) =>
            {
                b.Text = "格式刷";
                b.LargeImage = Resources.Mep.ConvertToBitmapSource();
                b.ToolTip = "非模态窗口、外部事件、DMU";
            });
            panel2.CreateButton<SizeConvertCommand>((b) =>
            {
                b.Text = "尺寸转换";
                b.LargeImage = Resources.Materials.ConvertToBitmapSource();
                b.ToolTip = "非模态窗口、外部事件、DMU";
            });

            #endregion

            #region Panel3
            panel3.CreateButton<CadToRevitCommand>((b) =>
            {
                b.Text = "CadToRevit";
                b.LargeImage = Resources.Materials.ConvertToBitmapSource();
                b.ToolTip = "没有窗口";
            });
            panel3.CreateButton<DockablePaneCommand>((b) =>
            {
                b.Text = "DockablePane";
                b.LargeImage = Resources.Materials.ConvertToBitmapSource();
                b.ToolTip = "可停靠窗口";
            });
            panel3.CreateButton<DataCollectCommand>((b) =>
            {
                b.Text = "房间导出";
                b.LargeImage = Resources.Manager.ConvertToBitmapSource();
                b.ToolTip = "导出房间内墙体长度及房间面积";
            });
            panel3.CreateButton<CreateSchemaCommand>((b) =>
            {
                b.Text = "AttachData";
                b.LargeImage = Resources.Materials.ConvertToBitmapSource();
                b.ToolTip = "Data hook on a document";
            });
            panel3.CreateButton<SetParameterFilterColorCommand>((b) =>
            {
                b.Text = "清除填充";
                b.LargeImage = Resources.Develop.ConvertToBitmapSource();
                b.ToolTip = "This is a Test Command";
            });

            #endregion

            #region IronMan.Revit.Commands.PulldownButtons
            var btn1 = new PushButtonDataProxy(typeof(BackgroundConvertCommand));
            panel0.AddItem(btn1.ConvertRevitButton());
            panel0.AddItem(new PushButtonDataProxy(typeof(SetParameterFilterColorCommand)).ConvertRevitButton());

            //var pullDownButtonData = new PulldownButtonData("工具集", "text")
            //{
            //    Image = Resources.PushButton16.ConvertToBitmapSource(),
            //    LargeImage = Resources.PushButton32.ConvertToBitmapSource()
            //};
            //var pullDownButton = panel3.AddItem(pullDownButtonData) as PulldownButton;

            //var typePullDownButton = typeof(AppUI).Assembly.GetTypes().Where(x => x.IsClass && x.IsPublic && x.Namespace == "IronMan.Revit.Commands.PushButtons");
            //foreach (var type in typePullDownButton)
            //{
            //    panel0.AddItem(type.get)
            //};

            #endregion

            #region Panel0
            panel0.CreateButton<ModeCommand>((b) =>
            {
                b.Text = "模态窗口";
                b.LargeImage = Resources.Materials.ConvertToBitmapSource();
                b.ToolTip = "模态窗口、线程影响因素";
            });
            panel0.CreateButton<FilterCommand>((b) =>
            {
                b.Text = "Filter";
                b.LargeImage = Resources.Develop.ConvertToBitmapSource();
                b.ToolTip = "This is a Test Command";
            });
            panel0.CreateButton<XRayCommand>((b) =>
            {
                b.Text = "X-Ray";
                b.LargeImage = Resources.Develop.ConvertToBitmapSource();
                b.ToolTip = "This is a Test Command";
            });
            panel0.CreateButton<ParameterFilterCommand>((b) =>
            {
                b.Text = "过滤器管理";
                b.LargeImage = Resources.Develop.ConvertToBitmapSource();
                b.ToolTip = "This is a Test Command";
            });
            panel0.CreateButton<FunctionTestCommand>((b) =>
            {
                b.Text = "Test";
                b.LargeImage = Resources.Develop.ConvertToBitmapSource();
                b.ToolTip = "Test";
            });
            panel0.CreateButton<InformationImportCommand>((b) =>
            {
                b.Text = "导入信息";
                b.LargeImage = Resources.DataImport.ConvertToBitmapSource();
                b.ToolTip = "Test";
            });
            panel0.CreateButton<InformationExportCommand>((b) =>
            {
                b.Text = "导出信息";
                b.LargeImage = Resources.DataExport.ConvertToBitmapSource();
                b.ToolTip = "Test";
            });
            #endregion

            return Result.Succeeded;
        }
    }
}
