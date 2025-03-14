using ERP.Server.Application.Customers;
using ERP.Server.Domain.Customers;
using ERP.Server.Domain.Depots;
using FluentValidation;
using GenericRepository;
using Mapster;
using MediatR;
using TS.Result;

namespace ERP.Server.Application.Depots;

public sealed record UpdateDepotCommand(
    Guid Id,
    string Name,
    AddressDto Address) : IRequest<Result<string>>;

public class UpdateCustomerCommandValidator : AbstractValidator<UpdateDepotCommand>
{
    public UpdateCustomerCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Ad boş olamaz.")
            .MaximumLength(50).WithMessage("Ad 50 karakterden uzun olamaz.");
        RuleFor(x => x.Address)
            .NotNull().WithMessage("Adres bilgisi zorunludur.")
            .SetValidator(new AddressDtoValidator()); // Adres için ayrı bir validator çağırıyoruz.
    }
}

internal sealed class UpdateDepotCommandHandler(
    IDepotRepository depotRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<UpdateDepotCommand, Result<string>>
{
    public async Task<Result<string>> Handle(UpdateDepotCommand request, CancellationToken cancellationToken)
    {
        Depot depot = await depotRepository.GetByExpressionWithTrackingAsync(p => p.Id == request.Id, cancellationToken);

        if (depot is null)
            return Result<string>.Failure("Bu idye uygun kayıt bulunamadı");

        if (depot.Name != request.Name)
        {
            bool isNameExists = await depotRepository.AnyAsync(p => p.Name == request.Name, cancellationToken);

            if (isNameExists)
                return Result<string>.Failure("Bu depo ile daha önce kayıt oluşturulmuş.");
        }

        request.Adapt(depot);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return "Depo başarıyla güncellendi";

    }
}
