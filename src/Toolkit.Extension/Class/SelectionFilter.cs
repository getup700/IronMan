using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;
using Org.BouncyCastle.Asn1.Cms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IronMan.Revit.Toolkit.Extension.Class
{
    public class SelectionFilter : ISelectionFilter
    {
        private Predicate<Element> _elementPredicate;
        private Predicate<Reference> _referencePredicate;
        private Type _type;

        public SelectionFilter(Type type, Predicate<Element> elementPredicate = null, Predicate<Reference> referencePredicate = null)
        {
            _type = type;
            _elementPredicate = elementPredicate;
            _referencePredicate = referencePredicate;
        }

        public SelectionFilter(Predicate<Element> elementPredicate = null, Predicate<Reference> referencePredicate = null)
        {
            _elementPredicate = elementPredicate;
            _referencePredicate = referencePredicate;
        }

        public bool AllowElement(Element elem)
        {
            if (_type != null)
            {
                return _type == elem.GetType() ? true : false;
            }
            return _elementPredicate != null ? true : _elementPredicate(elem);
        }

        public bool AllowReference(Reference reference, XYZ position)
        {
            return _referencePredicate != null ? true : _referencePredicate(reference);
        }
    }
}
