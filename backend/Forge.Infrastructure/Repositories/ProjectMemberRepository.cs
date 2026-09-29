using Forge.Application.Interfaces.Projects;
using Forge.Domain.Entities;
using Forge.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Forge.Infrastructure.Repositories
{
    public class ProjectMemberRepository : IProjectMemberRepository
    {
        private readonly ForgeDbContext _dbContext;
        private readonly ILogger<ProjectMemberRepository> _logger;

        public ProjectMemberRepository(ForgeDbContext dbContext, ILogger<ProjectMemberRepository> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<ProjectMember?> GetMembershipAsync(Guid userId, Guid projectId)
        {
            try
            {
                var projectMember = await _dbContext.ProjectMembers
                    .Include(x => x.User)
                    .FirstOrDefaultAsync(x => x.UserId == userId && x.ProjectId == projectId);

                return projectMember;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving project membership for user {UserId} and project {ProjectId}", userId, projectId);

                throw;
            }
        }
        public async Task<List<ProjectMember>> GetProjectMembersAsync(Guid projectId)
        {
            try
            {
                return await _dbContext.ProjectMembers
                    .Include(x => x.User)
                    .Where(x => x.ProjectId == projectId)
                    .OrderBy(x => x.CreatedAt)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error retrieving members for project {ProjectId}",
                    projectId);

                throw;
            }
        }

        public async Task AddAsync(ProjectMember member)
        {
            try
            {
                await _dbContext.ProjectMembers.AddAsync(member);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding project membership for user {UserId} and project {ProjectId}", member.UserId, member.ProjectId);

                throw;
            }
        }

        public Task RemoveAsync(ProjectMember member)
        {
            try
            {
                _dbContext.ProjectMembers.Remove(member);

                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removing user {UserId} from project {ProjectId}", member.UserId, member.ProjectId);

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
                _logger.LogError(ex, "Error saving project membership changes.");

                throw;
            }
        }
    }
}
