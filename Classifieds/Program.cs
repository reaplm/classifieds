using Classifieds.Repository;
using Classifieds.Repository.Impl;
using Classifieds.Service;
using Classifieds.Service.Impl;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//Add service for server side blazor
builder.Services.AddServerSideBlazor();

//MySQL Connection
var connectionString = builder.Configuration.GetSection("mysqlconnection")["connectionString"];
builder.Services.AddDbContext<ApplicationContext>(options => options.UseMySQL(connectionString));

//Dependency Injection
//builder.Services.AddScoped<IAdvertService, AdvertService>();
builder.Services.AddScoped<IAdvertRepo, AdvertRepo>();
builder.Services.AddTransient<IAdvertService, AdvertService>();

var baseUrl = builder.Configuration.GetValue<String>("BaseUrl");
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(baseUrl) });

//Json Options

        
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

//Mapping for signal R hub
app.MapBlazorHub();

app.Run();
