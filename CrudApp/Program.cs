using Microsoft.EntityFrameworkCore;
using CrudApp.Data;
using CrudApp.Models.SeedData;
using CrudApp.Repositories;
using CrudApp.Contracts;
using CrudApp.Settings;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<CrudAppContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("CrudAppContext") ?? throw new InvalidOperationException("Connection string 'CrudAppContext' not found.")));
builder.Services.Configure<MongoDBSettings>(builder.Configuration.GetSection("MongoDB"));
builder.Services.AddSingleton<MongoThingsContext>();

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IThingsRepository,SQLSThingsRepository>();
builder.Services.AddScoped<IMongoThingsRepository, MongoThingsRepository>();

var app = builder.Build();



using (var scope = app.Services.CreateScope())
{
    var service = scope.ServiceProvider;
    seedDataThing.Initialize(service);
}
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
    pattern: "{controller=Things}/{action=Index}/{id?}");

app.Run();

