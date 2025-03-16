using ERP.Server.Domain.RecipeDetails;
using GenericRepository;
using MediatR;
using TS.Result;

namespace ERP.Server.Application.RecipeDetails;

public sealed record DeleteRecipeDetailByIdCommand(
    Guid Id) : IRequest<Result<string>>;


internal sealed class DeleteRecipeDetailByIdCommandHandler(
    IRecipeDetailRepository recipeDetailRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<DeleteRecipeDetailByIdCommand, Result<string>>
{
    public async Task<Result<string>> Handle(DeleteRecipeDetailByIdCommand request, CancellationToken cancellationToken)
    {
        RecipeDetail recipeDetail = await recipeDetailRepository.GetByExpressionAsync(p => p.Id == request.Id);
        if (recipeDetail is null)
            return Result<string>.Failure("Bu reçeteye ait ürün bulunamadı");

        recipeDetailRepository.Delete(recipeDetail);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return "Bu reçeteye ait ürün başarıyla silinmiştir";
    }
}
