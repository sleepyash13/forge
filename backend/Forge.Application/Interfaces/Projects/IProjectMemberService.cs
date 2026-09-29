using Forge.Application.DTOs.Projects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Forge.Application.Interfaces.Projects
{
    public interface IProjectMemberService
    {
        Task<List<ProjectMemberResponse>> GetMembersAsync(Guid currentUserId, Guid projectId);
        Task<ProjectMemberResponse> AddMemberAsync(Guid currentUserId, Guid projectId, AddProjectMemberRequest request);
        Task<ProjectMemberResponse> UpdateRoleAsync(Guid currentUserId, Guid projectId, Guid targetUserId, UpdateProjectMemberRoleRequest request);
        Task RemoveMemberAsync(Guid currentUserId, Guid projectId, Guid targetUserId);
    }
}
