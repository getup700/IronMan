﻿using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using IronMan.Revit.Toolkit.Extension;

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

        public FloorProxy()
        {

        }


        private Floor _floor;
        private Level _level;
        private double _offset;

        public double Offset
        {
            get { return _offset; }
            set { SetProperty(ref _offset, value); }
        }

        public Level Level
        {
            get { return _level; }
            set { SetProperty(ref _level, value); }
        }

        public Floor Floor
        {
            get => _floor;
            set => SetProperty(ref _floor, value);
        }

        private Level GetLevel()
        {
            return base.Document.GetElement(Floor.LevelId) as Level;
        }

        private double GetOffset()
        {
            return Floor.get_Parameter(BuiltInParameter.FLOOR_HEIGHTABOVELEVEL_PARAM).AsDouble().ConvertToMilliMeters();
        }
    }
}
