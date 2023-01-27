using Autodesk.Revit.DB.ExtensibleStorage;
using RvtEntity = Autodesk.Revit.DB.ExtensibleStorage.Entity;
using Autodesk.Revit.DB;
using IronMan.Revit.Entity.Profiles;
using IronMan.Revit.Toolkit.Mvvm.Service.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IronMan.Revit.Entity
{
    [Extensible(typeof(ParameterFilterSetInfo))]
    //[Extensible(typeof(SheetTitleSchemaInfo))]
    public class DocumentProxy : ElementProxy
    {
        public DocumentProxy(Document document)
        {
            _document = document;
            _element = new FilteredElementCollector(document).OfClass(typeof(DataStorage)).FirstElement();
            _labels = GetLabel();
        }
        
        private int _count;
        private IList<string> _labels;
        public  Document _document;
        private Element _element;

        [ExtensibleMember]
        public IList<string> Labels
        {
            get { return _labels; }
            set { _labels = value; }
        }

        [ExtensibleMember]
        public string SheetTitle { get; set; }

        public int Count
        {
            get { return _count; }
            set { Set(ref _count, value); }
        }

        public IList<string> GetLabel()
        {
            Schema schema = Schema.Lookup(Contants.ExtensibleStorage.ParameterFilterSetInfoGuid);
            if (schema != null)
            {
                var entity = _element.GetEntity(schema);
                if (entity.Schema == null) return null;
                return entity?.Get<IList<string>>("Labels");
            }
            return null;
        }

        public RvtEntity GetEntity(Schema schema)
        {
            return _element?.GetEntity(schema);
        }

        public void SetEntity(RvtEntity entity)
        {
            _element?.SetEntity(entity);
        }

    }
}
