var builder = DistributedApplication.CreateBuilder(args);

var postgres = builder.AddPostgres("postgres")
    .WithImageTag("latest")
    .WithLifetime(ContainerLifetime.Persistent)
    .WithPgAdmin();

var appdb = postgres.AddDatabase("appdb");

var externalApi = builder.AddProject<Projects.ExternalApi>("externalapi");

builder.AddProject<Projects.WebApp>("webapp")
    .WithReference(appdb)
    .WithReference(externalApi);

builder.AddProject<Projects.BackgroundService>("backgroundservice")
    .WithReference(appdb);

builder.Build().Run();
