using System;
using System.Threading.Tasks;
using Octokit;

namespace IssueInProgressDaysLabeler.Model.Extensions
{
    public static class ApiHelpers
    {
        public static async Task<T> ExecuteWithRetry<T>(Func<Task<T>> action, int retryCount)
        {
            if (retryCount <= 0) throw new IndexOutOfRangeException(nameof(retryCount));

            while (true)
            {
                retryCount--;

                try
                {
                    var result = await action();

                    return result;
                }
                catch (ApiException)
                {
                    if (retryCount == 0) throw;
                }
            }
        }
    }
}