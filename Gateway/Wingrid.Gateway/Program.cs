using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Values;
using Wingrid.Gateway.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.AddAppAuthentication();
if(builder.Environment.ToString()?.ToLower().Equals("production") == true)
{
    builder.Configuration.AddJsonFile("ocelot.Production.json", optional: false, reloadOnChange: true);
}
else
{
    builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);
}

builder.Services.AddOcelot(builder.Configuration);


var app = builder.Build();
app.UseOcelot().GetAwaiter().GetResult();
app.Run();
