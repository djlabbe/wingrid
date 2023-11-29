using Microsoft.AspNetCore.Authentication.Cookies;
using Wingrid.Web.Services;
using Wingrid.Web.Utility;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient();
builder.Services.AddHttpClient<IFixturesService, FixturesService>();
builder.Services.AddHttpClient<IEventsService, EventsService>();
builder.Services.AddHttpClient<IAuthService, AuthService>();

StaticDetails.FixturesAPIBase = builder.Configuration["ServiceUrls:FixturesAPI"] ?? "";
StaticDetails.EventAPIBase = builder.Configuration["ServiceUrls:EventAPI"] ?? "";
StaticDetails.AuthAPIBase = builder.Configuration["ServiceUrls:AuthAPI"] ?? "";

builder.Services.AddScoped<ITokenProvider, TokenProvider>();
builder.Services.AddScoped<IBaseService, BaseService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IFixturesService, FixturesService>();
builder.Services.AddScoped<IEventsService, EventsService>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options => {
    options.ExpireTimeSpan = TimeSpan.FromHours(10);
    options.LoginPath = "/Auth/Login";
    options.AccessDeniedPath = "/Auth/AccessDenied";
});


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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html");

app.Run();
