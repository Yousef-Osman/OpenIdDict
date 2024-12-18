using Identity.Data;
using Identity.Models;

namespace Identity.Interfaces;

public interface IAccountService
{
    Task<ApplicationUser> GetByUserIdAsync(string userId, CancellationToken ct);

    Task<ApplicationUser> GetUserByEmail(string email, CancellationToken ct);

    Task<AuthenticationResult> AuthenticateAsync(string username, string password, CancellationToken ct);
}
