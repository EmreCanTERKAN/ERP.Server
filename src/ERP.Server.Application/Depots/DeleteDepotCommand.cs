using ERP.Server.Application.Customers;
using ERP.Server.Domain.Customers;
using ERP.Server.Domain.Depots;
using GenericRepository;
using MediatR;
using TS.Result;

namespace ERP.Server.Application.Depots;

public sealed record DeleteDepotCommand(
    Guid Id) : IRequest<Result<string>>;

internal sealed class DeleteCustomerCommandHandler(
    IDepotRepository depotRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<DeleteDepotCommand, Result<string>>
{
    public async Task<Result<string>> Handle(DeleteDepotCommand request, CancellationToken cancellationToken)
    {
        var depot = await depotRepository.FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

        if (depot is null)
            return Result<string>.Failure("Bu idye uygun kayıt bulunamadı");

        depotRepository.Delete(depot);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return "Depo başarıyla silindi.";

    }
}