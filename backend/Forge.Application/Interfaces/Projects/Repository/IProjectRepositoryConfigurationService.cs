using System;
using Forge.Application.DTOs.Projects.Repository;

namespace Forge.Application.Interfaces.Projects.Repository
{
    public interface IProjectRepositoryConfigurationService
    {
        Task<RepositoryResponse?> GetAsync(Guid userId, Guid projectId);
        Task<RepositoryResponse> CreateAsync(Guid userId, Guid projectId, CreateRepositoryRequest request);
        Task<RepositoryResponse> UpdateAsync(Guid userId, Guid projectId, UpdateRepositoryRequest request);
        Task DeleteAsync(Guid userId, Guid projectId);
    }
}


