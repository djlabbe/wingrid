using AutoMapper;
using Hangfire;
using Hangfire.Console;
using Hangfire.PostgreSql;
using Microsoft.EntityFrameworkCore;
using Wingrid.Services.Collector.Data;
using Wingrid.Services.Collector.Jobs;
using Wingrid.Services.Collector.Services;
using Wingrid.Services.Collector;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Wingrid.Services.Collector.Extensions;

var builder = WebApplication.CreateBuilder(args);

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
builder.Services.AddTransient<IEspnService, EspnService>();
builder.Services.AddTransient<IEventsService, EventsService>();
builder.Services.AddTransient<ITeamsService, TeamsService>();

builder.Services.AddDbContext<AppDbContext>(opt =>
{
    opt.UseNpgsql(connectionString);
});

IMapper mapper = MappingConfig.RegisterMaps().CreateMapper();
builder.Services.AddSingleton(mapper);
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddHangfire(config => {
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
        options.SwaggerEndpoint("/swagger/v1/swagger.jon", "v1");
        options.RoutePrefix = string.Empty;
    });
}

app.UseHangfireDashboard();
AddRecurringJob<TeamsJob>(TeamsJob.JobId);
AddRecurringJob<EventsJob>(EventsJob.JobId);

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
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
