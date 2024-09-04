using Microsoft.EntityFrameworkCore;
using CrudApp.Data;
using CrudApp.Models.SeedData;
using CrudApp.Repositories;
using CrudApp.Contracts;
using CrudApp.Settings;
using CrudApp.Models.IdentityModels;
using Microsoft.AspNetCore.Identity;
var builder = WebApplication.CreateBuilder(args);


// Add services to the container.

builder.Services.AddDbContext<CrudAppContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("CrudAppContext") ?? throw new InvalidOperationException("Connection string 'CrudAppContext' not found.")));

builder.Services.AddDbContext<CrudAppContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityDB") ?? throw new InvalidOperationException("Connection string 'IdentityDB' not found.")));

builder.Services.Configure<MongoDBSettings>(builder.Configuration.GetSection("MongoDB"));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<IdentityContext>()
    .AddDefaultTokenProviders();

builder.Services.AddSingleton<MongoThingsContext>();
builder.Services.AddScoped<SeedData>();
builder.Services.AddScoped<IThingsRepository, SQLSThingsRepository>();
builder.Services.AddScoped<IMongoThingsRepository, MongoThingsRepository>();

builder.Services.AddControllersWithViews();

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

using (var scope = app.Services.CreateScope())
{
    var service = scope.ServiceProvider;
    seedDataThing.Initialize(service);

    var MongoSeed = scope.ServiceProvider.GetRequiredService<SeedData>();
    await MongoSeed.Initialize();
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

