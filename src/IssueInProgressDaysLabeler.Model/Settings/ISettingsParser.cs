namespace IssueInProgressDaysLabeler.Model.Settings
{
    internal interface ISettingsParser
    {
        public IssueInProgressConsoleSettings? TryParse(string[] programArgs);
    }
}