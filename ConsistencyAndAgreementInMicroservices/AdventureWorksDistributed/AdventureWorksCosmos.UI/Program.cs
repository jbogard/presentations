using AdventureWorksDistributed.Core.Infrastructure;
using AdventureWorksDistributed.Orders.Contracts;
using AdventureWorksDistributed.Products.Models;
using AdventureWorksDistributed.UI.Infrastructure;
using Microsoft.Azure.Documents.Client;
using Newtonsoft.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var services = builder.Services;
services.AddRazorPages();
services.AddControllersWithViews();

services.AddDistributedMemoryCache();

services.AddSession(options =>
{
    // Set a short timeout for easy testing.
    options.IdleTimeout = TimeSpan.FromSeconds(300);
    options.Cookie.HttpOnly = true;
});

services.AddDbContext<AdventureWorks2016Context>();

services.AddHttpClient<IOrdersService, OrdersService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7151");
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseSession();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();