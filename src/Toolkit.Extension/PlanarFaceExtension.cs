using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IronMan.Revit.Toolkit.Extension
{
    public static class PlanarFaceExtension
    {
        /// <summary>
        /// 获取PlanarFace的外部轮廓与内部轮廓
        /// </summary>
        /// <param name="planarFace"></param>
        /// <param name="profile">外部轮廓</param>
        /// <param name="openingArrays">内部轮廓</param>
        /// <exception cref="ArgumentException"></exception>
        public static List<Curve> GetOutline(this PlanarFace planarFace, out List<CurveArray> openingArrays)
        {
            XYZ normal = planarFace?.FaceNormal;
            if (planarFace == null) throw new ArgumentException("仅支持平面墙体");
            List<Curve>profile = new List<Curve>();
            List<CurveArray> internalArrays = new List<CurveArray>();
            IList<CurveLoop> curveLoops = planarFace.GetEdgesAsCurveLoops();
            int i = 0;
            //orderByDescending bie to small
            foreach (CurveLoop curveLoop in curveLoops.OrderByDescending(x => x.GetExactLength()))
            {
                CurveArray array = new CurveArray();
                foreach (Curve curve in curveLoop)
                {
                    if (i == 0)
                        profile.Append(curve);
                    else
                        array.Append(curve);
                }
                if (i != 0)
                {
                    internalArrays.Add(array);
                }
                i++;
            }
            openingArrays = internalArrays;
            return profile;
        }
    }
}
