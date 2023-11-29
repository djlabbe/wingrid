using Auth.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Wingrid.Auth.Data;
using Wingrid.Services.Auth.Extensions;
using Wingrid.Services.Auth.Services;

var builder = WebApplication.CreateBuilder(args);
var connectionString = GetConnectionString(builder, "DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(opt =>
{
    opt.UseNpgsql(connectionString);
});
builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("ApiSettings:JwtOptions"));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();
builder.Services.AddControllers();
builder.Services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
builder.Services.AddScoped<IAuthService, AuthService>();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.AddAppAuthentication();
builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.jon", "Auth v1");
    options.RoutePrefix = string.Empty;
});


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

void ApplyMigrations()
{
    using var scope = app.Services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    if (context.Database.GetPendingMigrations().Any())
    {
        context.Database.Migrate();
    }
}
