using Hangfire.Server;

namespace Wingrid.Services.EventAPI.Jobs
{
    public interface IBatchJob
    {
        static string? JobId { get; }
        Task ExecuteAsync(PerformContext? performContext);
    }
}