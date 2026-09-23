using Forge.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Forge.Domain.Entities
{
    public class ProjectMember
    {
        public Guid Id { get; set; }
        public Guid ProjectId { get; set; }
        public Guid UserId { get; set; }
        public ProjectRole Role { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public Project? Project { get; set; }
        public User? User { get; set; }
    }
}
