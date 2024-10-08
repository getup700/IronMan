﻿using Autodesk.Revit.DB;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace IronMan.Revit.Entity
{
    public class ElementProxy : ObservableObject
    {
        private Element _element;

        public ElementProxy(Element element)
        {
            _element = element;
            _name = element.Name;
        }
        public ElementProxy()
        {

        }

        public Document Document => this._element?.Document;

        //public ElementId Id => (((this._element != null) && (this._element.Id)) ?? ElementId.InvalidElementId);
        public ElementId Id => (this._element != null) ? (this._element.Id) : ElementId.InvalidElementId;

        //public string Name
        //{
        //    get => this._element?.Name;
        //    set
        //    {
        //        this.SetProperty(() => this._element.Name = value, "Name");
        //    }
        //}
        private string _name;

        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }


    }
}
