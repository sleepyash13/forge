using System;
using System.Collections.Generic;
using System.Text;

namespace Forge.Domain.Entities
{
    public class Project
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsActive { get; set; }
        public ICollection<ProjectMember> Members { get; set; } = new List<ProjectMember>();
    }
}
