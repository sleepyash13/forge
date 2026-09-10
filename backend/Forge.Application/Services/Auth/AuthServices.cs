using Forge.Application.DTOs.Auth;
using Forge.Application.Interfaces;
using Forge.Domain.Entities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Forge.Application.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordService _passwordService;
        private readonly ILogger<AuthService> _logger;
        private readonly ITokenService _tokenService;

        public AuthService(IUserRepository userRepository, IPasswordService passwordService,
            ILogger<AuthService> logger, ITokenService tokenService)
        {
            _userRepository = userRepository;
            _passwordService = passwordService;
            _logger = logger;
            _tokenService = tokenService;
        }

        public async Task RegisterAsync(RegisterRequest request)
        {
            try
            {
                var email = request.Email.Trim().ToLowerInvariant();

                if (request.Password != request.ConfirmPassword)
                {
                    throw new InvalidOperationException("Passwords do not match.");
                }

                var existingUser = await _userRepository
                    .GetByEmailAsync(request.Email);

                if (existingUser != null)
                {
                    _logger.LogWarning("Registration failed because email {Email} already exists", email);

                    throw new InvalidOperationException("A user with this email already exists.");
                }
                var passwordHash = _passwordService.HashPassword(request.Password);

                var user = new User
                {
                    Id = Guid.NewGuid(),

                    Email = request.Email,

                    DisplayName = request.DisplayName,

                    PasswordHash = passwordHash,

                    CreatedAt = DateTime.UtcNow,

                    IsActive = true
                };

                await _userRepository.AddAsync(user);
                await _userRepository.SaveChangesAsync();

                _logger.LogInformation("User {UserId} registered successfully", user.Id);
            }
            catch (InvalidOperationException) { throw; }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to register user with email {Email}", request.Email);

                throw;
            }
        }

        public async Task<LoginResult> LoginAsync(LoginRequest request)
        {
            try
            {
                var email = request.Email.Trim().ToLowerInvariant();

                _logger.LogInformation("Login attempt for email {Email}", email);

                var user = await _userRepository.GetByEmailAsync(email);

                if (user == null)
                {
                    _logger.LogWarning("Login failed because the user was not found for email {Email}", email);

                    throw new UnauthorizedAccessException("Invalid email or password.");
                }

                if (!user.IsActive)
                {
                    _logger.LogWarning( "Login failed because user {UserId} is inactive", user.Id);

                    throw new UnauthorizedAccessException("Invalid email or password.");
                }

                var passwordValid = _passwordService.VerifyPassword(request.Password, user.PasswordHash);

                if (!passwordValid)
                {
                    _logger.LogWarning("Login failed due to invalid password for user {UserId}", user.Id);

                    throw new UnauthorizedAccessException("Invalid email or password.");
                }

                var accessToken = _tokenService.GenerateToken(user);

                _logger.LogInformation( "User {UserId} logged in successfully", user.Id);

                return new LoginResult
                {
                    AccessToken = accessToken,
                    UserId = user.Id,
                    Email = user.Email,
                    DisplayName = user.DisplayName,
                };
            }
            catch (UnauthorizedAccessException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred during login for email {Email}", request.Email);

                throw;
            }
        }
    }
}
