using ERP.Server.Domain.Products;
using GenericRepository;
using MediatR;
using TS.Result;

namespace ERP.Server.Application.Product;

public sealed record DeleteProductByIdCommand(
    Guid Id) : IRequest<Result<string>>;

internal sealed class DeleteProductByIdCommandHandler(
    IProductRepository productRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<DeleteProductByIdCommand, Result<string>>
{
    public async Task<Result<string>> Handle(DeleteProductByIdCommand request, CancellationToken cancellationToken)
    {
        var product = await productRepository.FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);
        if (product is null)
            return Result<string>.Failure("Ürün bulunamadı");

        productRepository.Delete(product);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return "Ürün başarıyla silindi";
    }
}
