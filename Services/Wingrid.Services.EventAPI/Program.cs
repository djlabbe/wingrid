using AutoMapper;
using Hangfire;
using Hangfire.Console;
using Microsoft.EntityFrameworkCore;
using Wingrid.Services.EventAPI.Data;
using Wingrid.Services.EventAPI.Jobs;
using Wingrid.Services.EventAPI.Services;
using Wingrid.Services.EventAPI;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using Wingrid.Services.EventAPI.Extensions;
using OpenTelemetry.Metrics;
using OpenTelemetry.Logs;
using Wingrid.MessageBus;
using Hangfire.PostgreSql;
using System.Security.Claims;
using Microsoft.Extensions.Primitives;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenTelemetry()
.WithMetrics(metrics =>
{
    metrics.AddMeter("Microsoft.AspNetCore.Hosting");
    metrics.AddMeter("Microsoft.AspNetCore.Server.Kestrel");
    metrics.AddMeter("System.Net.Http");
    metrics.AddPrometheusExporter();
    metrics.AddOtlpExporter();
});

builder.Logging.AddOpenTelemetry(options =>
{
    options.AddOtlpExporter();
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition(name: "Bearer", securityScheme: new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = "Enter the Bearer Authorization as follows: `Bearer Generated-JWT-Token`",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id=JwtBearerDefaults.AuthenticationScheme
                }
            }, Array.Empty<string>()
        }
    });
});

var connectionString = GetConnectionString(builder, "DefaultConnection");

builder.Services.AddControllers();

builder.Services.AddDbContext<AppDbContext>(opt =>
{
    opt.UseNpgsql(connectionString);
});

IMapper mapper = MappingConfig.RegisterMaps().CreateMapper();
builder.Services.AddSingleton(mapper);
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddScoped<IEspnService, EspnService>();
builder.Services.AddScoped<IEventsService, EventsService>();
builder.Services.AddScoped<ITeamsService, TeamsService>();
builder.Services.AddScoped<IFixturesService, FixturesService>();
builder.Services.AddScoped<IMessageBus, MessageBus>();

builder.Services.AddHangfire(config =>
{
    config.UseConsole();
    config.UsePostgreSqlStorage(c => c.UseNpgsqlConnection(connectionString));
}).AddHangfireServer(options => { options.WorkerCount = 10; });

builder.AddAppAuthentication();
builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.jon", "Event v1");
    options.RoutePrefix = string.Empty;
});


app.UseHangfireDashboard(options: new DashboardOptions
{
    Authorization = new[] { new JobDashboardAuthorizationFilter(ClaimTypes.Role, new StringValues("ADMIN_JOBS")) },
    AppPath = "/"
});

AddRecurringJob<TeamsJob>(TeamsJob.JobId);
AddRecurringJob<EventsJob>(EventsJob.JobId);
AddRecurringJob<WinnerDeterminationJob>(WinnerDeterminationJob.JobId);

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapPrometheusScrapingEndpoint();
ApplyMigrations();
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

void ApplyMigrations()
{
    using var scope = app.Services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    if (context.Database.GetPendingMigrations().Any())
    {
        context.Database.Migrate();
    }
}