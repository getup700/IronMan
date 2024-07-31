///************************************************************************************
///   Author:Tony Stark
///   CreateTime:2023/5/24 星期三 18:06:13
///   Mail:2609639898@qq.com
///   Github:https://github.com/getup700
///
///   Description:
///
///************************************************************************************

using Autodesk.Revit.DB;

namespace IronMan.Revit.Toolkit.Extension.Plugin
{
    internal class FailuresPerprocess : IFailuresPreprocessor
    {
        public FailureProcessingResult PreprocessFailures(FailuresAccessor failuresAccessor)
        {
            // Inside event handler, get all warnings
            var failList = failuresAccessor.GetFailureMessages();
            foreach (FailureMessageAccessor failure in failList)
            {
                var severity = failuresAccessor.GetSeverity();
                switch (severity)
                {
                    case FailureSeverity.Warning:

                        break;
                    case FailureSeverity.Error:
                        break;
                    case FailureSeverity.DocumentCorruption:
                        break;
                    default:
                        break;
                }
                //// check FailureDefinitionIds against ones that you want to dismiss, 
                //FailureDefinitionId failID = failure.GetFailureDefinitionId();
                //// prevent Revit from showing Unenclosed room warnings
                //if (failID == BuiltInFailures.RoomFailures.RoomNotEnclosed)
                //{
                //    failuresAccessor.DeleteWarning(failure);
                //}

            }

            return FailureProcessingResult.Continue;
        }
    }
}
