using Forge.Application.DTOs.Auth;
using Forge.Application.DTOs.User;
using Forge.Application.Interfaces;
using Forge.Application.Validators;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Forge.Application.Services.Auth
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordService _passwordService;
        private readonly ILogger<UserService> _logger;

        public UserService(IUserRepository userRepository, ILogger<UserService> logger,
            IPasswordService passwordService)
        {
            _userRepository = userRepository;
            _logger = logger;
            _passwordService = passwordService;
        }

        public async Task<UserProfileResponse> GetCurrentUserAsync(Guid userId)
        {
            try
            {
                _logger.LogInformation("Retrieving profile for user {UserId}", userId);

                var user = await _userRepository.GetByIdAsync(userId);

                if (user == null)
                {
                    _logger.LogWarning("User {UserId} was not found", userId);

                    throw new UnauthorizedAccessException("User account could not be found.");
                }

                if (!user.IsActive)
                {
                    _logger.LogWarning("User {UserId} is inactive", userId);

                    throw new UnauthorizedAccessException("User account is inactive.");
                }

                return new UserProfileResponse
                {
                    Id = user.Id,
                    Email = user.Email,
                    DisplayName = user.DisplayName,
                    CreatedAt = user.CreatedAt,
                    UpdatedAt = user.UpdatedAt ?? user.CreatedAt
                };
            }
            catch (UnauthorizedAccessException) { throw; }
            catch (Exception ex)
            {
                _logger.LogError( ex, "Unexpected error retrieving profile for user {UserId}", userId);

                throw;
            }
        }

        public async Task<UserProfileResponse> UpdateProfileAsync(Guid userId, UpdateProfileRequest request)
        {
            try
            {
                var user = await _userRepository.GetByIdAsync(userId);

                if (user == null || !user.IsActive)
                {
                    _logger.LogWarning("Profile update failed. User {UserId} was not found or inactive.", userId);

                    throw new KeyNotFoundException("User not found.");
                }

                user.DisplayName = request.DisplayName;

                user.UpdatedAt = DateTime.UtcNow;

                await _userRepository.SaveChangesAsync();

                _logger.LogInformation("Profile updated successfully for user {UserId}.", userId);

                return new UserProfileResponse
                {
                    Id = user.Id,
                    Email = user.Email,
                    DisplayName = user.DisplayName,
                    CreatedAt = user.CreatedAt
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while updating profile for user {UserId}.", userId);

                throw;
            }
        }

        public async Task ChangePasswordAsync(Guid userId, ChangePasswordRequest request)
        {
            try
            {
                var user = await _userRepository.GetByIdAsync(userId);

                if (user == null || !user.IsActive)
                {
                    _logger.LogWarning("Password change failed. User {UserId} was not found or inactive.", userId);

                    throw new KeyNotFoundException("User not found.");
                }

                if (string.IsNullOrWhiteSpace(request.CurrentPassword)) throw new ArgumentException( "Current password is required.");

                if (string.IsNullOrWhiteSpace(request.NewPassword)) throw new ArgumentException("New password is required.");

                if (request.NewPassword != request.ConfirmPassword) throw new ArgumentException("New password and confirmation password do not match.");
                
                
                PasswordValidator.Validate(request.NewPassword);

                if (request.CurrentPassword == request.NewPassword) throw new ArgumentException("New password must be different from your current password.");

                var currentPasswordValid = _passwordService.VerifyPassword(request.CurrentPassword, user.PasswordHash);

                if (!currentPasswordValid)
                {
                    _logger.LogWarning("Password change failed due to invalid current password for user {UserId}.", userId);

                    throw new UnauthorizedAccessException("Current password is incorrect.");
                }

                user.PasswordHash = _passwordService.HashPassword( request.NewPassword);
                user.UpdatedAt = DateTime.UtcNow;

                await _userRepository.SaveChangesAsync();

                _logger.LogInformation( "Password changed successfully for user {UserId}.", userId);
            }
            catch (ArgumentException) { throw; }
            catch (UnauthorizedAccessException) { throw;  }
            catch (KeyNotFoundException) { throw; }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while changing password for user {UserId}.", userId);
                throw;
            }
        }
    }
}
