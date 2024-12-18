using Identity.Data;
using Identity.Interfaces;
using Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Controllers
{
    public class UsersController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IAccountService _accountService;
        private readonly ILogger<UsersController> _logger;

        public UsersController(UserManager<ApplicationUser> userManager, IAccountService accountService, ILogger<UsersController> logger)
        {
            _userManager = userManager;
            _accountService = accountService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<ActionResult> AddUser([FromBody] UserRoleViewModel viewModel)
        {
            var activeDirectoryUser = await _accountService.GetUserByEmail(viewModel.Email, CancellationToken.None);

            if (activeDirectoryUser == null)
            {
                _logger.LogInformation($"User with email:{viewModel.Email} doesn't exists");
                return BadRequest("EmailNotRegistered");
            }
            var user = await _userManager.FindByEmailAsync(viewModel.Email);
            if (user != null)
            {
                _logger.LogInformation($"User with email:{viewModel.Email} already exists");
                return BadRequest("UserAlreadyExists");
            }
            activeDirectoryUser.CreatedAt = DateTimeOffset.UtcNow;
            activeDirectoryUser.UserName = GetUsername(activeDirectoryUser.Email);

            var createUserResult = await _userManager.CreateAsync(activeDirectoryUser);
            if (createUserResult.Succeeded)
            {
                var result = await _userManager.AddToRoleAsync(activeDirectoryUser, viewModel.Role);
                if (result.Succeeded)
                    return Ok();

                _logger.LogError(string.Join(",", result.Errors.Select(c => c.Description)));
                return BadRequest(result.Errors);
            }

            _logger.LogError(string.Join(",", createUserResult.Errors.Select(c => c.Description)));
            return BadRequest(createUserResult.Errors);
        }

        [HttpPut("user-role")]
        public async Task<ActionResult> AddUserToRole([FromBody] UserRoleViewModel viewModel)
        {
            var user = await _userManager.FindByEmailAsync(viewModel.Email);
            if (user == null)
            {
                _logger.LogInformation($"User with email:{viewModel.Email} doesn't exists");
                return BadRequest("EmailNotRegistered");
            }
            var isInRole = await _userManager.IsInRoleAsync(user, viewModel.Role);
            if (isInRole)
                return Ok();

            var userRoles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, userRoles);

            var result = await _userManager.AddToRoleAsync(user, viewModel.Role);
            if (result.Succeeded)
                return Ok();

            _logger.LogError(string.Join(",", result.Errors.Select(c => c.Description)));
            return BadRequest(result.Errors);
        }

        [HttpDelete]
        public async Task<ActionResult> RemoveUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                _logger.LogInformation($"User with Id:{userId} doesn't exists");
                return BadRequest("EmailNotRegistered");
            }
            var result = await _userManager.DeleteAsync(user);

            if (result.Succeeded)
                return Ok();

            _logger.LogError(string.Join(",", result.Errors.Select(c => c.Description)));
            return BadRequest(result.Errors);
        }

        private static string GetUsername(string email) => $"{email?.Split("@")[0]}";
    }
}
