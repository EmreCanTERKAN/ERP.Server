using ERP.Server.Domain.Recipes;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TS.Result;

namespace ERP.Server.Application.RecipeDetails;

public sealed record GetByIdRecipeWithDetailsQuery(
    Guid Id) : IRequest<Result<Recipe>>;

internal sealed class GetByIdRecipeWithDetailsQueryHandler(
    IRecipeRepository recipeRepository) : IRequestHandler<GetByIdRecipeWithDetailsQuery, Result<Recipe>>
{
    public async Task<Result<Recipe>> Handle(GetByIdRecipeWithDetailsQuery request, CancellationToken cancellationToken)
    {
        Recipe? recipe = await recipeRepository
            .Where(p => p.Id == request.Id)
            .Include(p => p.Product)
            .Include(p => p.Details!)
            .ThenInclude(p => p.Product)
            .FirstOrDefaultAsync(cancellationToken);

        if (recipe is null)
            return Result<Recipe>.Failure("Ürüne ait reçete bulunamadı");

        return recipe;
    }
}
