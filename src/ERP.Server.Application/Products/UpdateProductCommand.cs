using ERP.Server.Domain.Depots;
using ERP.Server.Domain.Enums;
using ERP.Server.Domain.Products;
using FluentValidation;
using GenericRepository;
using Mapster;
using MediatR;
using TS.Result;

namespace ERP.Server.Application.Product;

public sealed record UpdateProductCommand(
    Guid Id,
    string Name,
    int TypeValue) : IRequest<Result<string>>;


public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
{
    public UpdateProductCommandValidator()
    {
        RuleFor(p => p.Name)
            .NotEmpty().WithMessage("Ürün adı boş olamaz");

        RuleFor(p => p.TypeValue)
            .Must(value => ProductTypeEnum.TryFromValue(value, out _))
            .WithMessage("Geçersiz ürün türü seçildi.");
    }
}

internal sealed class UpdateProductCommandHandler(
    IProductRepository productRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<UpdateProductCommand, Result<string>>
{
    public async Task<Result<string>> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var product = await productRepository.FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);
        if (product is null)
            return Result<string>.Failure("Ürün bulunamadı");
        if(product.Name != request.Name)
        {
            bool isNameExists = await productRepository.AnyAsync(p => p.Name == request.Name, cancellationToken);
            if (isNameExists)
                return Result<string>.Failure("Bu product daha önce kayıt oluşturulmuş");
        }
        request.Adapt(product);
        product.Type = ProductTypeEnum.FromValue(request.TypeValue);

        productRepository.Update(product);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return "Ürün başarıyla güncellendi.";


    }
}
