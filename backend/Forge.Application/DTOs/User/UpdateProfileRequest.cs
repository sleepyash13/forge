using System;
using System.Collections.Generic;
using System.Text;

namespace Forge.Application.DTOs.User
{
    public class UpdateProfileRequest
    {
        public string? DisplayName { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
    }
}
