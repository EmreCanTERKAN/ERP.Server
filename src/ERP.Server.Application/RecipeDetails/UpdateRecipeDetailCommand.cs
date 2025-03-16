using ERP.Server.Domain.RecipeDetails;
using GenericRepository;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TS.Result;

namespace ERP.Server.Application.RecipeDetails;

public sealed record UpdateRecipeDetailCommand(
    Guid Id,
    Guid ProductId,
    decimal Quantity) : IRequest<Result<string>>;

internal sealed class UpdateRecipeDetailCommandHandler(
    IRecipeDetailRepository recipeDetailRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<UpdateRecipeDetailCommand, Result<string>>
{
    public async Task<Result<string>> Handle(UpdateRecipeDetailCommand request, CancellationToken cancellationToken)
    {
        RecipeDetail recipeDetail = await recipeDetailRepository.GetByExpressionWithTrackingAsync(p => p.Id == request.Id);
        if (recipeDetail is null)
        {
            return Result<string>.Failure("Bu reçeteye ait ürün bulunamadı");
        }

        RecipeDetail? oldRecipeDetail = await recipeDetailRepository
            .Where(p => 
                p.Id != request.Id &&
                p.ProductId == request.ProductId && 
                p.RecipeId == recipeDetail.RecipeId)
            .FirstOrDefaultAsync(cancellationToken);

        if (oldRecipeDetail is not null)
        {
            recipeDetailRepository.Delete(recipeDetail);
            oldRecipeDetail.Quantity += request.Quantity;
            recipeDetailRepository.Update(oldRecipeDetail);
        }
        else
        {
           request.Adapt(recipeDetail);
        }
        
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return "Reçetedeki ürün başarıyla güncellendi";
    }
}
