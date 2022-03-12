using IssueInProgressDaysLabeler.Model.Dtos;

namespace IssueInProgressDaysLabeler.Model.IssueUpdatePreprocessors
{
    internal interface IIssueUpdatePreprocessor
    {
        public void Preprocess(IssueUpdateWithNumber issueUpdateWithNumber);
    }
}