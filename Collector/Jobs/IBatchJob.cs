using Hangfire;
using Hangfire.Server;

namespace Collector.Jobs
{
   
    public interface IBatchJob
    {
        static string? JobId { get; }
        Task ExecuteAsync(PerformContext? performContext);
    }
}