using System;
using Microsoft.EntityFrameworkCore;
using Forge.Application.Interfaces.Projects.Repository;
using Forge.Domain.Entities;
using Forge.Infrastructure.Persistence;


namespace Forge.Infrastructure.Repositories
{
    public class ProjectRepositoryConfigurationRepository : IProjectRepositoryConfigurationRepository
    {
        private readonly ForgeDbContext _dbContext;

        public ProjectRepositoryConfigurationRepository(ForgeDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ProjectRepositoryConfiguration?> GetByProjectIdAsync(Guid projectId)
        {
            return await _dbContext.ProjectRepositoryConfigurations.FirstOrDefaultAsync(x => x.ProjectId == projectId);
        }

        public async Task<ProjectRepositoryConfiguration?> GetByIdAsync(Guid projectRepositoryConfigurationId)
        {
            return await _dbContext.ProjectRepositoryConfigurations.FirstOrDefaultAsync(x => x.Id == projectRepositoryConfigurationId);
        }

        public async Task AddAsync(ProjectRepositoryConfiguration projectRepositoryConfiguration)
        {
            await _dbContext.ProjectRepositoryConfigurations.AddAsync(projectRepositoryConfiguration);
        }

        public Task RemoveAsync(ProjectRepositoryConfiguration projectRepositoryConfiguration)
        {
            _dbContext.ProjectRepositoryConfigurations.Remove(projectRepositoryConfiguration);

            return Task.CompletedTask;
        }

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}

