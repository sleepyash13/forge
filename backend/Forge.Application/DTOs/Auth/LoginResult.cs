using System;
using System.Collections.Generic;
using System.Text;

namespace Forge.Application.DTOs.Auth
{
    public class LoginResult
    {
        public string AccessToken { get; set; } = string.Empty;
        public Guid UserId { get; set; }
        public string Email { get; set; } = string.Empty;
        public string? DisplayName { get; set; }
    }
}
