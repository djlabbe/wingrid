using AutoMapper;
using Hangfire;
using Hangfire.Console;
using Microsoft.EntityFrameworkCore;
using Wingrid.Data;
using Wingrid.Jobs;
using Wingrid.Services;
using Wingrid;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using Wingrid.Extensions;
using OpenTelemetry.Metrics;
using OpenTelemetry.Logs;
using Hangfire.PostgreSql;
using Microsoft.Extensions.Primitives;
using Wingrid.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Web.Services;


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



builder.Services.AddDbContext<AppDbContext>(opt =>
{
    opt.UseNpgsql(connectionString);
});

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddControllers();

IMapper mapper = MappingConfig.RegisterMaps().CreateMapper();
builder.Services.AddSingleton(mapper);
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddScoped<IEspnService, EspnService>();
builder.Services.AddScoped<IEventsService, EventsService>();
builder.Services.AddScoped<ITeamsService, TeamsService>();
builder.Services.AddScoped<IStatisticsService, StatisticsService>();
builder.Services.AddScoped<IFixturesService, FixturesService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddTransient<IEmailSender, EmailSender>();

builder.Services.Configure<AuthMessageSenderOptions>(builder.Configuration);
builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("ApiSettings:JwtOptions"));



builder.Services.AddHangfire(config =>
{
    config.UseConsole();
    config.UsePostgreSqlStorage(c => c.UseNpgsqlConnection(connectionString));
}).AddHangfireServer(options => { options.WorkerCount = 10; });

builder.AddAppAuthentication();
builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.jon", "Event v1");
        options.RoutePrefix = string.Empty;
    });
}

app.UseHangfireDashboard(options: new DashboardOptions
{
    Authorization = new[] { new JobDashboardAuthorizationFilter("role", new StringValues("ADMIN_JOBS")) },
    AppPath = "/"
});

AddRecurringJob<TeamsJob>(TeamsJob.JobId);
AddRecurringJob<EventsJob>(EventsJob.JobId);
AddRecurringJob<WinnerDeterminationJob>(WinnerDeterminationJob.JobId);

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapPrometheusScrapingEndpoint();
ApplyMigrations();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html");
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
    var options = new RecurringJobOptions
    {
        TimeZone = GetJobTimeZone()
    };

    RecurringJob.AddOrUpdate<T>(jobId, j => j.ExecuteAsync(null), JobSchedule.GetCronExpression<T>(builder.Environment.EnvironmentName), options);
}

TimeZoneInfo GetJobTimeZone()
{
    TimeZoneInfo? tz = null;
    try
    {
        tz = TimeZoneInfo.GetSystemTimeZones().Where(z => z.BaseUtcOffset.TotalHours == -7.0d && (z.Id.Contains("Phoenix", StringComparison.OrdinalIgnoreCase) ||
            z.Id.Contains("Mountain Standard Time", StringComparison.OrdinalIgnoreCase))).FirstOrDefault();
    }
    catch { }
    return tz ?? TimeZoneInfo.Utc;
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

