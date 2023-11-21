using Microsoft.EntityFrameworkCore;
using Persistence;

var builder = WebApplication.CreateBuilder(args);
var connectionString = GetConnectionString(builder, "DefaultConnection");

builder.Services.AddControllers();

builder.Services.AddDbContext<DataContext>(opt =>
{
    opt.UseNpgsql(connectionString);
});


var app = builder.Build();


app.UseHttpsRedirection();

app.UseAuthorization();

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

app.MapControllers();
app.Run();

string GetConnectionString(WebApplicationBuilder builder, string name)
{
    if (string.IsNullOrWhiteSpace(name))
        throw new ArgumentException(nameof(name));

    var connectionString = builder.Configuration.GetConnectionString(name);

    var password = builder.Configuration["DB_Password"] ?? throw new Exception("Missing environment variable: DB_Password");
    return $"{connectionString};Password={password}";
}