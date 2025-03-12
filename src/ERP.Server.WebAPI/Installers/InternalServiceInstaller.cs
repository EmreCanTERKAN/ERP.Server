using ERP.Server.Application;
using ERP.Server.Infrastructure;

namespace ERP.Server.WebAPI.Installers;

public static class InternalServiceInstaller
{
    public static IServiceCollection AddInternalServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddApplication();
        services.AddInfrastructure(configuration);
        services.AddCors();
        services.AddExceptionHandler<ExceptionHandler>().AddProblemDetails();
        return services;
    }
}