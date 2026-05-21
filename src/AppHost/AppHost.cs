var builder = DistributedApplication.CreateBuilder(args);

var postgres = builder.AddPostgres("postgres")
    .WithPgAdmin();

var simulationDatabase = postgres.AddDatabase("simulationdb");

var rabbitMq = builder.AddRabbitMQ("rabbitmq")
    .WithManagementPlugin();

var api = builder.AddProject<Projects.RangeExtendedEvDigitalTwin_Api>("api")
    .WithReference(simulationDatabase)
    .WithReference(rabbitMq)
    .WithHttpHealthCheck("/health")
    .WithExternalHttpEndpoints();

builder.AddViteApp("frontend", "../frontend")
    .WithReference(api)
    .WaitFor(api);

builder.Build().Run();
