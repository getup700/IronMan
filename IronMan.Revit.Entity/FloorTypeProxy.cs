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
    public class FloorTypeProxy : ObservableObject
    {
        public FloorTypeProxy(FloorType floorType)
        {
            FloorType = floorType;
            Name = floorType.Name;
            Layers =GetLayers(out _layerCount);
        }

        private string _name;
        private FloorType _floorType;
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


        public string Name
        {
            get { return _name; }
            set { Set(ref _name, value); }
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
