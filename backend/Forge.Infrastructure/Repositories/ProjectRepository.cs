using Forge.Application.Interfaces.Projects;
using Forge.Domain.Entities;
using Forge.Infrastructure.Persistence;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Forge.Infrastructure.Repositories
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly ForgeDbContext _dbContext;
        private readonly ILogger<ProjectRepository> _logger;

        public ProjectRepository(ForgeDbContext dbContext, ILogger<ProjectRepository> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<Project?> GetByIdAsync(Guid projectId)
        {
            try
            {
                return await _dbContext.Projects.FirstOrDefaultAsync(x => x.Id == projectId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving project {ProjectId}", projectId);

                throw;
            }
        }

        public async Task AddAsync(Project project)
        {
            try
            {
                await _dbContext.Projects.AddAsync(project);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding project {ProjectId}", project.Id);

                throw;
            }
        }

        public async Task SaveChangesAsync()
        {
            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving project changes");

                throw;
            }
        }

        public async Task DeleteAsync(Project project)
        {
            try
            {
                _dbContext.Projects.Remove(project);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting project {ProjectId}", project.Id);

                throw;
            }
        }

        public async Task<List<Project>> GetByUserIdAsync(Guid userId)
        {
            try
            {
                return await _dbContext.Projects.Where(x => x.Members.Any(m => m.UserId == userId))
                    .OrderByDescending(x => x.CreatedAt)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving projects for user {UserId}", userId);

                throw;
            }
        }
    }
}
