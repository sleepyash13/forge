using System;
using System.Collections.Generic;
using System.Text;

namespace Forge.Application.DTOs.User
{
    public class UserProfileResponse
    {
        public Guid Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string? DisplayName { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
