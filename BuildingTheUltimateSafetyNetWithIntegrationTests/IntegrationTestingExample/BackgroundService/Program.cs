var builder = Host.CreateApplicationBuilder(args);

builder.AddServiceDefaults();

builder.UseNServiceBusWithConfiguration("backgroundservice", "appdb");

var host = builder.Build();
host.Run();

public partial class Program { }