using Forge.Application.DTOs.Auth;
using Forge.Application.Interfaces;
using Forge.Application.Services.Auth;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Forge.Api.Controllers
{
    [ApiController]
    [Route("api/v1/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IAntiforgery _antiforgery;
        private readonly ILogger<AuthController> _logger;
        private readonly IWebHostEnvironment _environment;
        private readonly IUserService _userService;

        public AuthController(IAuthService authService, IAntiforgery antiforgery,
            ILogger<AuthController> logger, IWebHostEnvironment environment,
            IUserService userService)
        {
            _authService = authService;
            _antiforgery = antiforgery;
            _logger = logger;
            _environment = environment;
            _userService = userService;
        }

        [HttpGet("csrf")]
        public IActionResult GetCsrfToken()
        {
            var tokens = _antiforgery.GetAndStoreTokens(HttpContext);

            Response.Cookies.Append("XSRF-TOKEN", tokens.RequestToken!, new CookieOptions
            {
                HttpOnly = false,
                Secure = !_environment.IsDevelopment(),
                SameSite = SameSiteMode.Lax,
                Path = "/"
            });

            return Ok(new
            {
                success = true
            });
        }

        [HttpPost("register")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            _logger.LogInformation("Register endpoint reached.");

            await _authService.RegisterAsync(request);

            return Ok(new
            {
                message = "Congratulations! You’ve successfully registered."
            });
        }

        [HttpPost("login")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var result = await _authService.LoginAsync(request);

            Response.Cookies.Append("forge_auth", result.AccessToken, new CookieOptions{
                    HttpOnly = true,
                    Secure = !_environment.IsDevelopment(),
                    SameSite = SameSiteMode.Lax,
                    Path = "/",
                    MaxAge = TimeSpan.FromMinutes(60)
                });

            return Ok(new
            {
                success = true,
                message = "Login successful.",
                data = new
                {
                    userId = result.UserId,
                    email = result.Email,
                    displayName = result.DisplayName
                }
            });
        }

        [Authorize]
        [HttpGet("me")]
        public async Task<IActionResult> Me()
        {
            var userIdClaim =User.FindFirstValue(JwtRegisteredClaimNames.Sub);
            var emailClaim =User.FindFirstValue(JwtRegisteredClaimNames.Email);
            var displayNameClaim = User.FindFirstValue(ClaimTypes.Name);

            if (!Guid.TryParse(userIdClaim, out var userId))
            {
                _logger.LogWarning("Authenticated request contains an invalid user ID claim.");

                return Unauthorized(new
                {
                    success = false,
                    message = "Invalid authentication information."
                });
            }

            return Ok(new
            {
                success = true,
                data = new
                {
                    userId,
                    email = emailClaim,
                    displayName = displayNameClaim
                }
            });
        }


        [HttpPost("logout")]
        [ValidateAntiForgeryToken]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("forge_auth", new CookieOptions
            {
                HttpOnly = true,
                Secure = !_environment.IsDevelopment(),
                SameSite = SameSiteMode.Lax,
                Path = "/"
            });

            return Ok(new
            {
                success = true,
                message = "Logout successful."
            });
        }

        [Authorize]
        [HttpPut("password")]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> ChangePassword(ChangePasswordRequest request)
        {
            var userIdClaim = User.FindFirstValue(JwtRegisteredClaimNames.Sub);

            if (!Guid.TryParse(userIdClaim, out var userId))
            {
                _logger.LogWarning("Password change request contains an invalid user ID claim.");

                return Unauthorized(new
                {
                    success = false,
                    message = "Invalid authentication information."
                });
            }

            await _userService.ChangePasswordAsync(userId, request);

            _logger.LogInformation("Password change completed and current authentication cookie invalidated for user {UserId}.", userId);

            return Ok(new
            {
                success = true,
                message = "Password changed successfully. Please log in again."
            });
        }
    }
}
