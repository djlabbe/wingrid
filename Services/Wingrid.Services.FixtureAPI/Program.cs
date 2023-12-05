using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Wingrid.Services.FixtureAPI.Data;
using Wingrid.Services.FixtureAPI;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using Wingrid.Services.FixtureAPI.Extensions;
using OpenTelemetry.Metrics;
using OpenTelemetry.Logs;
using Wingrid.Services.FixtureAPI.Services;
using Wingrid.Services.FixtureAPI.Messaging;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenTelemetry().WithMetrics(metrics =>
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

var optionBuilder = new DbContextOptionsBuilder<AppDbContext>();
optionBuilder.UseNpgsql(connectionString);
builder.Services.AddSingleton(new ScoreService(optionBuilder.Options));

builder.Services.AddSingleton<IAzureServiceBusConsumer, AzureServiceBusConsumer>();
IMapper mapper = MappingConfig.RegisterMaps().CreateMapper();
builder.Services.AddSingleton(mapper);
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddScoped<IFixturesService, FixturesService>();
builder.Services.AddScoped<IEventsService, EventsService>();
builder.Services.AddScoped<IEntryService, EntryService>();
builder.Services.AddHttpClient("Events", u => u.BaseAddress = new Uri(builder.Configuration["ServiceUrls:EventsAPI"] ?? ""));


builder.AddAppAuthentication();
builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.jon", "Fixture v1");
    options.RoutePrefix = string.Empty;
});


app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapPrometheusScrapingEndpoint();
ApplyMigrations();
app.UseAzureServiceBusConsumer();
app.Run();

static string GetConnectionString(WebApplicationBuilder builder, string name)
{
    if (string.IsNullOrWhiteSpace(name))
        throw new ArgumentException("Connection name is required.", nameof(name));

    var connectionString = builder.Configuration.GetConnectionString(name);

    var password = builder.Configuration["DB_Password"] ?? throw new Exception("Missing environment variable: DB_Password");
    return $"{connectionString};Password={password}";
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