using ERP.Server.Domain.RecipeDetails;
using GenericRepository;
using Mapster;
using MediatR;
using TS.Result;

namespace ERP.Server.Application.RecipeDetails;

public sealed record CreateRecipeDetailCommand(
    Guid RecipeId,
    Guid ProductId,
    decimal Quantity) : IRequest<Result<string>>;

internal sealed class CreateRecipeDetailCommandHandler(
    IRecipeDetailRepository recipeDetailRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<CreateRecipeDetailCommand, Result<string>>
{
    public async Task<Result<string>> Handle(CreateRecipeDetailCommand request, CancellationToken cancellationToken)
    {
        RecipeDetail? recipeDetail = await recipeDetailRepository
            .GetByExpressionWithTrackingAsync(p =>
                p.RecipeId == request.RecipeId &&
                p.ProductId == request.ProductId);

        if (recipeDetail is not null)
        {
            recipeDetail.Quantity += request.Quantity;
            await unitOfWork.SaveChangesAsync(cancellationToken);
            return "Ürün miktarı güncellendi.";
        }

        var newRecipeDetail = request.Adapt<RecipeDetail>();
        await recipeDetailRepository.AddAsync(newRecipeDetail, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return "Reçeteye ürün kaydı başarıyla tamamlandı.";
    }


}
