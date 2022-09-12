using System;
using System.Threading.Tasks;
using Octokit;

namespace IssueInProgressDaysLabeler.Model.Extensions
{
    public static class ApiHelpers
    {
        public static async Task<T> ExecuteWithRetryAndDelay<T>(Func<Task<T>> action, int retryCount)
        {
            if (action == null) throw new ArgumentNullException(nameof(action));

            if (retryCount <= 0) throw new IndexOutOfRangeException(nameof(retryCount));

            while (true)
            {
                retryCount--;

                try
                {
                    var result = await action();

                    // NOTE: recommended way to send multiple requests to github
                    await Task.Delay(TimeSpan.FromSeconds(1));

                    return result;
                }
                catch (ApiException)
                {
                    if (retryCount == 0) throw;
                    
                    await Task.Delay(TimeSpan.FromSeconds(1));
                }
            }
        }
    }
}