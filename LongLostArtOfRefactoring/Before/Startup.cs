using System;
using System.Threading;
using Before.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Before;

public class Startup
{
    // This method gets called by the runtime. Use this method to add services to the container.
    // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddTransient<OfferService>();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseRouting();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapGet("/", async context =>
            {
                await context.Response.WriteAsync("Hello World!");
            });
        });

        var offerService = app.ApplicationServices.GetRequiredService<OfferService>();
        offerService.AssignOffer(Guid.NewGuid(), Guid.NewGuid(), CancellationToken.None);
        offerService.ExpireOffer(Guid.NewGuid(), Guid.NewGuid(), CancellationToken.None);
        offerService.ReassignOffers(Guid.NewGuid(), Guid.NewGuid(), CancellationToken.None);
    }
}