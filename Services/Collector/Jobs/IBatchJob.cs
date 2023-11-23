using Hangfire.Server;

namespace Wingrid.Services.Collector.Jobs
{
    public interface IBatchJob
    {
        static string? JobId { get; }
        Task ExecuteAsync(PerformContext? performContext);
    }
}