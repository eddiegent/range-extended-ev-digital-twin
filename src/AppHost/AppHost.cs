var builder = DistributedApplication.CreateBuilder(args);

var postgres = builder.AddPostgres("postgres")
    .WithPgAdmin();

var simulationDatabase = postgres.AddDatabase("simulationdb");

var rabbitMq = builder.AddRabbitMQ("rabbitmq")
    .WithManagementPlugin();

var api = builder.AddProject<Projects.RangeExtendedEvDigitalTwin_Api>("api")
    .WithReference(simulationDatabase)
    .WithReference(rabbitMq)
    .WithEnvironment("Supabase__Url", "http://localhost:54321")
    .WithEnvironment("Supabase__AnonKey", "replace-me")
    .WithHttpHealthCheck("/health")
    .WithExternalHttpEndpoints();

var frontend = builder.AddViteApp("frontend", "../frontend")
    .WithReference(api)
    .WaitFor(api);

api.PublishWithContainerFiles(frontend, "../frontend/dist");

builder.Build().Run();
