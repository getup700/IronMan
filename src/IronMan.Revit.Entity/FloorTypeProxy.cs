using Autodesk.Revit.DB;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace IronMan.Revit.Entity
{
    public class FloorTypeProxy : ElementProxy
    {
        private FloorType _floorType;

        public FloorTypeProxy(Element element) : base(element)
        {
            _floorType = element as FloorType;
            Layers = GetLayers(out _layerCount);
        }

        private ObservableCollection<CompoundStructureLayer> _layers;
        private int _layerCount;


        public int LayerCount
        {
            get { return _layerCount; }
            set { Set(ref _layerCount, value); }
        }


        public ObservableCollection<CompoundStructureLayer> Layers
        {
            get { return _layers; }
            set { Set(ref _layers, value); }
        }


        public FloorType FloorType
        {
            get { return _floorType; }
            set { Set(ref _floorType, value); }
        }



        private ObservableCollection<CompoundStructureLayer> GetLayers(out int layerCount)
        {
            var layerList = new ObservableCollection<CompoundStructureLayer>();
            var types = FloorType.GetCompoundStructure().GetLayers();
            foreach(var type in types)
            {
                layerList.Add(type);
            }
            layerCount = layerList.Count;
            return layerList;
        }
    }
}
