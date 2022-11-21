using Divergent.Sales.Data.Context;
using Microsoft.EntityFrameworkCore;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace Divergent.Sales.API;

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
        services.AddControllers().AddNewtonsoftJson();

        services.AddDbContext<SalesContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

        services.AddCors();

        services.AddOpenTelemetryTracing(config => config
            .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("Divergent.Sales.API"))
            .AddZipkinExporter(o =>
            {
                o.Endpoint = new Uri("http://localhost:9411/api/v2/spans");
            })
            .AddJaegerExporter(c =>
            {
                c.AgentHost = "localhost";
                c.AgentPort = 6831;
            })
            .AddAspNetCoreInstrumentation()
            .AddSource("NServiceBus.Core")
            .AddSqlClientInstrumentation(opt => opt.SetDbStatementForText = true)
        );
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseCors(policyBuilder =>
        {
            policyBuilder.AllowAnyOrigin();
            policyBuilder.AllowAnyMethod();
            policyBuilder.AllowAnyHeader();
        });

        app.UseRouting();

        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}