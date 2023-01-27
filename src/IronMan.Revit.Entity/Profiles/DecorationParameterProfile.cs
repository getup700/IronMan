using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IronMan.Revit.Entity.Profiles
{
    public class DecorationParameterProfile : DefinitionProfile
    {
        public DecorationParameterProfile()
        {
            Parameters.Add(RoomCode);
            Parameters.Add(RoomName);
            Parameters.Add(Floor);
            Parameters.Add(GroundMethodCode);
            Parameters.Add(GroundThickness);
            Parameters.Add(InteriorWallMethodCode);
            Parameters.Add(CeilingMethodCode);
            Parameters.Add(CeilingHeight);
            Parameters.Add(BaseBoardMethodCode);
            Parameters.Add(BaseBoardHeight);
        }

        public DefinitionInfo RoomCode => CreateParameter("房间编号", BuiltInParameterGroup.PG_TEXT, ParameterType.Text, UnitType.UT_Number);
        public DefinitionInfo RoomName => CreateParameter("房间名称", BuiltInParameterGroup.PG_TEXT, ParameterType.Text, UnitType.UT_Number);
        public DefinitionInfo Floor => CreateParameter("楼层", BuiltInParameterGroup.PG_TEXT, ParameterType.Text, UnitType.UT_Number);
        public DefinitionInfo GroundMethodCode => CreateParameter("楼地面_做法编号", BuiltInParameterGroup.PG_TEXT, ParameterType.Text, UnitType.UT_Number);
        public DefinitionInfo GroundThickness => CreateParameter("楼地面_厚度", BuiltInParameterGroup.PG_TEXT, ParameterType.Integer, UnitType.UT_Number);
        public DefinitionInfo InteriorWallMethodCode => CreateParameter("内墙面_做法编号", BuiltInParameterGroup.PG_TEXT, ParameterType.Text, UnitType.UT_Number);
        public DefinitionInfo CeilingMethodCode => CreateParameter("顶棚_做法编号", BuiltInParameterGroup.PG_TEXT, ParameterType.Text, UnitType.UT_Number);
        public DefinitionInfo CeilingHeight => CreateParameter("顶棚_做法高度", BuiltInParameterGroup.PG_TEXT, ParameterType.Integer, UnitType.UT_Number);
        public DefinitionInfo BaseBoardMethodCode => CreateParameter("踢脚_做法编号", BuiltInParameterGroup.PG_TEXT, ParameterType.Text, UnitType.UT_Number);
        public DefinitionInfo BaseBoardHeight => CreateParameter("踢脚_做法高度", BuiltInParameterGroup.PG_TEXT, ParameterType.Integer, UnitType.UT_Number);

    }
}
