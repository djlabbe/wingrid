using Hangfire;

namespace Wingrid.Jobs
{
    public class JobSchedule
    {
        private static readonly Dictionary<string, Dictionary<string, string>> CronMaps = new()
        {
            {
                "Development", new Dictionary<string, string>
                {
                    { nameof(TeamsJob), "0 0 * * 2" }, // “At 00:00 on Tuesday.”
                    { nameof(EventsJob), "7 * * * *" }, // Every hour at :07
                    { nameof(WinnerDeterminationJob), "0 3 * * *" }, // Every day at 03:00
                }
            },
            {
                "Production", new Dictionary<string, string>
                {
                    { nameof(TeamsJob), "0 * * * *" },
                    { nameof(EventsJob), "7 * * * *" },
                    { nameof(WinnerDeterminationJob), "0 3 * * *" }, // Every day at 03:00
                }
            }
        };

        public static string GetCronExpression<T>(string envName)
        {

            if (!CronMaps.TryGetValue(envName, out var envSchedule))
                return Cron.Never();

            if (!envSchedule.TryGetValue(typeof(T).Name, out var expr))
                return Cron.Never();

            return expr;
        }
    }
}