using Forge.Domain.Entities;
using Forge.Infrastructure.Persistence;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Forge.Infrastructure.Logging
{
    public class DatabaseLogger : ILogger
    {
        private readonly string _categoryName;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DatabaseLogger( string categoryName, IServiceScopeFactory scopeFactory,
            IHttpContextAccessor httpContextAccessor)
        {
            _categoryName = categoryName;
            _scopeFactory = scopeFactory;
            _httpContextAccessor = httpContextAccessor;
        }

        public IDisposable? BeginScope<TState>(TState state) where TState : notnull
        {
            return NullScope.Instance;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            if (logLevel < LogLevel.Information) return false;

            // Avoid framework/database logging noise.
            if (_categoryName.StartsWith("Microsoft.", StringComparison.OrdinalIgnoreCase))
                return false;

            if (_categoryName.StartsWith("System.", StringComparison.OrdinalIgnoreCase))
                return false;

            return true;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception,
            Func<TState, Exception?, string> formatter)
        {
            if (!IsEnabled(logLevel)) return;

            try
            {
                var httpContext = _httpContextAccessor.HttpContext;

                Guid? userId = null;

                var userIdClaim = httpContext?.User?.FindFirst(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub);

                if (userIdClaim != null && Guid.TryParse(userIdClaim.Value, out var parsedUserId))
                {
                    userId = parsedUserId;
                }

                var log = new ApplicationLog
                {
                    Id = Guid.NewGuid(),
                    Level = logLevel.ToString(),
                    Source = _categoryName,
                    Description = formatter(state, exception),
                    Exception = exception?.ToString(),
                    UserId = userId,
                    RequestPath = httpContext?.Request.Path.Value,
                    RequestMethod = httpContext?.Request.Method,
                    CreatedAt = DateTime.UtcNow
                };

                using var scope = _scopeFactory.CreateScope();

                var dbContext = scope.ServiceProvider.GetRequiredService<ForgeDbContext>();

                dbContext.ApplicationLogs.Add(log);
                dbContext.SaveChanges();
            }
            catch
            {
                // Never allow database logging failure
                // to break the actual application operation.
            }
        }

        private sealed class NullScope : IDisposable
        {
            public static readonly NullScope Instance = new();

            public void Dispose()
            {
            }
        }
    }
}
