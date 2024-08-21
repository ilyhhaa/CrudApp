using Microsoft.EntityFrameworkCore;
using CrudApp.Data;
using CrudApp.Models.SeedData;
using CrudApp.Repositories;
using CrudApp.Contracts;
using CrudApp.Settings;
var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddDbContext<CrudAppContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("CrudAppContext") ?? throw new InvalidOperationException("Connection string 'CrudAppContext' not found.")));
builder.Services.Configure<MongoDBSettings>(builder.Configuration.GetSection("MongoDB"));
builder.Services.AddSingleton<MongoThingsContext>();
builder.Services.AddScoped<IThingsRepository, SQLSThingsRepository>();
builder.Services.AddScoped<IMongoThingsRepository, MongoThingsRepository>();

builder.Services.AddControllersWithViews();

var app = builder.Build();



using (var scope = app.Services.CreateScope())
{
    var service = scope.ServiceProvider;
    seedDataThing.Initialize(service);
}
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
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
    pattern: "{controller=Things}/{action=Index}/{id?}");

app.Run();

