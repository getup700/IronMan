using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace IronMan.Revit.Toolkit.Extension
{
    public static class GeometryElementExtension
    {
        /// <summary>
        /// get revit internal method where is from geometry element
        /// </summary>
        /// <returns></returns>
        private static MethodInfo GetTransientDisplayMethod()
        {
            return typeof(GeometryElement)
                .GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)
                .FirstOrDefault(x => x.Name == "SetForTransientDisplay");
        }

        public static List<GeometryObject> GetAllObjects(this GeometryElement geometryElement)
        {
            if (geometryElement == null) return null;
            List<GeometryObject> results = new List<GeometryObject>();
            //遍历GeometryELement中的GeometryObject
            IEnumerator<GeometryObject> enumerator = geometryElement.GetEnumerator();
            while (enumerator.MoveNext())
            {
                GeometryObject geoObject = enumerator.Current;
                //如果是嵌套的GeometryElement
                if (geoObject is GeometryElement subGeometryElement)
                {
                    GetAllObjects(subGeometryElement);
                }
                //如果是嵌套的GeometryInstance
                else if (geoObject is GeometryInstance geometryInstance)
                {
                    GetAllObjects(geometryInstance.GetInstanceGeometry());
                }
                //如果直接是Solid
                else if (geoObject is Solid solid)
                {
                    if (solid.Faces.Size > 0 || solid.Edges.Size > 0)
                    {
                        results.Add(geoObject);
                    }
                }
            }
            return results;
        }

        /// <summary>
        /// Creates geometry of transient (temporary) element for application display which will not be saved with the model.
        /// </summary>
        /// <param name="document"></param>
        /// <param name="objects"></param>
        /// <returns>The element id of the created element</returns>
        /// <exception cref="Exception"></exception>
        public static ElementId TransientDisplay(this Document document, IEnumerable<GeometryObject> objects, ElementId graphicsStyleId = null)
        {
            if (document == null)
            {
                throw new ArgumentNullException(nameof(document));
            }

            MethodInfo method = GetTransientDisplayMethod();
            if (method == null)
            {
                throw new Exception($"No target method");
            }

            return (ElementId)method.Invoke(null, parameters: new object[4]
            {
               document,
               ElementId.InvalidElementId,
               objects,
               graphicsStyleId ?? ElementId.InvalidElementId
            });
        }

        /// <summary>
        /// Creates geometry of transient (temporary) element for application display which will not be saved with the model.
        /// </summary>
        /// <param name="document"></param>
        /// <param name="obj"></param>
        /// <param name="graphicsStyleId"></param>
        /// <returns>The element id of the created element</returns>
        public static ElementId TransientDisplay(this Document document, GeometryObject obj, ElementId graphicsStyleId = null)
        {
            return document.TransientDisplay(new List<GeometryObject>() { obj }, graphicsStyleId);
        }

    }
}
