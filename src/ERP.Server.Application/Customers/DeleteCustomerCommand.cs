using ERP.Server.Domain.Customers;
using GenericRepository;
using MediatR;
using TS.Result;

namespace ERP.Server.Application.Customers;

public sealed record DeleteCustomerCommand(
    Guid Id) : IRequest<Result<string>>;

internal sealed class DeleteCustomerCommandHandler(
    ICustomerRepository customerRepository,
    IUnitOfWork unitOfWork ) : IRequestHandler<DeleteCustomerCommand, Result<string>>
{
    public async Task<Result<string>> Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
    {
        var customer = await customerRepository.FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

        if (customer is null)
            return Result<string>.Failure("Bu idye uygun kayıt bulunamadı");

        customerRepository.Delete(customer);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return "Müşteri başarıyla silindi.";

    }
}
