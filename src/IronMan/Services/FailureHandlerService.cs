using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IronMan.Revit.Services
{
    public  class FailureHandlerService : IFailuresPreprocessor
    {
        public FailureProcessingResult PreprocessFailures(FailuresAccessor failuresAccessor)
        {
            var failureMessageAccessor = failuresAccessor.GetFailureMessages();
            foreach (var failure in failureMessageAccessor)
            {
                var failureMessage = failure.GetDescriptionText();
                FailureSeverity failureSeverity = failure.GetSeverity();
                //error messages
                if (failureSeverity == FailureSeverity.Error)
                {
                    return FailureProcessingResult.ProceedWithRollBack;
                }
                //warning messages
                if (failureSeverity == FailureSeverity.Warning)
                {
                    return FailureProcessingResult.ProceedWithCommit;
                }
                else
                {
                    failuresAccessor.DeleteAllWarnings();
                    return FailureProcessingResult.Continue;
                }
            }
            return FailureProcessingResult.Continue;
        }
    }
}
