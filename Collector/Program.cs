using Hangfire;
using Hangfire.Console;
using Hangfire.PostgreSql;
using Microsoft.EntityFrameworkCore;
using Wingrid.Collector;
using Wingrid.Collector.Jobs;
using Wingrid.Collector.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = GetConnectionString(builder, "DefaultConnection");

builder.Services.AddControllers();
builder.Services.AddTransient<IEspnService, EspnService>();
builder.Services.AddTransient<IEventsService, EventsService>();
builder.Services.AddTransient<ITeamsService, TeamsService>();

builder.Services.AddDbContext<DataContext>(opt =>
{
    opt.UseNpgsql(connectionString);
});

builder.Services.AddHangfire(config => {
    config.UseConsole(); 
    config.UsePostgreSqlStorage(c => c.UseNpgsqlConnection(connectionString));
}).AddHangfireServer(options => { options.WorkerCount = 10; });


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        options.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();

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

app.MapControllers();

app.Run();

static string GetConnectionString(WebApplicationBuilder builder, string name)
{
    if (string.IsNullOrWhiteSpace(name))
        throw new ArgumentException("Connection name is required.", nameof(name));

    var connectionString = builder.Configuration.GetConnectionString(name);

    var password = builder.Configuration["DB_Password"] ?? throw new Exception("Missing environment variable: DB_Password");
    return $"{connectionString};Password={password}";
}

void AddRecurringJob<T>(string jobId) where T : IBatchJob
{
    RecurringJob.AddOrUpdate<T>(jobId, j => j.ExecuteAsync(null), JobSchedule.GetCronExpression<T>(builder.Environment.EnvironmentName));
}