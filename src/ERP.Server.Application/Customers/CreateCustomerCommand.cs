using ERP.Server.Domain.Customers;
using FluentValidation;
using GenericRepository;
using Mapster;
using MediatR;
using TS.Result;

namespace ERP.Server.Application.Customers;

public sealed record CreateCustomerCommand(
    string FirstName,
    string LastName,
    string TaxDepartment,
    string TaxNumber,
    AddressDto Address) : IRequest<Result<string>>;

public sealed record AddressDto(
    string City,
    string Town,
    string FullAddress);

public class CreateCustomerCommandValidator : AbstractValidator<CreateCustomerCommand>
{
    public CreateCustomerCommandValidator()
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

public class AddressDtoValidator : AbstractValidator<AddressDto>
{
    public AddressDtoValidator()
    {
        RuleFor(x => x.City)
            .NotEmpty().WithMessage("Şehir boş olamaz.")
            .MaximumLength(100).WithMessage("Şehir adı 100 karakterden uzun olamaz.");

        RuleFor(x => x.Town)
            .NotEmpty().WithMessage("İlçe boş olamaz.")
            .MaximumLength(100).WithMessage("İlçe adı 100 karakterden uzun olamaz.");

        RuleFor(x => x.FullAddress)
            .NotEmpty().WithMessage("Tam adres boş olamaz.")
            .MaximumLength(250).WithMessage("Adres 250 karakterden uzun olamaz.");
    }
}



internal sealed class CreateCustomerCommandHandler(
    ICustomerRepository customerRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<CreateCustomerCommand, Result<string>>
{
    public async Task<Result<string>> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
    {
        bool isTaxNumberExists = await customerRepository.AnyAsync(p => p.TaxNumber == request.TaxNumber, cancellationToken);

        if (isTaxNumberExists)
            return Result<string>.Failure("Bu vergi numarası ile daha önce kayıt oluşturulmuş");

        Customer customer = request.Adapt<Customer>();

        customerRepository.Add(customer);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return "Customer başarıyla kaydedildi";
    }
}
