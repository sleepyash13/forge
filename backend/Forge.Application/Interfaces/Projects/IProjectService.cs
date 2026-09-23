using Forge.Application.DTOs.Projects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Forge.Application.Interfaces.Projects
{
    public interface IProjectService
    {
        Task<ProjectResponse> CreateAsync(Guid userId, CreateProjectRequest request);
        Task<ProjectResponse> GetByIdAsync(Guid userId, Guid projectId);
        Task<ProjectResponse> UpdateAsync(Guid userId, Guid projectId, UpdateProjectRequest request);
        Task DeleteAsync(Guid userId, Guid projectId);
        Task<List<ProjectResponse>> GetMyProjectsAsync(Guid userId);
    }
}
