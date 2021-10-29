using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CommandLine;

namespace IssueInProgressDaysLabeler.Model
{
    public class Program
    {
        public static Task<int> Main(string[] args)
        {
            using var parser = new Parser(with =>
            {
                with.EnableDashDash = true;
                with.HelpWriter = Console.Out;
            });

            var result = parser.ParseArguments<Options>(args)
                    .MapResult(OnParsed, OnError);

            return result;
        }

        private static async Task<int> OnParsed(Options options)
        {
            var gitHubClientFacade = GithubClientFactory.Create(
                options.GithubToken,
                options.Owner,
                options.Repository);

            await IncrementDaysStrategy.IncrementDays(
                gitHubClientFacade,
                options.Labels);

            return StatusCodeConstants.SuccessStatusCode;
        }

        private static Task<int> OnError(IEnumerable<Error> errors)
            => Task.FromResult(StatusCodeConstants.ErrorBadArgumentsStatusCode);
    }
}