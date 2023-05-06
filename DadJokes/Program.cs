using DadJokes.Services;
using DadJokes.Clients;
using DadJokes.Interface;
using DadJokes.AppSettings;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddOptions();

// Load appsettings.json into config object
IConfiguration configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
.Build();



var settingsSection = configuration.GetSection("DadJokeAppSettings");
builder.Services.Configure<DadJokeAppSettings>(configuration.GetSection("DadJokeAppSettings"));

// Add config object to the service collection
builder.Services.AddSingleton(configuration);

// DI 
builder.Services.AddScoped<IDadJokeService, DadJokeService>();
builder.Services.AddScoped<IDadJokeApiClient, DadJokeApiClient>();

// configure HttpClient
builder.Services.AddHttpClient();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}



app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


app.Run();

