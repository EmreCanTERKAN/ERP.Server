using ERP.Server.WebAPI.Installers;
using ERP.Server.WebAPI.Modules;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddInternalServices(builder.Configuration);

builder.Services.AddExternalServices();

var app = builder.Build();

app.MapOpenApi();

app.MapScalarApiReference();

app.MapDefaultEndpoints();

app.AddMiddlewares();

app.MapControllers().RequireRateLimiting("fixed").RequireAuthorization();

app.RegisterRoutes();


app.Run();