﻿using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Visual;
using IronMan.Revit.Toolkit.Extension;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace IronMan.Revit.Entity
{
    public class MaterialPlus : ElementProxy
    {
        public MaterialPlus(Material material) :base (material)
        {
            _material = material;
            _color = material.Color;
            _appearanceColor = GetAppearanceColor();
        }

        public MaterialPlus()
        {

        }


        private Color _color;
        private Color _appearanceColor;
        private Material _material;

        public Document Doc { get => Material.Document; }

        public Material Material
        {
            get => _material;
            private set => _material = value;
        }



        public Color AppearanceColor
        {
            get => _appearanceColor;
            set
            {
                SetProperty(ref _appearanceColor, value);
                //Doc.NewTransaction(() => SetAppearanceColor(_appearanceColor));
            }
        }

        public Color Color
        {
            get => _color;
            set
            {
                SetProperty(ref _color, value);
               // Doc.NewTransaction(() => Material.Color = _color);
            }
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
                    //AssetPropertyDoubleArray4d property = asset?.FindByName("generic_diffuse") as AssetPropertyDoubleArray4d;
                    //return property?.GetValueAsColor();
                }
            }
            return null;
        }


        public void SetAppearanceColor(Color color)
        {
            ElementId id = Material.AppearanceAssetId;
            if (id != null && id.IntegerValue != -1)
            {
                //using (AppearanceAssetEditScope scope = new AppearanceAssetEditScope(Doc))
                //{
                //    Asset asset = scope.Start(id);
                //    GetColorProperty(asset)?.SetValueAsColor(color);
                //    scope.Commit(true);
                //}
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
