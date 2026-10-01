using System;

namespace Forge.Domain.Entities
{
    public class ProjectRepositoryConfiguration
    {
        public Guid Id { get; set; }
        public Guid ProjectId { get; set; }
        public string RepositoryUrl { get; set; } = string.Empty;
        public string DefaultBranch { get; set; } = string.Empty;
        public string? BuildCommand { get; set; }
        public string? DeployConfiguration { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public Project? Project { get; set; }
    }
}