using Autodesk.Revit.DB;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Toolkit.Extension;

namespace IronMan.Revit.Entity
{

    public class FloorProxy : ElementProxy
    {
        public FloorProxy(Floor floor):base (floor)
        {
            _floor = floor;
            _level = GetLevel();
            _offset = GetOffset();

        }
        private Floor _floor;
        private Level _level;
        private double _offset;

        public double Offset
        {
            get { return _offset; }
            set { Set(ref _offset, value); }
        }

        public Level Level
        {
            get { return _level; }
            set { Set(ref _level, value); }
        }

        public Floor Floor
        {
            get => _floor;
            set => Set(ref _floor, value);
        }


        private Level GetLevel()
        {
            return base.Document.GetElement(Floor.LevelId) as Level;
        }

        private double GetOffset()
        {
            return Floor.get_Parameter(BuiltInParameter.FLOOR_HEIGHTABOVELEVEL_PARAM).AsDouble().ConvertToMM();
        }
    }
}
