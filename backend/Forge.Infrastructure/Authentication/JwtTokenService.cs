using Forge.Application.Interfaces;
using Forge.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Forge.Infrastructure.Authentication
{
    public class JwtTokenService : ITokenService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<JwtTokenService> _logger;

        public JwtTokenService(IConfiguration configuration, ILogger<JwtTokenService> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public string GenerateToken(User user)
        {
            try
            {
                var jwtConfig = _configuration.GetSection("Jwt");

                var key = jwtConfig["Key"];
                var issuer = jwtConfig["Issuer"];
                var audience = jwtConfig["Audience"];

                if (string.IsNullOrWhiteSpace(key))
                {
                    throw new InvalidOperationException(
                        "JWT Key is not configured.");
                }

                var expiryMinutes = double.Parse(jwtConfig["Expiry"] ?? "60");
                var claims = new List<Claim>
                {
                    new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                    new(JwtRegisteredClaimNames.Email, user.Email.ToString()),
                    new(ClaimTypes.Name, user.DisplayName ?? user.Email)
                };

                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
                var creds = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
                var token = new JwtSecurityToken(
                        issuer: issuer,
                        audience: audience,
                        claims: claims,
                        expires: DateTime.UtcNow.AddMinutes(expiryMinutes),
                        signingCredentials: creds 
                    );

                var tokenVal = new JwtSecurityTokenHandler().WriteToken(token);

                _logger.LogInformation("JWT Token generated for the user {UserId}", user.Id);

                return tokenVal;
            }
            catch (Exception ex)
            {
                _logger.LogError(
                   ex,
                   "Failed to generate JWT for user {UserId}",
                   user.Id);

                throw;
            }
        }
    }
}
