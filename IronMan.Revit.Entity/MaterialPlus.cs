using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Visual;
using Toolkit.Extension;
using System;
using System.ComponentModel;
using GalaSoft.MvvmLight;

namespace IronMan.Revit.Entity
{
    public class MaterialPlus : ObservableObject
    {
        private string _name;
        private Color _color;
        private Color _appearanceColor;
        private ElementId _id;
        private Material _material;

        public Document Doc { get => Material.Document; }

        public Material Material
        {
            get => _material;
            private set => _material = value;
        }

        public ElementId Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public Color AppearanceColor
        {
            get => _appearanceColor;
            set
            {
                _appearanceColor = value;
                RaisePropertyChanged();
                Doc.NewTransaction(() => SetAppearanceColor(_appearanceColor));
            }
        }

        public Color Color
        {
            get => _color;
            set
            {
                Set(ref _color, value);
                Doc.NewTransaction(() => Material.Color = _color);
            }
        }

        public string Name
        {
            get => _name;
            set
            {
                Set(ref _name, value);
                Doc.NewTransaction(() => Material.Name = _name);
            }
        }

        public MaterialPlus(Material material)
        {
            _material = material;
            _id = material.Id;
            _name = material.Name;
            _color = material.Color;
            _appearanceColor = GetAppearanceColor();
        }

        private Color GetAppearanceColor()
        {
            ElementId id = Material.AppearanceAssetId;
            if (id != null && id.IntegerValue != -1)
            {
                AppearanceAssetElement appearanceAssetElement = Doc.GetElement(id) as AppearanceAssetElement;
                Asset asset = appearanceAssetElement.GetRenderingAsset();
                if (asset.Size != 0)
                {
                    AssetPropertyDoubleArray4d property = asset?.FindByName("generic_diffuse") as AssetPropertyDoubleArray4d;
                    return property?.GetValueAsColor();
                }
            }
            return null;
        }

        private AssetPropertyDoubleArray4d GetColorProperty(Asset asset)
        {
            return (AssetPropertyDoubleArray4d)asset?.FindByName("generic_diffuse");
        }

        private void SetAppearanceColor(Color color)
        {
            ElementId id = Material.AppearanceAssetId;
            if (id != null && id.IntegerValue != -1)
            {
                using (AppearanceAssetEditScope scope = new AppearanceAssetEditScope(Doc))
                {
                    Asset asset = scope.Start(id);
                    GetColorProperty(asset)?.SetValueAsColor(color);
                    scope.Commit(true);
                }
            }
        }

        //public void Save()
        //{
        //    Doc.NewTransaction(() =>
        //    {
        //        Material.Name = _name;
        //        Material.Color = _color;
        //        SetAppearanceColor(this._appearanceColor);
        //    },"修改材质");
        //}
    }
}
