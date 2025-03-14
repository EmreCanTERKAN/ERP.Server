using ERP.Server.Application.Customers;
using ERP.Server.Domain.Depots;
using FluentValidation;
using GenericRepository;
using Mapster;
using MediatR;
using TS.Result;

namespace ERP.Server.Application.Depots;

public sealed record CreateDepotCommand(
    string Name,
    AddressDto Address) : IRequest<Result<string>>;

public class CreateDepotCommandValidator : AbstractValidator<CreateDepotCommand>
{

    public CreateDepotCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Depo ismi boş olamaz");

        RuleFor(x => x.Address)
           .NotNull().WithMessage("Adres bilgisi zorunludur.")
           .SetValidator(new AddressDtoValidator());
    }
}


internal sealed class CreateDepotCommandHandler(
    IDepotRepository depotRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<CreateDepotCommand, Result<string>>
{
    public async Task<Result<string>> Handle(CreateDepotCommand request, CancellationToken cancellationToken)
    {
        bool isNameExists = await depotRepository.AnyAsync(p => p.Name == request.Name, cancellationToken);

        if (isNameExists)
            return Result<string>.Failure("Bu depo daha önce oluşturulmuş");

        Depot depot = request.Adapt<Depot>();

        depotRepository.Add(depot);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return "Depo başarıyla kaydedildi";
    }
}
