namespace ERP.Server.WebAPI.Modules;

public static class RouteRegistrar
{
    public static void RegisterRoutes(this IEndpointRouteBuilder app)
    {
        app.RegisterCustomerRoutes();
        app.RegisterAuthRoutes();
        app.RegisterDepotRoutes();
    }
}
