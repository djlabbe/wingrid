using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Wingrid.Gateway.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        policy =>
        {
            policy.AllowAnyOrigin();
            policy.AllowAnyHeader();
            policy.AllowAnyMethod();
        });
});

builder.AddAppAuthentication();
if (builder.Environment.EnvironmentName.ToString()?.ToLower().Equals("production") == true)
{
    builder.Configuration.AddJsonFile("ocelot.Production.json", optional: false, reloadOnChange: true);
}
else
{
    builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);
}

builder.Services.AddOcelot(builder.Configuration);


var app = builder.Build();
app.UseCors();
app.UseOcelot().GetAwaiter().GetResult();
app.Run();
