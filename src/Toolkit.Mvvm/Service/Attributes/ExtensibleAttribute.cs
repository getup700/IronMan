using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IronMan.Revit.Toolkit.Mvvm.Service.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class ExtensibleAttribute:Attribute
    {
        public ExtensibleAttribute(Type profile)
        {
            Profile = profile;
        }

        public Type Profile { get;}
    }
}
