using Application.Services;
using Collector.Jobs;
using Collector.Services;
using Hangfire;
using Hangfire.Console;
using Hangfire.PostgreSql;
using Microsoft.EntityFrameworkCore;
using Persistence;

var builder = WebApplication.CreateBuilder(args);
var connectionString = GetConnectionString(builder, "DefaultConnection");

// Add services to the container.
builder.Services.AddTransient<IEspnService, EspnService>();
builder.Services.AddTransient<IEventsService, EventsService>();
builder.Services.AddTransient<ITeamsService, TeamsService>();

builder.Services.AddControllers();
builder.Services.AddDbContext<DataContext>(opt =>
{
    opt.UseNpgsql(connectionString);
});

builder.Services
    .AddHangfire(config => {
        config.UseConsole(); 
        config.UsePostgreSqlStorage(c => c.UseNpgsqlConnection(connectionString));
    })
    .AddHangfireServer(options => { options.WorkerCount = 10; });

var app = builder.Build();

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseHangfireDashboard();

var services = app.Services.CreateScope().ServiceProvider;

try
{
    var context = services.GetRequiredService<DataContext>();
    context.Database.Migrate();
}
catch (Exception ex)
{
    var logger = services.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex, "An error occured during migration.");
}

AddRecurringJob<TeamsJob>(TeamsJob.JobId);
AddRecurringJob<EventsJob>(EventsJob.JobId);

// app.MapControllers();

app.Run();


void AddRecurringJob<T>(string jobId) where T : IBatchJob
{
    RecurringJob.AddOrUpdate<T>(jobId, j => j.ExecuteAsync(null), JobSchedule.GetCronExpression<T>(app.Environment.EnvironmentName));
}

string GetConnectionString(WebApplicationBuilder builder, string name)
{
    if (string.IsNullOrWhiteSpace(name))
        throw new ArgumentException("Name is required.", nameof(name));

    var connectionString = builder.Configuration.GetConnectionString(name);

    var password = builder.Configuration["DB_Password"] ?? throw new Exception("Missing environment variable: DB_Password");

    return $"{connectionString};Password={password}";
}