using Identity.Data;
using Identity.Interfaces;
using Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Identity.Services;

public class LocalAccountService: IAccountService
{
    private readonly ILogger<LocalAccountService> _logger;
    private readonly UserManager<ApplicationUser> _userManager;

    public LocalAccountService(
        ILogger<LocalAccountService> logger, UserManager<ApplicationUser> userManager)
    {
        _logger = logger;
        _userManager = userManager;
    }

    public async Task<ApplicationUser> GetByUserIdAsync(string userId, CancellationToken ct)
    {
        return await _userManager.FindByIdAsync(userId);
    }

    public async Task<ApplicationUser> GetUserByEmail(string email, CancellationToken ct)
    {
        return await _userManager.FindByEmailAsync(email);
    }

    public async Task<AuthenticationResult> AuthenticateAsync(string username, string password, CancellationToken ct)
    {
        try
        {
            var user = await _userManager.Users.SingleOrDefaultAsync(c => c.NormalizedUserName == username.ToUpper(), ct);

            if (user is null)
            {
                return AuthenticationResult.Failed("EmailNotRegistered");
            }

            var isCorrectCredentials = await _userManager.CheckPasswordAsync(user, password);

            if (isCorrectCredentials)
            {
                _logger.LogInformation("Valid User Credentials");
                return AuthenticationResult.Success(user.Id);
            }

            _logger.LogInformation("Invalid User Credentials");
            return AuthenticationResult.Failed("ADInvalidCredentials");
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex, "An unexpected error occurred");

            return AuthenticationResult.Failed("DefaultErrorMessage");
        }
    }
}
