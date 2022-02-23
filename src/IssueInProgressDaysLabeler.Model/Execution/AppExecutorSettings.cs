using System;
using System.Collections.Generic;

namespace IssueInProgressDaysLabeler.Model.Execution
{
    public record AppExecutorSettings(IReadOnlyCollection<string> Labels, DateTimeOffset? Since);
}