using IronMan.Revit.Entity;
using Toolkit.Extension;
using GalaSoft.MvvmLight.Command;
using IronMan.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using Autodesk.Revit.DB;
using GalaSoft.MvvmLight.Messaging;
using IronMan.IServices;

namespace IronMan.ViewModels
{
    public class MaterialDialogViewModel : ViewModelBase
    {
        private readonly IMaterialService _service;
        //作为数据缓存而创建的属性
        private string _name;
        private Color _color;
        private Color _appearanceColor;

        public MaterialPlus MaterialPlus { get; set; }

        public Color AppearanceColor
        {
            get => _appearanceColor;
            set => Set(ref _appearanceColor, value);
        }

        public Color Color
        {
            get => _color;
            set => Set(ref _color, value);
        }

        public string Name
        {
            get => _name;
            set => Set(ref _name, value);
        }

        public MaterialDialogViewModel(IMaterialService service)
        {
            this._service = service;
        }

        public void Initial(object sender)
        {
            if (sender != null)
            {
                MaterialPlus = (MaterialPlus)sender;
                //后台数据临时存放到缓冲属性
                Name = MaterialPlus.Name;
                Color = MaterialPlus.Color;
                AppearanceColor = MaterialPlus.AppearanceColor;
            }
        }

        public RelayCommand SetColorCommand
        {
            get => new RelayCommand(() =>
            {
                System.Windows.Forms.ColorDialog colorDialog = new System.Windows.Forms.ColorDialog();
                if (colorDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    Color = colorDialog.Color.ConvertToRevitColor();
                }
            });
        }

        public RelayCommand SetAppearanceColorCommand
        {
            get => new RelayCommand(() =>
            {
                System.Windows.Forms.ColorDialog colorDialog = new System.Windows.Forms.ColorDialog();
                if (colorDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    AppearanceColor = colorDialog.Color.ConvertToRevitColor();
                }
            });
        }

        public RelayCommand SubmitCommand
        {
            get => new RelayCommand(() =>
            {
                if (MaterialPlus == null)
                {
                    MaterialPlus = this._service.CreateElement(Name);
                }
                Save();
                //if (_message.Notification == "Create")
                //{
                //    Document doc = _message.Target as Document;
                //    doc.NewTransaction(() =>
                //    {
                //        ElementId id = Material.Create(doc, Name);
                //        MaterialPlus = new MaterialPlus(doc.GetElement(id) as Material);
                //        Save();
                //    });
                //    _message.Execute(MaterialPlus);//huidiao
                //}
                //if(_message.Notification =="Edit")
                //{
                //    Document doc = _message.Target as Document;
                //    doc.NewTransaction(() =>
                //    {
                //        Save(); 
                //    });
                //}

                MessengerInstance.Send(true, Contacts.Tokens.MaterialDialogWindow);
            });
        }

        private void Save()
        {
            if (MaterialPlus.Name != Name)
            {
                MaterialPlus.Name = Name;
            }
            if (MaterialPlus.Color != Color)
            {
                MaterialPlus.Color = Color;
            }
            if (MaterialPlus.AppearanceColor != AppearanceColor)
            {
                MaterialPlus.AppearanceColor = AppearanceColor;
            }
        }

    }
}
