using System;

namespace Forge.Application.DTOs.Projects.Repository
{
    public class CreateRepositoryRequest
    {
        public string RepositoryUrl { get; set; } = string.Empty;
        public string DefaultBranch { get; set; } = string.Empty;        
        public string? BuildCommand { get; set; }
        public string? DeployConfiguration { get; set; }
    }
}
