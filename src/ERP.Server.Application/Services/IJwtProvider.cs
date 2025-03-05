using ERP.Server.Domain.Users;

namespace ERP.Server.Application.Services;
public interface IJwtProvider
{
    public Task<string> CreateTokenAsync(AppUser user, string password, CancellationToken cancellationToken = default);
}
