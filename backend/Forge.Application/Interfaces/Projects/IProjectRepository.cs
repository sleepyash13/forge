using Forge.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Forge.Application.Interfaces.Projects
{
    public interface IProjectRepository
    {
        Task<Project?> GetByIdAsync(Guid projectId);
        Task AddAsync(Project project);
        Task SaveChangesAsync();
        Task DeleteAsync(Project project);
        Task<List<Project>> GetByUserIdAsync(Guid userId);
    }
}
