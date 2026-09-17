using System;
using System.Collections.Generic;
using System.Text;

namespace Forge.Application.DTOs.Auth
{
    public class RegisterRequest
    {
        public string FirstName { get; set; } = string.Empty;
        public string? MiddleName { get; set; }
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string ConfirmPassword { get; set; } = string.Empty;
        public string? DisplayName { get; set; }
    }
}
