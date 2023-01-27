using Autodesk.Revit.DB;
using Autodesk.Revit.DB.ExtensibleStorage;
using RvtEntity = Autodesk.Revit.DB.ExtensibleStorage.Entity;
using IronMan.Revit.Entity.Contants;
using IronMan.Revit.Entity.Profiles;
using IronMan.Revit.Toolkit.Mvvm.Service.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IronMan.Revit.Toolkit.Extension;

namespace IronMan.Revit.Entity
{
    [Extensible(typeof(ParameterFilterElementInfo))]
    public class ParameterFilterProxy : ElementProxy
    {
        public ParameterFilterProxy(ParameterFilterElement filter):base(filter)
        {
            _filter = filter;
            Name = _filter.Name;
            _labels = GetLabel();
        }

        private ParameterFilterElement _filter;
        private int _count;
        private IList<string> _labels;
        private bool _isApplied;
        private bool _isVisible;
        private View _activeView;
        private Color _color;

        public Color Color
        {
            get { return _color; }
            set { Set(ref _color, value); }
        }

       

        public bool IsVisible
        {
            get => _isVisible;
            set
            {
                Set(ref _isVisible, value);
            }
        }

        public bool IsApplied
        {
            get { return _isApplied; }
            set
            {
                Set(ref _isApplied, value);
            }
        }

        [ExtensibleMember]
        public IList<string> Labels
        {
            get { return _labels; }
            set { Set(ref _labels, value); }
        }

        public int Count
        {
            get { return _count; }
            set { Set(ref _count, value); }
        }

        public IList<string> GetLabel()
        {
            Schema schema = Schema.Lookup(Contants.ExtensibleStorage.ParameterFilterElementInfoGuid);
            if (schema != null)
            {
                var entity = _filter.GetEntity(schema);
                if (entity.Schema == null) return null;
                return entity?.Get<IList<string>>("Labels");
            }
            return null;
        }

        public RvtEntity GetEntity(Schema schema)
        {
            return _filter?.GetEntity(schema);
        }

        public void SetEntity(RvtEntity entity)
        {
            _filter?.SetEntity(entity);
        }

        public void SetVisible(bool visible)
        {
            _activeView.SetFilterVisibility(Id, visible);
        }

        public void SetApply(bool apply)
        {
            if (_activeView.Document == null) return;
            if(apply)
            {
                _activeView.AddFilter(Id);
            }
            else
            {
                _activeView.RemoveFilter(Id);
            }
        }
        public void SetActiveView(View activeView)
        {
            if (activeView != null && _filter.Document != null)
            {
                _activeView = activeView;
                IsApplied = activeView.IsFilterApplied(_filter.Id);
                if (IsApplied == true)
                {
                    IsVisible = activeView.GetFilterVisibility(_filter.Id);
                }
            }
        }
    }
}
