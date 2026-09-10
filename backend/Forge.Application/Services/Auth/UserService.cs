using Forge.Application.DTOs.User;
using Forge.Application.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Forge.Application.Services.Auth
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<UserService> _logger;

        public UserService(IUserRepository userRepository, ILogger<UserService> logger)
        {
            _userRepository = userRepository;
            _logger = logger;
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
                    CreatedAt = user.CreatedAt
                };
            }
            catch (UnauthorizedAccessException) { throw; }
            catch (Exception ex)
            {
                _logger.LogError( ex, "Unexpected error retrieving profile for user {UserId}", userId);

                throw;
            }
        }
    }
}
