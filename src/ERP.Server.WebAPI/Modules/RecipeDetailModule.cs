using ERP.Server.Application.RecipeDetails;
using MediatR;
using TS.Result;

public static class RecipeDetailModule
{
    public static void RegisterRecipeDetailRoutes(this IEndpointRouteBuilder app)
    {
        RouteGroupBuilder group = app.MapGroup("/recipeDetail").WithTags("RecipeDetail");

        group.MapPost("create",
            async (ISender sender, CreateRecipeDetailCommand request, CancellationToken cancellationToken) =>
            {
                var response = await sender.Send(request, cancellationToken);
                return response.IsSuccessful ? Results.Ok(response) : Results.InternalServerError(response);
            })
            .Produces<Result<string>>()
            .WithName("CreateRecipeDetail");

        group.MapPost("deleteById",
            async (ISender sender, DeleteRecipeDetailByIdCommand request, CancellationToken cancellationToken) =>
            {
                var response = await sender.Send(request, cancellationToken);
                return response.IsSuccessful ? Results.Ok(response) : Results.InternalServerError(response);
            })
            .Produces<Result<string>>()
            .WithName("DeleteRecipeDetail");

        group.MapPost("update",
            async (ISender sender, UpdateRecipeDetailCommand request, CancellationToken cancellationToken) =>
            {
                var response = await sender.Send(request, cancellationToken);
                return response.IsSuccessful ? Results.Ok(response) : Results.InternalServerError(response);
            })
            .Produces<Result<string>>()
            .WithName("UpdateRecipeDetail");

        group.MapPost("getByIdRecipeWithDetails",
            async (ISender sender, GetByIdRecipeWithDetailsQuery request, CancellationToken cancellationToken)  =>
            {
                var response = await sender.Send(request, cancellationToken);
                return response.IsSuccessful ? Results.Ok(response) : Results.InternalServerError(response);
            })
            .Produces<Result<string>>()
            .WithName("GetByIdRecipeWithDetails");

    }
}
