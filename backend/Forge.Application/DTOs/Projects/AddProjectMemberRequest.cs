using System;
using System.Collections.Generic;
using System.Text;

namespace Forge.Application.DTOs.Projects
{
    public class AddProjectMemberRequest
    {
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
    }
}
