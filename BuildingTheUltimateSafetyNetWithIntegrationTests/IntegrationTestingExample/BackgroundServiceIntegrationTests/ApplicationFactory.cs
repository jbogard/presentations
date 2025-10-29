using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Communication.HandlerIntegrationTests;

public class ApplicationFactory<TProgram> : WebApplicationFactory<TProgram>
    where TProgram : class
{
    private Action<IWebHostBuilder>? _internalConfiguration;
    private Action<IServiceCollection>? _services;
    private Action<ConfigurationBuilder>? _configurationBuilder;
    private Action<ILoggingBuilder>? _logging;
    private Action<IConfigurationBuilder>? _hostConfigurationBuilder;

    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.ConfigureHostConfiguration(_hostConfigurationBuilder ?? (_ => { }));

        return base.CreateHost(builder);
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        IConfiguration? configuration;

        _internalConfiguration = webHostBuilder =>
        {
            webHostBuilder.ConfigureLogging(_logging ?? (_ => { }));

            webHostBuilder.UseEnvironment("IntegrationTests");

            webHostBuilder.ConfigureAppConfiguration(
                (_, configurationBuilder) =>
                {
                    configurationBuilder.Sources.Clear(); //removes all providers (no appSettings.json, yaml etc)
                    var confBuilder = new ConfigurationBuilder();

                    _configurationBuilder?.Invoke(confBuilder);

                    configuration = confBuilder.Build();

                    configurationBuilder.AddConfiguration(configuration);
                }
            );

            webHostBuilder.ConfigureTestServices(services =>
            {
                _services?.Invoke(services);
            });
        };

        _internalConfiguration(builder.Configure(_ => { }));
    }

    public ApplicationFactory<TProgram> RegisterServices(Action<IServiceCollection> services)
    {
        _services = services;
        return this;
    }

    public ApplicationFactory<TProgram> UseAppConfiguration(Action<ConfigurationBuilder> config)
    {
        _configurationBuilder = config;
        return this;
    }

    public ApplicationFactory<TProgram> UseHostConfiguration(Action<IConfigurationBuilder> config)
    {
        _hostConfigurationBuilder = config;
        return this;
    }

    public ApplicationFactory<TProgram> ConfigureLogging(Action<ILoggingBuilder> logging)
    {
        _logging = logging;
        return this;
    }
}
