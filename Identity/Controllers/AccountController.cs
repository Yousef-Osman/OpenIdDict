using Identity.Data;
using Identity.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Identity.Models;

namespace Identity.Controllers;

public class AccountController : Controller
{
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly ILogger<AccountController> _logger;
    private readonly IAccountService _accountService;
    private readonly UserManager<ApplicationUser> _userManager;

    public AccountController(
        SignInManager<ApplicationUser> signInManager,
        ILogger<AccountController> logger,
        IAccountService accountService,
        UserManager<ApplicationUser> userManager)
    {
        _signInManager = signInManager;
        _logger = logger;
        _accountService = accountService;
        _userManager = userManager;
    }

    /// <summary>
    /// Entry point into the login workflow.
    /// </summary>
    [HttpGet]
    public IActionResult Login(string returnUrl)
    {
        if (User.Identity?.IsAuthenticated == true)
        {
            return RedirectPermanent(returnUrl ?? "~/");
        }

        var vm = new LoginViewModel
        {
            ReturnUrl = returnUrl,
            ExternalProviders = GetExternalProviders()
        };

        return View(vm);
    }

    /// <summary>
    /// Handle post back from username/password login.
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginInputModel model, CancellationToken ct)
    {
        if (!ModelState.IsValid)
        {
            var vm = new LoginViewModel
            {
                Username = model.Username,
                ReturnUrl = model.ReturnUrl,
                ExternalProviders = GetExternalProviders()
            };
            return View(vm);
        }

        var result = await _accountService.AuthenticateAsync(model.Username, model.Password, ct);

        if (!result.Succeeded)
        {
            _logger.LogWarning("Invalid login attempt for user {Username}.", model.Username);
            ModelState.AddModelError(string.Empty, result.ErrorMessage);
            return View(new LoginViewModel { Username = model.Username, ReturnUrl = model.ReturnUrl });
        }

        var appUser = await _userManager.FindByNameAsync(model.Username);

        if (appUser == null)
        {
            ModelState.AddModelError(string.Empty, "UserNotRegistered");

            return View(new LoginViewModel
            {
                Username = model.Username,
                ReturnUrl = model.ReturnUrl,
                ExternalProviders = GetExternalProviders(),
                ClientUrl = model.ClientUrl
            });
        }

        var claims = new List<Claim>
            {
                new(ClaimTypes.Name, model.Username),
                new(ClaimTypes.Email, appUser.Email!)
            };

        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

        await HttpContext.SignInAsync(new ClaimsPrincipal(claimsIdentity));

        if (Url.IsLocalUrl(model.ReturnUrl))
        {
            return Redirect(model.ReturnUrl);
        }

        return Redirect("~/");
    }

    /// <summary>
    /// Show logout page.
    /// </summary>
    [HttpGet]
    public IActionResult Logout(string logoutId)
    {
        return View(new LogoutViewModel { LogoutId = logoutId });
    }

    /// <summary>
    /// Handle logout page post back.
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout(LogoutInputModel model)
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        _logger.LogInformation("User logged out.");
        var url = Url.Action("Login");
        return RedirectPermanent(url ?? "~/");

    }

    [HttpGet]
    public IActionResult AccessDenied()
    {
        return View();
    }

    /// <summary>
    /// Retrieve external login providers.
    /// </summary>
    private IEnumerable<ExternalProvider> GetExternalProviders()
    {
        var schemes = _signInManager.GetExternalAuthenticationSchemesAsync().Result;

        return schemes.Select(scheme => new ExternalProvider
        {
            DisplayName = scheme.DisplayName ?? scheme.Name,
            AuthenticationScheme = scheme.Name
        });
    }
}