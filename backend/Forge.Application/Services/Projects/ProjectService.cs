using Forge.Application.DTOs.Projects;
using Forge.Application.Interfaces.Projects;
using Forge.Application.Mappers;
using Forge.Application.Validators;
using Forge.Domain.Entities;
using Forge.Domain.Enums;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Forge.Application.Services.Projects
{
    public class ProjectService : IProjectService
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IProjectMemberRepository _memberRepository;
        private readonly IProjectAuthorizationService _authorizationService;
        private readonly ILogger<ProjectService> _logger;

        public ProjectService(IProjectRepository projectRepository, IProjectMemberRepository memberRepository,
            IProjectAuthorizationService authorizationService, ILogger<ProjectService> logger)
        {
            _projectRepository = projectRepository;
            _memberRepository = memberRepository;
            _authorizationService = authorizationService;
            _logger = logger;
        }

        public async Task<ProjectResponse> CreateAsync(Guid userId, CreateProjectRequest request)
        {
            try
            {
                ProjectValidator.ValidateName(request.Name);

                var project = new Project
                {
                    Id = Guid.NewGuid(),
                    Name = ProjectValidator.NormalizeName(request.Name),
                    Description = ProjectValidator.NormalizeDescription(request.Description),
                    CreatedAt = DateTime.UtcNow,
                    IsActive = true
                };

                await _projectRepository.AddAsync(project);

                var ownerMembership = new ProjectMember
                {
                    Id = Guid.NewGuid(),
                    ProjectId = project.Id,
                    UserId = userId,
                    Role = ProjectRole.Owner,
                    CreatedAt = DateTime.UtcNow
                };

                await _memberRepository.AddAsync(ownerMembership);
                await _projectRepository.SaveChangesAsync();

                _logger.LogInformation("Project {ProjectId} created by user {UserId}", project.Id, userId);

                return ProjectMapper.ToResponse(project);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating project for user {UserId}", userId);

                throw;
            }
        }

        public async Task<ProjectResponse> GetByIdAsync(Guid userId, Guid projectId)
        {
            try
            {
                var authorized = await _authorizationService.HasPermissionAsync(userId, projectId, ProjectPermission.ViewProject);

                if (!authorized) throw new UnauthorizedAccessException("You do not have permission to view this project.");

                var project = await _projectRepository.GetByIdAsync(projectId);

                if (project is null) throw new KeyNotFoundException("Project was not found.");

                return ProjectMapper.ToResponse(project);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving project {ProjectId} for user {UserId}", projectId, userId);

                throw;
            }
        }

        public async Task<ProjectResponse> UpdateAsync(Guid userId, Guid projectId, UpdateProjectRequest request)
        {
            try
            {
                var authorized = await _authorizationService.HasPermissionAsync(userId, projectId, ProjectPermission.UpdateProject);

                if (!authorized) throw new UnauthorizedAccessException("You do not have permission to update this project.");

                ProjectValidator.ValidateName(request.Name);

                var project = await _projectRepository.GetByIdAsync(projectId);

                if (project is null) throw new KeyNotFoundException("Project was not found.");

                project.Name = ProjectValidator.NormalizeName(request.Name);
                project.Description = ProjectValidator.NormalizeDescription(request.Description);
                project.UpdatedAt = DateTime.UtcNow;

                await _projectRepository.SaveChangesAsync();

                _logger.LogInformation(
                    "Project {ProjectId} updated by user {UserId}",
                    projectId,
                    userId);

                return ProjectMapper.ToResponse(project);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating project {ProjectId} for user {UserId}", projectId, userId);

                throw;
            }
        }

        public async Task DeleteAsync(Guid userId, Guid projectId)
        {
            try
            {
                var authorized = await _authorizationService.HasPermissionAsync(userId, projectId, ProjectPermission.DeleteProject);

                if (!authorized) throw new UnauthorizedAccessException("You do not have permission to delete this project.");

                var project = await _projectRepository.GetByIdAsync(projectId);

                if (project is null) throw new KeyNotFoundException("Project was not found.");

                await _projectRepository.DeleteAsync(project);
                await _projectRepository.SaveChangesAsync();

                _logger.LogInformation("Project {ProjectId} deleted by user {UserId}", projectId, userId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting project {ProjectId} for user {UserId}", projectId, userId);

                throw;
            }
        }

        public async Task<List<ProjectResponse>> GetMyProjectsAsync(Guid userId)
        {
            try
            {
                var projects = await _projectRepository.GetByUserIdAsync(userId);

                return projects.Select(ProjectMapper.ToResponse).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving projects for user {UserId}", userId);

                throw;
            }
        }
    }
}
