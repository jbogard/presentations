using System;
using ITOps.ViewModelComposition;
using ITOps.ViewModelComposition.Gateway;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Trace;

namespace Divergent.CompositionGateway
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRouting();
            services.AddViewModelComposition();
            services.AddCors();

            services.AddOpenTelemetryTracing(config => config
                .AddZipkinExporter(o =>
                {
                    o.Endpoint = new Uri("http://localhost:9411/api/v2/spans");
                    o.ServiceName = "Divergent.CompositionGateway";
                })
                .AddJaegerExporter(c =>
                {
                    c.AgentHost = "localhost";
                    c.AgentPort = 6831;
                    c.ServiceName = "Divergent.CompositionGateway";
                })
                .AddAspNetCoreInstrumentation()
                .AddSqlClientInstrumentation(opt => opt.SetTextCommandContent = true)
            );
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseCors(policyBuilder =>
            {
                policyBuilder.AllowAnyOrigin();
                policyBuilder.AllowAnyMethod();
                policyBuilder.AllowAnyHeader();
            });

            app.RunCompositionGatewayWithDefaultRoutes();
        }
    }
}
