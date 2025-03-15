using ERP.Server.Domain.RecipeDetails;
using ERP.Server.Domain.Recipes;
using GenericRepository;
using MediatR;
using TS.Result;

namespace ERP.Server.Application.Recipes;

public sealed record CreateRecipeCommand(
    Guid ProductId,
    List<RecipeDetailDto> Details) : IRequest<Result<string>>;

public sealed record RecipeDetailDto(
    Guid ProductId,
    decimal Quantity);

internal sealed class CreateRecipeCommandHandler(
    IRecipeRepository recipeRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<CreateRecipeCommand, Result<string>>
{
    public async Task<Result<string>> Handle(CreateRecipeCommand request, CancellationToken cancellationToken)
    {
        bool isRecipeExists = await recipeRepository.AnyAsync(p => p.ProductId == request.ProductId, cancellationToken);

        if (isRecipeExists)
            return Result<string>.Failure("Bu ürüne ait reçete daha önce oluşturulmuş");

        Recipe recipe = new()
        {
            ProductId = request.ProductId,
            RecipeDetails = request.Details.Select(s =>
            new RecipeDetail()
            {
                ProductId = s.ProductId,
                Quantity = s.Quantity
            }).ToList()
        };

        await recipeRepository.AddAsync(recipe , cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return "Reçete kaydı başarıyla oluşturuldu";
    }
}

   