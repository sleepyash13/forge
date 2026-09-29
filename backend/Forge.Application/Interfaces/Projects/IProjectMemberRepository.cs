using Forge.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Forge.Application.Interfaces.Projects
{
    public interface IProjectMemberRepository
    {
        Task<ProjectMember?> GetMembershipAsync(Guid userId, Guid projectId);
        Task<List<ProjectMember>> GetProjectMembersAsync(Guid projectId);
        Task AddAsync(ProjectMember member);
        Task RemoveAsync(ProjectMember member);
        Task SaveChangesAsync();
    }
}
