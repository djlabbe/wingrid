using Hangfire.Server;

namespace Wingrid.Jobs
{
    public interface IBatchJob
    {
        static string? JobId { get; }
        Task ExecuteAsync(PerformContext? performContext);
    }
}