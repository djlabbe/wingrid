using Hangfire;

namespace Wingrid.Services.Collector.Jobs
{
    public class JobSchedule
    {
        private static readonly Dictionary<string, Dictionary<string, string>> CronMaps = new()
        {
            {
                "Development", new Dictionary<string, string>
                {
                    { nameof(TeamsJob), "0 0 * * 0" }, // At every 30th minute
                    { nameof(EventsJob), "7 * * * *" }, // At minute 45
                }
            },
            {
                "Production", new Dictionary<string, string>
                {
                    { nameof(TeamsJob), "0 0 * * 0" }, // At 00:00 on Sunday
                    { nameof(EventsJob), "7 * * * *" }, // At minute 7
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