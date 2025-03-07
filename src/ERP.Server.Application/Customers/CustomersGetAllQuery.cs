using ERP.Server.Domain.Abstractions;
using ERP.Server.Domain.Customers;
using ERP.Server.Domain.Users;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace ERP.Server.Application.Customers;

public sealed record CustomersGetAllQuery() : IRequest<IQueryable<CustomersGetAllQueryResponse>>;

public sealed class CustomersGetAllQueryResponse : EntityDto
{
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string FullName => string.Join(" ", FirstName, LastName);
    public string TaxDepartment { get; set; } = default!;
    public string TaxNumber { get; set; } = default!;
    public Address Address { get; set; } = default!;
}

internal sealed class CustomersGetAllQueryHandler(
    ICustomerRepository customerRepository,
    UserManager<AppUser> userManager) : IRequestHandler<CustomersGetAllQuery, IQueryable<CustomersGetAllQueryResponse>>
{
    public Task<IQueryable<CustomersGetAllQueryResponse>> Handle(CustomersGetAllQuery request, CancellationToken cancellationToken)
    {
        var response = (from customers in customerRepository.GetAll()
                        join create_user in userManager.Users.AsQueryable() on customers.CreateUserId equals create_user.Id
                        join update_user in userManager.Users.AsQueryable() on customers.UpdateUserId equals update_user.Id into update_user
                        from update_users in update_user.DefaultIfEmpty()
                        select new CustomersGetAllQueryResponse
                        {
                            FirstName = customers.FirstName,
                            LastName = customers.LastName,
                            TaxDepartment = customers.TaxDepartment,
                            TaxNumber = customers.TaxNumber,
                            Address = customers.Address,
                            CreateAt = customers.CreateAt,
                            DeleteAt = customers.DeleteAt,
                             
                            UpdateAt = customers.UpdateAt,
                            Id = customers.Id,
                            IsActive = customers.IsActive,
                            IsDeleted = customers.IsDeleted,
                            CreateUserId = customers.CreateUserId,
                            CreateUserName = create_user.FirstName + " " + create_user.LastName + " (" + create_user.Email + ")",
                            UpdateUserId = customers.UpdateUserId,
                            UpdateUserName = customers.UpdateUserId == null ? null : update_users.FirstName + " " + update_users.LastName + " (" + update_users.Email + ")",
                        });
        return Task.FromResult(response);
    }
}

