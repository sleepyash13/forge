using System;
using System.Collections.Generic;
using System.Text;

namespace Forge.Application.DTOs.Projects
{
    public class UpdateProjectRequest
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
    }
}
