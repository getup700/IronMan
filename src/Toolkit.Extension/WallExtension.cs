using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IronMan.Revit.Toolkit.Extension
{
    public static class WallExtension
    {
        /// <summary>
        /// 获取与Wall方向相同的PlanarFace
        /// </summary>
        /// <param name="wall"></param>
        /// <returns></returns>
        public static List<PlanarFace> GetSameDirectionPlanarFaces(this Wall wall)
        {
            Options option = new Options();
            option.ComputeReferences = true;
            option.DetailLevel = ViewDetailLevel.Fine;
            List<PlanarFace> planarFaces = new List<PlanarFace>();

            GeometryElement geometryElement = wall.get_Geometry(option);
            List<GeometryObject> geoObjects = geometryElement.GetAllObjects();

            foreach (GeometryObject obj in geoObjects)
            {
                Solid solid = obj as Solid;
                if (solid != null)
                {
                    foreach (Face face in solid.Faces)
                    {
                        PlanarFace pf = face as PlanarFace;
                        if (pf != null)
                        {
                            if (pf.FaceNormal.CrossProduct(wall.Orientation).IsZeroLength())
                            {
                                planarFaces.Add(pf);
                            }
                        }
                    }

                }
            }
            return planarFaces;
        }

        /// <summary>
        /// 获取墙的两个侧面
        /// </summary>
        /// <param name="wall"></param>
        /// <returns></returns>
        public static List<Face> GetWallFaces(this Wall wall)
        {
            List<Face> normalFaces = new List<Face>();
            Options options = new Options();
            options.ComputeReferences = true;
            GeometryElement geoElement = wall.get_Geometry(options);
            foreach (GeometryObject geoObj in geoElement)
            {
                if (geoObj == null || (geoObj as Solid).Faces.Size <= 1) continue;
                foreach (Face face in (geoObj as Solid).Faces)
                {
                    XYZ normal = (face as PlanarFace).FaceNormal;
                    if (normal.AngleTo(wall.Orientation) < 0.01 || normal.AngleTo(-wall.Orientation) < 0.01)
                    {
                        normalFaces.Add(face);
                    }
                }
            }
            return normalFaces;
        }
    }
}
