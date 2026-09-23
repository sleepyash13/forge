using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace Forge.Application.Common.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static Guid GetUserId(this ClaimsPrincipal user)
        {
            var value = user.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!Guid.TryParse(value, out var userId)) { throw new UnauthorizedAccessException("Invalid authentication information."); }

            return userId;
        }
    }
}
