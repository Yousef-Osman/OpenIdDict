using System.Security.Claims;
using Identity.Data;
using Identity.Interfaces;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;

namespace Identity.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IAccountService _accountService;

        public TokenController(UserManager<ApplicationUser> userManager, IAccountService accountService)
        {
            _userManager = userManager;
            _accountService = accountService;
        }

        [HttpPost("~/connect/token")]
        public async Task<IActionResult> Exchange()
        {
            var request = HttpContext.GetOpenIddictServerRequest() ??
                          throw new InvalidOperationException("The OpenID Connect request cannot be retrieved.");

            ClaimsPrincipal claimsPrincipal;

            if (request.IsClientCredentialsGrantType())
            {
                // Note: the client credentials are automatically validated by OpenIddict:
                // if client_id or client_secret are invalid, this action won't be invoked.

                var identity = new ClaimsIdentity(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);

                // Subject (sub) is a required field, we use the client id as the subject identifier here.
                identity.AddClaim(OpenIddictConstants.Claims.Subject, request.ClientId ?? throw new InvalidOperationException());

                claimsPrincipal = new ClaimsPrincipal(identity);

                claimsPrincipal.SetScopes(request.GetScopes());
            }

            else if (request.IsAuthorizationCodeGrantType())
            {
                // Retrieve the claims principal stored in the authorization code
                claimsPrincipal = (await HttpContext.AuthenticateAsync(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme)).Principal;
            }
            else if (request.IsPasswordGrantType())
            {
                var result = await _accountService.AuthenticateAsync(request.Username, request.Password, CancellationToken.None);

                if (!result.Succeeded)
                {
                    return Unauthorized("ADInvalidCredentials");
                }

                var appUser = await _userManager.FindByNameAsync(request.Username!);
                if (appUser == null)
                {
                    return NotFound("UserNotRegistered");
                }

                var roles = await _userManager.GetRolesAsync(appUser);

                var claims = CreateTokenClaims(request, appUser, roles);

                // Create a new claims principal
                var claimsIdentity = new ClaimsIdentity(claims, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);

                claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                // Set requested scopes (this is not done automatically)
                claimsPrincipal.SetScopes(request.GetScopes());
            }
            else
            {
                throw new InvalidOperationException("The specified grant type is not supported.");
            }

            return SignIn(claimsPrincipal!, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
        }

        [HttpGet("~/connect/authorize")]
        [HttpPost("~/connect/authorize")]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> Authorize()
        {
            var request = HttpContext.GetOpenIddictServerRequest() ??
                throw new InvalidOperationException("The OpenID Connect request cannot be retrieved.");

            // Retrieve the user principal stored in the authentication cookie.
            var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // If the user principal can't be extracted, redirect the user to the login page.
            if (!result.Succeeded)
            {
                return Challenge(
                    authenticationSchemes: CookieAuthenticationDefaults.AuthenticationScheme,
                    properties: new AuthenticationProperties
                    {
                        RedirectUri = Request.PathBase + Request.Path + QueryString.Create(
                            Request.HasFormContentType ? Request.Form.ToList() : Request.Query.ToList())
                    });
            }

            var appUser = await _userManager.FindByNameAsync(result.Principal.Identity!.Name!);
            if (appUser == null)
            {
                return Challenge(
                    authenticationSchemes: CookieAuthenticationDefaults.AuthenticationScheme,
                    properties: new AuthenticationProperties
                    {
                        RedirectUri = Request.PathBase + Request.Path + QueryString.Create(
                            Request.HasFormContentType ? Request.Form.ToList() : Request.Query.ToList())
                    });
            }
            var roles = await _userManager.GetRolesAsync(appUser);

            // 
            var claims = CreateTokenClaims(request, appUser, roles);
            var claimsIdentity = new ClaimsIdentity(claims, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);

            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            // Set requested scopes (this is not done automatically)
            claimsPrincipal.SetScopes(request.GetScopes());

            // Signing in with the OpenIddict authentiction scheme trigger OpenIddict to issue a code (which can be exchanged for an access token)
            return SignIn(claimsPrincipal, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
        }

        [HttpPost("~/connect/endsession")]
        [HttpGet("~/connect/endsession")]
        public async Task<IActionResult> LogoutPost()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return SignOut(
                  authenticationSchemes: OpenIddictServerAspNetCoreDefaults.AuthenticationScheme,
                  properties: new AuthenticationProperties
                  {
                      RedirectUri = "/"
                  });
        }

        [Authorize(AuthenticationSchemes = OpenIddictServerAspNetCoreDefaults.AuthenticationScheme)]
        [HttpGet("~/connect/userinfo")]
        public async Task<IActionResult> Userinfo()
        {
            var claimsPrincipal = (await HttpContext.AuthenticateAsync(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme)).Principal;
            return Ok(new
            {
                Name = claimsPrincipal.GetClaim(OpenIddictConstants.Claims.Name),
                Given_name = claimsPrincipal.GetClaim(OpenIddictConstants.Claims.GivenName),
                Sub = claimsPrincipal.GetClaim(OpenIddictConstants.Claims.Subject),
                Role = claimsPrincipal.GetClaim(OpenIddictConstants.Claims.Role),
                Email = claimsPrincipal.GetClaim(OpenIddictConstants.Claims.Email)
            });
        }

        private List<Claim> CreateTokenClaims(OpenIddictRequest request, ApplicationUser appUser, IList<string> roles)
        {
            var claims = roles
                .Select(role => new Claim(ClaimTypes.Role, role).SetDestinations(OpenIddictConstants.Destinations.AccessToken)).ToList();

            claims.Add(new Claim(OpenIddictConstants.Claims.Subject, appUser.Id));

            claims.Add(new Claim(OpenIddictConstants.Claims.Email, appUser.Email!)
                .SetDestinations(OpenIddictConstants.Destinations.IdentityToken)
                .SetDestinations(OpenIddictConstants.Destinations.AccessToken));

            claims.Add(new Claim(OpenIddictConstants.Claims.Name, appUser.Name!)
                .SetDestinations(OpenIddictConstants.Destinations.AccessToken));

            claims.Add(new Claim(OpenIddictConstants.Claims.GivenName, appUser.Name!)
                .SetDestinations(OpenIddictConstants.Destinations.AccessToken));

            claims.Add(new Claim(OpenIddictConstants.Claims.ClientId, request.ClientId!)
                .SetDestinations(OpenIddictConstants.Destinations.AccessToken));

            claims.Add(new Claim(OpenIddictConstants.Claims.Role, roles.FirstOrDefault()!)
                .SetDestinations(OpenIddictConstants.Destinations.AccessToken));

            return claims;
        }
    }
}
