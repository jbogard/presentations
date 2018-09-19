using System;
using AdventureWorksCosmos.Products.Models;
using AdventureWorksCosmos.UI.Infrastructure;
using AdventureWorksCosmos.UI.Models.Inventory;
using AdventureWorksCosmos.UI.Models.Orders;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AdventureWorksCosmos.UI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            services.AddDistributedMemoryCache();

            services.AddMediatR(typeof(Startup));

            services.AddScoped(typeof(IDocumentDBRepository<>), typeof(DocumentDBRepository<>));
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IDocumentMessageDispatcher, DocumentMessageDispatcher>();
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(UnitOfWorkBehavior<,>));
            services.Scan(c =>
            {
                c.FromAssembliesOf(typeof(Startup))
                    .AddClasses(t => t.AssignableTo(typeof(IDocumentMessageHandler<>)))
                    .AsImplementedInterfaces()
                    .WithTransientLifetime();
            });

            services.AddSession(options =>
            {
                // Set a short timeout for easy testing.
                options.IdleTimeout = TimeSpan.FromSeconds(300);
                options.Cookie.HttpOnly = true;
            });
            services.AddDbContext<AdventureWorks2016Context>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseSession();

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action=Index}/{id?}");
            });
        }
    }
}
