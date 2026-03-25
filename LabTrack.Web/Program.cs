using LabTrack.Core.Interfaces;
using LabTrack.Infrastructure.Repositories;
using LabTrack.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

// MySQL
var mySqlConnection = builder.Configuration.GetConnectionString("MySql");
builder.Services.AddDbContext<AppDbContext>(options =>
options.UseMySql(mySqlConnection, Microsoft.EntityFrameworkCore.ServerVersion.AutoDetect(mySqlConnection)));

// MongoDB
var mongoConnectionString = builder.Configuration.GetConnectionString("MongoDb");
var mongoDatabaseName = builder.Configuration["MongoDb:DatabaseName"];
builder.Services.AddSingleton<IMongoClient>(new MongoClient(mongoConnectionString));
builder.Services.AddScoped<IMongoDatabase>(sp =>
    sp.GetRequiredService<IMongoClient>().GetDatabase(mongoDatabaseName));

// MVC
builder.Services.AddScoped<ILabRunRepository, LabRunRepository>();
builder.Services.AddScoped<ISensorLogRepository, SensorLogRepository>();

builder.Services.AddControllersWithViews();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
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