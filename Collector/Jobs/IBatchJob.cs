using Hangfire.Server;

namespace Wingrid.Collector.Jobs
{
    public interface IBatchJob
    {
        static string? JobId { get; }
        Task ExecuteAsync(PerformContext? performContext);
    }
}