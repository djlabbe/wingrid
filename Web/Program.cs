using Microsoft.EntityFrameworkCore;
using Persistence;
using Web.Services;

var builder = WebApplication.CreateBuilder(args);
var connectionString = GetConnectionString(builder, "DefaultConnection");


// Add services to the container.
builder.Services.AddDbContext<DataContext>(opt =>
{
    opt.UseNpgsql(connectionString);
});


builder.Services.AddMemoryCache().AddTransient<IEventsService, EventsService>();

builder.Services.AddControllers().AddNewtonsoftJson();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html");

app.Run();

string GetConnectionString(WebApplicationBuilder builder, string name)
{
    if (string.IsNullOrWhiteSpace(name))
        throw new ArgumentException(nameof(name));

    var connectionString = builder.Configuration.GetConnectionString(name);

    var password = builder.Configuration["DB_Password"] ?? throw new Exception("Missing environment variable: DB_Password");
    return $"{connectionString};Password={password}";
}