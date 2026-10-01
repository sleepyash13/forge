using System;
using Forge.Domain.Entities;

namespace Forge.Application.Interfaces.Projects.Repository
{
    public interface IProjectRepositoryConfigurationRepository
    {
        Task<ProjectRepositoryConfiguration?> GetByProjectIdAsync(Guid projectId);
        Task<ProjectRepositoryConfiguration?> GetByIdAsync(Guid projectRepositoryConfigurationId);
        Task AddAsync(ProjectRepositoryConfiguration projectRepositoryConfiguration);
        Task RemoveAsync(ProjectRepositoryConfiguration projectRepositoryConfiguration);
        Task SaveChangesAsync();
    }
}
