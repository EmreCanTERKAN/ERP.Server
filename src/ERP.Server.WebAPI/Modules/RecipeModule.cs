using ERP.Server.Application.RecipeDetails;
using ERP.Server.Application.Recipes;
using ERP.Server.Domain.Recipes;
using MediatR;
using TS.Result;

public static class RecipeModule
{
    public static void RegisterRecipeRoutes(this IEndpointRouteBuilder app)
    {
        RouteGroupBuilder group = app.MapGroup("/recipe").WithTags("Recipe");

        group.MapPost("getAll",
            async (ISender sender, CancellationToken cancellationToken) =>
            {
                var response = await sender.Send(new GetAllRecipeQuery(), cancellationToken);
                return response.IsSuccessful ? Results.Ok(response) : Results.InternalServerError(response);
            })
            .Produces<Result<List<Recipe>>>()
            .WithName("GetAllRecipe");

        group.MapPost("create",
            async (ISender sender, CreateRecipeCommand request, CancellationToken cancellationToken) =>
            {
                var response = await sender.Send(request, cancellationToken);
                return response.IsSuccessful ? Results.Ok(response) : Results.InternalServerError(response);
            })
            .Produces<Result<string>>()
            .WithName("CreateRecipe");

        group.MapPost("deleteById",
            async (ISender sender, DeleteRecipeByIdCommand request, CancellationToken cancellationToken) =>
            {
                var response = await sender.Send(request, cancellationToken);
                return response.IsSuccessful ? Results.Ok(response) : Results.InternalServerError(response);
            })
            .Produces<Result<string>>()
            .WithName("DeleteRecipe");


    }
}
