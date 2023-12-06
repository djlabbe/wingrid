using Hangfire;

namespace Wingrid.Services.EventAPI.Jobs
{
    public class JobSchedule
    {
        private static readonly Dictionary<string, Dictionary<string, string>> CronMaps = new()
        {
            {
                "Development", new Dictionary<string, string>
                {
                    { nameof(TeamsJob), "0 0 * * *" }, // Every day at 12:00 AM
                    { nameof(EventsJob), "7 * * * *" }, // Every hour at :07
                }
            },
            {
                "Production", new Dictionary<string, string>
                {
                    { nameof(TeamsJob), "0 0 * * *" }, // Every day at 12:00 AM
                    { nameof(EventsJob), "7 * * * *" }, // Every hour at :07
                }
            }
        };

        public static string GetCronExpression<T>(string envName)
        {
            // if (envName == "Development")
            //     return Cron.Never();

            if (!CronMaps.TryGetValue(envName, out var envSchedule))
                return Cron.Never();

            if (!envSchedule.TryGetValue(typeof(T).Name, out var expr))
                return Cron.Never();

            return expr;
        }
    }
}