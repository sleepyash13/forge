using Forge.Application.Common.Extensions;
using Forge.Application.DTOs.Users;
using Forge.Application.Interfaces.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Forge.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/v1/users")]

    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UsersController> _logger;

        public UsersController(IUserService userService, ILogger<UsersController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        [HttpGet("me")]
        public async Task<IActionResult> GetCurrentUser()
        {
            var userId = User.GetUserId();

            var result = await _userService.GetCurrentUserAsync(userId);

            return Ok(new
            {
                success = true,
                data = result
            });
        }

        [HttpPut("profile")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateProfile(UpdateProfileRequest request)
        {
            var userId = User.GetUserId();

            var result = await _userService.UpdateProfileAsync(userId, request);

            return Ok(new
            {
                success = true,
                message = "Profile updated successfully.",
                data = result
            });
        }
    }
}
