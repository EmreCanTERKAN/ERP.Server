using Projects;

var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<ERP_Server_WebAPI>("erp-server-webapi");

builder.Build().Run();
