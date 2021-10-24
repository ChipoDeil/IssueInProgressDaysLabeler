namespace IssueInProgressDaysLabeler.Model
{
    internal static class StatusCodeConstants
    {
        internal const int ErrorBadArgumentsStatusCode = 0xA0;
        internal const int SuccessStatusCode = 0;
    }

    internal static class GithubConstants
    {
        internal const string AppName = "IssueInProgressDaysLabeler";
        internal const string LabelTemplate = "В работе: {0} дней"; // TODO: customization
        internal const string LabelTemplatePrefix = "В работе:";
    }
}