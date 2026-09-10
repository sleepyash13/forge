using Forge.Application.Interfaces;
using Forge.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Forge.Application.Services.Auth
{
    public class PasswordService : IPasswordService
    {
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly ILogger<PasswordService> _logger;

        public PasswordService(IPasswordHasher<User> passwordHasher,
            ILogger<PasswordService> logger)
        {
            _passwordHasher = passwordHasher;
            _logger = logger;
        }

        public string HashPassword(string password)
        {
            try
            {
                User user = new User();

                return _passwordHasher.HashPassword(user, password);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                   ex,
                   "An unexpected error occurred while hashing a password.");

                throw;
            }
        }

        public bool VerifyPassword(string password, string passwordHash)
        {
            try
            {
                var user = new User();

                var result =
                    _passwordHasher.VerifyHashedPassword(
                        user,
                        passwordHash,
                        password);

                return result == PasswordVerificationResult.Success ||
                       result == PasswordVerificationResult.SuccessRehashNeeded;
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "An unexpected error occurred while verifying a password.");

                throw;
            }
        }
    }
}
