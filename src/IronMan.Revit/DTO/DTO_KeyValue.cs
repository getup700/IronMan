using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Mechanical;
using GalaSoft.MvvmLight;
using Org.BouncyCastle.Asn1.BC;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace IronMan.Revit.DTO
{
    public class DTO_KeyValue : ObservableObject
    {
        //public Dictionary<string, string> Dic;
        //public DataTable KeyValue;

        private string _name;
        private object _value;
        private bool _isSelected;

        public bool IsSelected
        {
            get { return _isSelected; }
            set { Set(ref _isSelected, value); }
        }

        public string StringValue =>(string) _value;

        public object Value
        {
            get { return _value; }
            set { Set(ref _value, value); }
        }

        public string Name
        {
            get { return _name; }
            set { Set(ref _name, value); }
        }

        public ElementId NameId { get; set; }

        public Reference Reference { get; set; }
    }
}
