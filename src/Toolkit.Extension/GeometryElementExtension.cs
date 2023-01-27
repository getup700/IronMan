using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IronMan.Revit.Toolkit.Extension
{
    public static class GeometryElementExtension
    {
        public static List<GeometryObject> GetAllObjects(this GeometryElement geometryElement)
        {
            if (geometryElement == null) return null;
            List<GeometryObject> results = new List<GeometryObject>();
            //遍历GeometryELement中的GeometryObject
            IEnumerator<GeometryObject> enumerator = geometryElement.GetEnumerator();
            while (enumerator.MoveNext())
            {
                GeometryObject geoObject = enumerator.Current;
                Type type = geoObject.GetType();
                //如果是嵌套的GeometryElement
                if (type.Equals(typeof(GeometryElement)))
                {
                    GetAllObjects(geoObject as GeometryElement);
                }
                //如果是嵌套的GeometryInstance
                else if (type.Equals(typeof(PlanarFace)))
                {
                    GetAllObjects((geoObject as GeometryInstance).GetInstanceGeometry());
                }
                //如果直接是Solid
                else if (type.Equals(typeof(Solid)))
                {
                    Solid solid = (Solid)geoObject;
                    if (solid.Faces.Size > 0 || solid.Edges.Size > 0)
                    {
                        results.Add(geoObject);
                    }
                }
            }
            return results;
        }
    }
}
