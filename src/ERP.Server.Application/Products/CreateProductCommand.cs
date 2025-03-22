using ERP.Server.Domain.Enums;
using ERP.Server.Domain.Products;
using FluentValidation;
using GenericRepository;
using Mapster;
using MediatR;
using TS.Result;

namespace ERP.Server.Application.Product;

public sealed record CreateProductCommand(
    string Name,
    int TypeValue) : IRequest<Result<string>>;


public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(p => p.Name)
            .NotEmpty().WithMessage("Ürün adı boş olamaz");

        RuleFor(p => p.TypeValue)
            .Must(value => ProductTypeEnum.TryFromValue(value, out _))
            .WithMessage("Geçersiz ürün türü seçildi.");
    }
}

internal sealed class CreateProductCommandHandler(
    IProductRepository productRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<CreateProductCommand, Result<string>>
{
    public async Task<Result<string>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        bool isNameExists = await productRepository.AnyAsync(p => p.Name == request.Name, cancellationToken);

        if (isNameExists)
        {
            return Result<string>.Failure("Ürün adı daha önce kaydedilmiştir");
        }

        Domain.Products.Product product = request.Adapt<Domain.Products.Product>();
        product.Type = ProductTypeEnum.FromValue(request.TypeValue);

        productRepository.Add(product);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return "Ürün başarıyla oluşturuldu.";
    }
}
