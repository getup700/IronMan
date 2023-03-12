using Autodesk.Revit.UI;
using IronMan.Revit.Toolkit.Extension;
using System;
using System.Collections.Generic;
using IronMan.Revit.Toolkit.Mvvm.Interfaces;
using IronMan.Revit.Commands;
using Autodesk.Revit.DB;
using IronMan.Revit.Properties;
using System.Linq;
using IronMan.Revit.Commands.PushButtons;
using IronMan.Revit.Entity;
using IronMan.Revit.Availabilities;
using UIFramework;

namespace IronMan.Revit
{
    public class AppUI : IApplicationUI
    {
        private readonly IUIProvider _uiProvider;
        private readonly string _tabName = "IronMan";

        public AppUI(IUIProvider uiProvider)
        {
            _uiProvider = uiProvider;
        }

        public List<RibbonPanel> RibbonPanels { get; set; }

        public Result Initial()
        {
            CreateTab();
            return CreatePanel();
        }

        public void CreateTab()
        {
            _uiProvider.GetUIApplication().CreateRibbonTab(_tabName);
        }

        public Result CreatePanel()
        {
            //RibbonPanel archiPanel = _uiProvider.GetUIApplication().CreateRibbonPanel(_tabName, "建筑");
            RibbonPanel mepPanel = _uiProvider.GetUIApplication().CreateRibbonPanel(_tabName, "机电");
            RibbonPanel globalPanel = _uiProvider.GetUIApplication().CreateRibbonPanel(_tabName, "全局");
            RibbonPanel developPanel = _uiProvider.GetUIApplication().CreateRibbonPanel(_tabName, "Develop");

            _uiProvider.GetUIApplication().AddPanel(_tabName, "建筑", (archiPanel) =>
            {
                archiPanel.CreateButton<ShowSelectInfoCommands>((b) =>
                {
                    b.Text = "属性窗口";
                    b.LargeImage = Resources.Materials.ConvertToBitmapSource();
                    b.ToolTip = "模态窗口、对话框";
                });
                archiPanel.CreateButton<MaterialsCommand>((b) =>
                {
                    b.Text = "材质管理";
                    b.LargeImage = Resources.Materials.ConvertToBitmapSource();
                    b.ToolTip = "模态窗口、对话框";
                });
                archiPanel.CreateButton<FloorTypeManageCommand>((b) =>
                {
                    b.Text = "楼板类型";
                    b.LargeImage = Resources.Materials.ConvertToBitmapSource();
                    b.ToolTip = "模态窗口";
                });
            });
            #region archiPanel


            #endregion

            #region mepPanel
            mepPanel.CreateButton<QuicklyWallCommand>((b) =>
            {
                b.Text = "QuicklyWall";
                b.LargeImage = Resources.Materials.ConvertToBitmapSource();
                b.ToolTip = "非模态窗口、外部事件";
            });
            mepPanel.CreateButton<SmartRoomCommand>((b) =>
            {
                b.Text = "SmartRoom";
                b.LargeImage = Resources.Materials.ConvertToBitmapSource();
                b.ToolTip = "非模态窗口、外部事件、DMU";
            });
            mepPanel.CreateButton<FormatPainterCommand>((b) =>
            {
                b.Text = "格式刷";
                b.LargeImage = Resources.Mep.ConvertToBitmapSource();
                b.ToolTip = "非模态窗口、外部事件、DMU";
            });
            mepPanel.CreateButton<SizeConvertCommand>((b) =>
            {
                b.Text = "尺寸转换";
                b.LargeImage = Resources.Materials.ConvertToBitmapSource();
                b.ToolTip = "非模态窗口、外部事件、DMU";
            });
            #endregion

            #region globalPanel
            PulldownButton pulldownButton = globalPanel.CreatePulldownButton(() =>
            {
                var data = new PulldownButtonData("name", "PanelName")
                {
                    Image = Resources.PushButton32.ConvertToBitmapSource(),
                    LargeImage = Resources.PushButton16.ConvertToBitmapSource(),
                };
                return data;
            });
            pulldownButton.AddPushButton(UIExtension.CreatePushButtonData<ShieldF1Command>(b =>
            {
                b.Text = "CadToRevit";
                b.LargeImage = Resources.Materials.ConvertToBitmapSource();
                b.ToolTip = "没有窗口";
            }));
            pulldownButton.AddPushButton(UIExtension.CreatePushButtonData<DockablePaneCommand>(b =>
            {
                b.Text = "DockablePane";
                b.LargeImage = Resources.Materials.ConvertToBitmapSource();
                b.ToolTip = "可停靠窗口";
            }));
            pulldownButton.AddSeparator();
            pulldownButton.AddPushButton(UIExtension.CreatePushButtonData<DataCollectCommand>(b =>
            {
                b.Text = "房间导出";
                b.LargeImage = Resources.Manager.ConvertToBitmapSource();
                b.ToolTip = "导出房间内墙体长度及房间面积";
            }));
            var radioButtonGroup = globalPanel.CreateRadioButtonGroup();
            radioButtonGroup.AddToggleButtonData<DockablePaneCommand>(() =>
            {
                var data = new ToggleButtonData("radioButton", "RadioButton")
                {
                    Image = Resources.PushButton16.ConvertToBitmapSource(),
                    LargeImage = Resources.PushButton32.ConvertToBitmapSource()
                };
                return data;
            });
            radioButtonGroup.AddToggleButtonData<SetParameterFilterColorCommand>(() =>
            {
                var data = new ToggleButtonData("setParameterFilterColorCommand", "SetParameterFilterColorCommand")
                {
                    Image = Resources.PushButton16.ConvertToBitmapSource(),
                    LargeImage = Resources.PushButton32.ConvertToBitmapSource()
                };
                return data;
            });

            //ToggleButton toggleButton = globalPanel.CreateToggleButton<DockablePaneCommand>((b) =>
            //{
            //    b.Image = Resources.PushButton32.ConvertToBitmapSource();
            //    //b.LargeImage = Resources.PushButton16.ConvertToBitmapSource();
            //    b.ToolTipImage = Resources.PushButton16.ConvertToBitmapSource();
            //    b.ToolTip = "this is a toolTip";
            //});
            //globalPanel.CreateButton<ShieldF1Command>((b) =>
            //{
            //    b.Text = "CadToRevit";
            //    b.LargeImage = Resources.Materials.ConvertToBitmapSource();
            //    b.ToolTip = "没有窗口";
            //});
            //globalPanel.CreateButton<DockablePaneCommand>((b) =>
            //{
            //    b.Text = "DockablePane";
            //    b.LargeImage = Resources.Materials.ConvertToBitmapSource();
            //    b.ToolTip = "可停靠窗口";
            //});
            //globalPanel.CreateButton<DataCollectCommand>((b) =>
            //{
            //    b.Text = "房间导出";
            //    b.LargeImage = Resources.Manager.ConvertToBitmapSource();
            //    b.ToolTip = "导出房间内墙体长度及房间面积";
            //});

            globalPanel.CreateButton<CreateSchemaCommand>((b) =>
            {
                b.Text = "AttachData";
                b.LargeImage = Resources.Materials.ConvertToBitmapSource();
                b.ToolTip = "Data hook on a document";
            });

            globalPanel.CreateButton<SetParameterFilterColorCommand>((b) =>
            {
                b.Text = "清除填充";
                b.LargeImage = Resources.Develop.ConvertToBitmapSource();
                b.Image = Resources.PushButton16.ConvertToBitmapSource();
                //b.AvailabilityClassName = typeof(OnlyProjectAvailability).FullName;
                b.ToolTip = "鼠标指针放上去就会显示";
                b.LongDescription = "鼠标指针停放1-2s后可触发显示";
                b.ToolTipImage = Resources.Develop.ConvertToBitmapSource();
            });

            globalPanel.CreateStackedItems<DockablePaneCommand, DataCollectCommand, CreateSchemaCommand>(a =>
            {
                a.Text = "DockablePane";
                a.LargeImage = Resources.Materials.ConvertToBitmapSource();
                a.ToolTip = "可停靠窗口";
            }, b =>
            {
                b.Text = "房间导出";
                b.LargeImage = Resources.Manager.ConvertToBitmapSource();
                b.ToolTip = "导出房间内墙体长度及房间面积";
            }, c =>
            {
                c.Text = "AttachData";
                c.LargeImage = Resources.Materials.ConvertToBitmapSource();
                c.ToolTip = "Data hook on a document";
            });
            globalPanel.AddSeparator();

            TextBox textBox = globalPanel.CreateTextBox((t) =>
            {
                t.Image = Resources.Develop.ConvertToBitmapSource();
            });
            textBox.PromptText = "Enter your text";
            //textBox.ItemText = "this is a itemText";
            textBox.EnterPressed += (s, e) =>
            {
                if (s is TextBox textBox)
                {
                    TaskDialog.Show("Title", textBox.Value?.ToString());
                    //e.Application.ActiveUIDocument.Document;
                }
            };
            //增加一个点击按钮进行输入
            textBox.ShowImageAsButton = true;
            //聚焦后选中
            textBox.SelectTextOnFocus = true;

            ComboBox comboBox = globalPanel.CreateComboBox((t) =>
            {
                t.Name = "ComboBoxName";
                t.Image = Resources.Develop.ConvertToBitmapSource();
            });
            ComboBoxMemberData firstComboBoxMemberData = new ComboBoxMemberData("firstComboBox", "FirstMember")
            {
                GroupName = "GroupOne",
                Image = Resources.Develop.ConvertToBitmapSource()
            };
            ComboBoxMemberData secondComboBoxMemberData = new ComboBoxMemberData("secondComboBox", "SecondMember")
            {
                GroupName = "GroupOne",
                Image = Resources.Develop.ConvertToBitmapSource()
            };
            ComboBoxMemberData thirdComboBoxMemberData = new ComboBoxMemberData("thirdComboBox", "ThirdMember")
            {
                GroupName = "GroupTwo",
                Image = Resources.Develop.ConvertToBitmapSource()
            };
            ComboBoxMember comboBoxMember1 = comboBox.AddItem(firstComboBoxMemberData);
            ComboBoxMember comboBoxMember2 = comboBox.AddItem(secondComboBoxMemberData);
            ComboBoxMember comboBoxMember3 = comboBox.AddItem(thirdComboBoxMemberData);

            comboBox.CurrentChanged += (sender, evenArgs) =>
            {
                textBox.ItemText = sender.ToString();
                TaskDialog.Show("Title", "ComboBox Changed");
            };
            #endregion

            #region IronMan.Revit.Commands.PulldownButtons
            var btn1 = new PushButtonDataProxy(typeof(BackgroundConvertCommand));
            developPanel.AddItem(btn1.ConvertRevitButton());
            developPanel.AddItem(new PushButtonDataProxy(typeof(SetParameterFilterColorCommand)).ConvertRevitButton());

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

            #region developPanel
            developPanel.CreateButton<ModeCommand>((b) =>
            {
                b.Text = "模态窗口";
                b.LargeImage = Resources.Materials.ConvertToBitmapSource();
                b.ToolTip = "模态窗口、线程影响因素";
            });
            developPanel.CreateButton<FilterCommand>((b) =>
            {
                b.Text = "Filter";
                b.LargeImage = Resources.Develop.ConvertToBitmapSource();
                b.ToolTip = "This is a Test Command";
            });
            developPanel.CreateButton<XRayCommand>((b) =>
            {
                b.Text = "X-Ray";
                b.LargeImage = Resources.Develop.ConvertToBitmapSource();
                b.ToolTip = "This is a Test Command";
            });
            developPanel.CreateButton<ParameterFilterCommand>((b) =>
            {
                b.Text = "过滤器管理";
                b.LargeImage = Resources.Develop.ConvertToBitmapSource();
                b.ToolTip = "This is a Test Command";
                b.SetContextualHelp(new ContextualHelp(ContextualHelpType.ChmFile, "https://www.baidu.com/"));
            });
            developPanel.AddSlideOut();
            developPanel.CreateButton<FunctionTestCommand>((b) =>
            {
                b.Text = "Test";
                b.LargeImage = Resources.Develop.ConvertToBitmapSource();
                b.ToolTip = "Test";
            });
            developPanel.CreateButton<InformationImportCommand>((b) =>
            {
                b.Text = "导入信息";
                b.LargeImage = Resources.DataImport.ConvertToBitmapSource();
                b.ToolTip = "Test";
            });
            developPanel.CreateButton<InformationExportCommand>((b) =>
            {
                b.Text = "导出信息";
                b.LargeImage = Resources.DataExport.ConvertToBitmapSource();
                b.ToolTip = "Test";
            });
            developPanel.CreateButton<FailureTestCommand>((b) =>
            {
                b.Text = "FailureTest";
                b.LargeImage = Resources.DataExport.ConvertToBitmapSource();
                b.ToolTip = "Test";
            });

            #endregion

            return Result.Succeeded;
        }
    }
}
