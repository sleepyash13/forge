using Forge.Application.Interfaces.Projects;
using Forge.Domain.Enums;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Forge.Application.Services.Projects
{
    public class ProjectAuthorizationService : IProjectAuthorizationService
    {
        private readonly IProjectMemberRepository _memberRepository;
        private readonly ILogger<ProjectAuthorizationService> _logger;

        public ProjectAuthorizationService(IProjectMemberRepository memberRepository, 
            ILogger<ProjectAuthorizationService> logger)
        {
            _memberRepository = memberRepository;
            _logger = logger;
        }

        public async Task<bool> HasPermissionAsync(Guid userId, Guid projectId, ProjectPermission permission)
        {
            try
            {
                var membership = await _memberRepository.GetMembershipAsync(userId, projectId);

                if (membership is null)
                {
                    _logger.LogWarning(
                        "Authorization denied. User {UserId} is not a member of project {ProjectId}. Permission: {Permission}",
                        userId,
                        projectId,
                        permission
                    );

                    return false;
                }

                var allowed = IsPermissionAllowed(membership.Role, permission);

                if (!allowed)
                {
                    _logger.LogWarning(
                        "Authorization denied. User {UserId} with role {Role} attempted permission {Permission} on project {ProjectId}",
                        userId,
                        membership.Role,
                        permission,
                        projectId
                    );
                }

                return allowed;
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error checking project authorization for user {UserId}, project {ProjectId}, permission {Permission}",
                    userId,
                    projectId,
                    permission
                );

                throw;
            }

        }

        private static bool IsPermissionAllowed(ProjectRole role, ProjectPermission permission)
        {
            return role switch
            {
                ProjectRole.Owner => permission switch
                {
                    ProjectPermission.ViewProject => true,
                    ProjectPermission.UpdateProject => true,
                    ProjectPermission.ManageMembers => true,
                    ProjectPermission.CreateBuild => true,
                    ProjectPermission.Deploy => true,
                    ProjectPermission.ManageSettings => true,
                    ProjectPermission.DeleteProject => true,
                    _ => false
                },

                ProjectRole.Maintainer => permission switch
                {
                    ProjectPermission.ViewProject => true,
                    ProjectPermission.UpdateProject => true,
                    ProjectPermission.ManageMembers => true,
                    ProjectPermission.CreateBuild => true,
                    ProjectPermission.Deploy => true,
                    ProjectPermission.ManageSettings => true,
                    ProjectPermission.DeleteProject => false,
                    _ => false
                },

                ProjectRole.Developer => permission switch
                {
                    ProjectPermission.ViewProject => true,
                    ProjectPermission.CreateBuild => true,
                    _ => false
                },

                ProjectRole.Viewer => permission switch
                {
                    ProjectPermission.ViewProject => true,
                    _ => false
                },

                _ => false
            };
        }
    }
}
