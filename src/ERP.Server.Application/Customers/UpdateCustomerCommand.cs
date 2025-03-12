using ERP.Server.Domain.Customers;
using FluentValidation;
using GenericRepository;
using Mapster;
using MediatR;
using TS.Result;

namespace ERP.Server.Application.Customers;

public sealed record UpdateCustomerCommand(
    Guid Id,
    string FirstName,
    string LastName,
    string TaxDepartment,
    string TaxNumber,
    AddressDto Address) : IRequest<Result<string>>;

public class UpdateCustomerCommandValidator : AbstractValidator<UpdateCustomerCommand>
{
    public UpdateCustomerCommandValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("Ad boş olamaz.")
            .MaximumLength(50).WithMessage("Ad 50 karakterden uzun olamaz.");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Soyad boş olamaz.")
            .MaximumLength(50).WithMessage("Soyad 50 karakterden uzun olamaz.");

        RuleFor(x => x.TaxDepartment)
            .NotEmpty().WithMessage("Vergi dairesi boş olamaz.")
            .MaximumLength(50).WithMessage("Vergi dairesi adı 100 karakterden uzun olamaz.");

        RuleFor(x => x.TaxNumber)
            .NotEmpty().WithMessage("Vergi numarası boş olamaz.")
            .Length(11).WithMessage("Vergi numarası 11 karakter olmalıdır."); // Min ve max yerine Length() kullanıldı.

        RuleFor(x => x.Address)
            .NotNull().WithMessage("Adres bilgisi zorunludur.")
            .SetValidator(new AddressDtoValidator()); // Adres için ayrı bir validator çağırıyoruz.
    }
}


internal sealed class UpdateCustomerCommandHandler(
    ICustomerRepository customerRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<UpdateCustomerCommand, Result<string>>
{
    public async Task<Result<string>> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
    {
        Customer customer = await customerRepository.GetByExpressionWithTrackingAsync(p => p.Id == request.Id, cancellationToken);

        if (customer is null)
            return Result<string>.Failure("Bu idye uygun kayıt bulunamadı");

        if(customer.TaxNumber != request.TaxNumber)
        {
            bool isTaxNumberExists = await customerRepository.AnyAsync(p => p.TaxNumber == request.TaxNumber, cancellationToken);

            if (isTaxNumberExists)
                return Result<string>.Failure("Bu vergi numarası ile daha önce kayıt oluşturulmuş");
        }

        request.Adapt(customer);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return "Müşteri başarıyla güncellendi";

    }
}
