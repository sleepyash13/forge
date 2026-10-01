using Forge.Application.DTOs.Projects.Repository;
using Forge.Domain.Entities;

namespace Forge.Application.Features.ProjectRepositories;

internal static class RepositoryResponseMapper
{
    public static RepositoryResponse ToResponse(ProjectRepositoryConfiguration repository)
    {
        return new RepositoryResponse
        {
            Id = repository.Id,
            ProjectId = repository.ProjectId,
            RepositoryUrl = repository.RepositoryUrl,
            DefaultBranch = repository.DefaultBranch,
            BuildCommand = repository.BuildCommand,
            DeployConfiguration = repository.DeployConfiguration,
            CreatedAt = repository.CreatedAt,
            UpdatedAt = repository.UpdatedAt
        };
    }
}
