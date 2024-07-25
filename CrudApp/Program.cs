﻿using Microsoft.EntityFrameworkCore;
using CrudApp.Data;
using CrudApp.Models.SeedData;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<CrudAppContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("CrudAppContext") ?? throw new InvalidOperationException("Connection string 'CrudAppContext' not found.")));

// Add services to the container.
builder.Services.AddControllersWithViews();

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

