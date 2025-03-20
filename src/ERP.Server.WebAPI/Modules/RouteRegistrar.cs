namespace ERP.Server.WebAPI.Modules;

public static class RouteRegistrar
{
    public static void RegisterRoutes(this IEndpointRouteBuilder app)
    {
        app.RegisterCustomerRoutes();
        app.RegisterAuthRoutes();
        app.RegisterDepotRoutes();
        app.RegisterProductRoutes();
        app.RegisterRecipeRoutes();
        app.RegisterRecipeDetailRoutes();
        app.RegisterOrderRoutes();
        app.RegisterInvoiceRoutes();
    }
}
