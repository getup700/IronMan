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
using IronMan.Revit.Commands.Test;
using IronMan.Revit.Commands.Test.ExtensibleStorage;
using IronMan.Revit.Commands.Test.Cad2Revit;

namespace IronMan.Revit
{
    public class AppUI : IApplicationUI
    {
        private readonly IUIProvider _uiProvider;
        private readonly string _tabName = "IronMan";
        private readonly string _archiPanelName = "建筑";
        private readonly string _mepPanelName = "机电";
        private readonly string _globalPanelName = "全局";
        private readonly string _cadPanelName = "CAD识别";
        private readonly string _devPanelName = "Develop";

        public AppUI(IUIProvider uiProvider)
        {
            _uiProvider = uiProvider;
        }

        public List<RibbonPanel> RibbonPanels { get; set; }

        public void Initial()
        {
            CreateTab();

            InitialArchiPanel();
            InitialMepPanel();
            InitialCadPanel();
            InitialDevelopPanel();
            InitialGlobalPanel();
        }

        public void CreateTab()
        {
            _uiProvider.GetUIApplication().CreateRibbonTab(_tabName);
        }

        private void InitialArchiPanel()
        {
            var panel = _uiProvider.GetUIApplication().CreateRibbonPanel(_tabName, _archiPanelName);

            panel.CreateButton<FloorTypeManageCommand>((b) =>
            {
                b.Text = "楼板类型";
                b.LargeImage = Resources.Materials.ConvertToBitmapSource();
                b.ToolTip = "模态窗口";
            });
            panel.CreateButton<QuicklyWallCommand>((b) =>
            {
                b.Text = "QuicklyWall";
                b.LargeImage = Resources.Materials.ConvertToBitmapSource();
                b.ToolTip = "非模态窗口、外部事件";
            });
        }

        private void InitialMepPanel()
        {
            var panel = _uiProvider.GetUIApplication().CreateRibbonPanel(_tabName, _mepPanelName);

            panel.CreateButton<SmartRoomCommand>((b) =>
            {
                b.Text = "SmartRoom";
                b.LargeImage = Resources.Materials.ConvertToBitmapSource();
                b.ToolTip = "非模态窗口、外部事件、DMU";
            });
            panel.CreateButton<FormatPainterCommand>((b) =>
            {
                b.Text = "格式刷";
                b.LargeImage = Resources.Mep.ConvertToBitmapSource();
                b.ToolTip = "非模态窗口、外部事件、DMU";
            });
            panel.CreateButton<SizeConvertCommand>((b) =>
            {
                b.Text = "尺寸转换";
                b.LargeImage = Resources.Materials.ConvertToBitmapSource();
                b.ToolTip = "非模态窗口、外部事件、DMU";
            });
        }

        private void InitialGlobalPanel()
        {
            var panel = _uiProvider.GetUIApplication().CreateRibbonPanel(_tabName, _globalPanelName);
            PulldownButton pulldownButton = panel.CreatePulldownButton(data =>
            {
                data.Text = "设置";
                data.Name = "btn_设置";
                data.Image = Resources.PushButton32.ConvertToBitmapSource();
                data.LargeImage = Resources.PushButton16.ConvertToBitmapSource();

            });
            pulldownButton.AddPushButton(UIExtension.CreatePushButtonData<ShieldF1Command>(b =>
            {
                b.Text = "屏蔽F1";
                b.LargeImage = Resources.Materials.ConvertToBitmapSource();
                b.ToolTip = "没有窗口";
            }));
            pulldownButton.AddPushButton(UIExtension.CreatePushButtonData<BackgroundConvertCommand>(b =>
            {
                b.Text = "翻转背景";
                b.LargeImage = Resources.Materials.ConvertToBitmapSource();
                b.ToolTip = "没有窗口";
            }));

            pulldownButton.AddSeparator();



            var radioButtonGroup = panel.CreateRadioButtonGroup();
            radioButtonGroup.AddToggleButtonData<DockablePaneCommand>(data =>
            {
                data.Text = "ToggleButton1";
                data.Name = "ToggleButton1";
                data.Image = Resources.Materials.ConvertToBitmapSource();
            }); 
            radioButtonGroup.AddToggleButtonData<DockablePaneCommand>(data =>
            {
                data.Text = "ToggleButton2";
                data.Name = "ToggleButton1";
                data.Image = Resources.Materials.ConvertToBitmapSource();
            });
            panel.AddSeparator();
            panel.CreateButton<ParameterFilterCommand>((b) =>
            {
                b.Text = "过滤器管理";
                b.LargeImage = Resources.Develop.ConvertToBitmapSource();
                b.ToolTip = "This is a Test Command";
                b.SetContextualHelp(new ContextualHelp(ContextualHelpType.ChmFile, "https://www.baidu.com/"));
            });

            panel.CreateButton<SetParameterFilterColorCommand>((b) =>
            {
                b.Text = "清除填充";
                b.LargeImage = Resources.Develop.ConvertToBitmapSource();
                b.Image = Resources.PushButton16.ConvertToBitmapSource();
                //b.AvailabilityClassName = typeof(OnlyProjectAvailability).FullName;
                b.ToolTip = "鼠标指针放上去就会显示";
                b.LongDescription = "鼠标指针停放1-2s后可触发显示";
                b.ToolTipImage = Resources.Develop.ConvertToBitmapSource();
            });

            panel.CreateStackedItems<DockablePaneCommand, DataCollectCommand, CreateSchemaCommand>(a =>
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
            panel.AddSeparator();

            TextBox textBox = panel.CreateTextBox((t) =>
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

            ComboBox comboBox = panel.CreateComboBox((t) =>
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
        }

        private void InitialCadPanel()
        {
            var panel = _uiProvider.GetUIApplication().CreateRibbonPanel(_tabName, _cadPanelName);
            panel.CreateButton<ConvertCAD2Revit>(b =>
            {
                b.Text = "智能转换";
                b.LargeImage = Resources.Materials.ConvertToBitmapSource();
                b.ToolTip = "智能转换墙，梁，柱，板";
            });

            var button = panel.CreatePulldownButton(data =>
              {
                  data.Text = "创建墙";
                  data.LargeImage = Resources.Materials.ConvertToBitmapSource();
              });
        }

        private void InitialDevelopPanel()
        {
            var panel = _uiProvider.GetUIApplication().CreateRibbonPanel(_tabName, _devPanelName);

            panel.CreateStackedItems<CreateSchemaCommand, AttachDataCommand, DeleteSchemaCommand>(
                btn1 =>
                {
                    btn1.Text = "创建schema";
                    btn1.LargeImage = Resources.Materials.ConvertToBitmapSource();
                    btn1.ToolTip = "模态窗口、线程影响因素";
                },
                btn2 =>
                {
                    btn2.Text = "挂接数据";
                    btn2.LargeImage = Resources.Materials.ConvertToBitmapSource();
                    btn2.ToolTip = "Data hook on a document";
                },
                btn3 =>
                {
                    btn3.Text = "删除schema";
                    btn3.LargeImage = Resources.Materials.ConvertToBitmapSource();
                    btn3.ToolTip = "模态窗口、线程影响因素";
                });

            panel.CreateButton<IdingCommand>((b) =>
            {
                b.Text = "空闲事件";
                b.LargeImage = Resources.Materials.ConvertToBitmapSource();
                b.ToolTip = "模态窗口、线程影响因素";
            });
            panel.CreateButton<SubTransactionCommand>((b) =>
            {
                b.Text = "事务管理";
                b.LargeImage = Resources.Materials.ConvertToBitmapSource();
                b.ToolTip = "模态窗口、线程影响因素";
            });
            panel.CreateButton<ModeCommand>((b) =>
            {
                b.Text = "模态窗口";
                b.LargeImage = Resources.Materials.ConvertToBitmapSource();
                b.ToolTip = "模态窗口、线程影响因素";
            });
            panel.CreateButton<FilterCommand>((b) =>
            {
                b.Text = "Filter";
                b.LargeImage = Resources.Develop.ConvertToBitmapSource();
                b.ToolTip = "This is a Test Command";
            });
            panel.CreateButton<XRayCommand>((b) =>
            {
                b.Text = "X-Ray";
                b.LargeImage = Resources.Develop.ConvertToBitmapSource();
                b.ToolTip = "This is a Test Command";
            });
            panel.AddSlideOut();
            panel.CreateButton<FunctionTestCommand>((b) =>
            {
                b.Text = "Test";
                b.LargeImage = Resources.Develop.ConvertToBitmapSource();
                b.ToolTip = "Test";
            });
            panel.CreateButton<InformationImportCommand>((b) =>
            {
                b.Text = "导入信息";
                b.LargeImage = Resources.DataImport.ConvertToBitmapSource();
                b.ToolTip = "Test";
            });
            panel.CreateButton<InformationExportCommand>((b) =>
            {
                b.Text = "导出信息";
                b.LargeImage = Resources.DataExport.ConvertToBitmapSource();
                b.ToolTip = "Test";
            });
            panel.CreateButton<FailureTestCommand>((b) =>
            {
                b.Text = "FailureTest";
                b.LargeImage = Resources.DataExport.ConvertToBitmapSource();
                b.ToolTip = "Test";
            });
            //var btn1 = new PushButtonDataProxy(typeof(BackgroundConvertCommand));
            //panel.AddItem(btn1.ConvertRevitButton());
            //panel.AddItem(new PushButtonDataProxy(typeof(SetParameterFilterColorCommand)).ConvertRevitButton());



        }
    }
}
