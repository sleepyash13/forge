using Forge.Application.DTOs.Projects;
using Forge.Application.Interfaces.Projects;
using Forge.Application.Interfaces.Users;
using Forge.Domain.Entities;
using Forge.Domain.Enums;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Forge.Application.Services.Projects
{
    public class ProjectMemberService : IProjectMemberService
    {
        private readonly IProjectMemberRepository _memberRepository;
        private readonly IUserRepository _userRepository;
        private readonly IProjectAuthorizationService _authorizationService;
        private readonly ILogger<ProjectMemberService> _logger;
        public ProjectMemberService(IProjectMemberRepository memberRepository, IUserRepository userRepository,
            IProjectAuthorizationService authorizationService, ILogger<ProjectMemberService> logger)
        {
            _memberRepository = memberRepository;
            _userRepository = userRepository;
            _authorizationService = authorizationService;
            _logger = logger;
        }

        public async Task<List<ProjectMemberResponse>> GetMembersAsync(Guid currentUserId, Guid projectId)
        {
            try
            {
                var allowed = await _authorizationService.HasPermissionAsync(currentUserId, projectId, ProjectPermission.ViewProject);

                if (!allowed)
                {
                    throw new UnauthorizedAccessException("You do not have permission to view this project.");
                }

                var members = await _memberRepository.GetProjectMembersAsync(projectId);

                return members.Select(MapToResponse).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving members for project {ProjectId} by user {UserId}", projectId, currentUserId);

                throw;
            }
        }

        public async Task<ProjectMemberResponse> AddMemberAsync(Guid currentUserId, Guid projectId, AddProjectMemberRequest request)
        {
            try
            {
                var allowed = await _authorizationService.HasPermissionAsync(currentUserId, projectId, ProjectPermission.ManageMembers);

                if (!allowed) { throw new UnauthorizedAccessException("You do not have permission to manage project members."); }
                if (string.IsNullOrWhiteSpace(request.Email)) { throw new ArgumentException("Email is required."); }
                if (!Enum.TryParse<ProjectRole>(request.Role, true, out var role)) { throw new ArgumentException("Invalid project role."); }

                var user = await _userRepository.GetByEmailAsync(request.Email.Trim().ToLowerInvariant());

                if (user is null) { throw new KeyNotFoundException("A user with the specified email was not found."); }

                var existing = await _memberRepository.GetMembershipAsync(user.Id, projectId);

                if (existing is not null) { throw new InvalidOperationException("The user is already a member of this project."); }

                if (role == ProjectRole.Owner)
                {
                    var currentMembership = await _memberRepository.GetMembershipAsync(currentUserId, projectId);
                    if (currentMembership?.Role != ProjectRole.Owner)
                    { 
                        throw new UnauthorizedAccessException("Only the project owner can assign the Owner role."); 
                    }
                }

                var member = new ProjectMember
                {
                    Id = Guid.NewGuid(),
                    ProjectId = projectId,
                    UserId = user.Id,
                    Role = role,
                    CreatedAt = DateTime.UtcNow
                };

                await _memberRepository.AddAsync(member);
                await _memberRepository.SaveChangesAsync();

                _logger.LogInformation(
                    "User {TargetUserId} added to project {ProjectId} with role {Role} by user {CurrentUserId}",
                    user.Id, projectId, role, currentUserId
                );

                member.User = user;

                return MapToResponse(member);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding member to project {ProjectId} by user {UserId}", projectId, currentUserId);

                throw;
            }
        }

        public async Task<ProjectMemberResponse> UpdateRoleAsync(Guid currentUserId, Guid projectId, Guid targetUserId, UpdateProjectMemberRoleRequest request)
        {
            try
            {
                var allowed = await _authorizationService.HasPermissionAsync(currentUserId, projectId, ProjectPermission.ManageMembers);

                if (!allowed) throw new UnauthorizedAccessException("You do not have permission to manage project members.");
                if (!Enum.TryParse<ProjectRole>(request.Role, true, out var newRole)) throw new ArgumentException("Invalid project role.");

                var targetMembership = await _memberRepository.GetMembershipAsync(targetUserId, projectId);

                if (targetMembership is null) throw new KeyNotFoundException("The specified user is not a member of this project."); 

                var currentMembership = await _memberRepository.GetMembershipAsync(currentUserId, projectId);
                if (currentMembership is null) throw new UnauthorizedAccessException("You are not a member of this project.");

                if (newRole == ProjectRole.Owner && currentMembership.Role != ProjectRole.Owner)
                {
                    throw new UnauthorizedAccessException("Only the project owner can assign the Owner role.");
                }

                // Only Owner can modify an existing Owner.
                if (targetMembership.Role == ProjectRole.Owner && currentMembership.Role != ProjectRole.Owner)
                {
                    throw new UnauthorizedAccessException("Only the project owner can modify the owner role.");
                }

                // Prevent removing the last Owner through role changes.
                if (targetMembership.Role == ProjectRole.Owner && newRole != ProjectRole.Owner)
                {
                    var members = await _memberRepository.GetProjectMembersAsync(projectId);
                    var ownerCount = members.Count(x => x.Role == ProjectRole.Owner);

                    if (ownerCount <= 1) {
                        throw new InvalidOperationException("A project must have at least one owner.");
                    }
                }

                var oldRole = targetMembership.Role;
                targetMembership.Role = newRole;
                targetMembership.UpdatedAt = DateTime.UtcNow;

                await _memberRepository.SaveChangesAsync();

                _logger.LogInformation(
                    "Role changed for user {TargetUserId} in project {ProjectId} from {OldRole} to {NewRole} by user {CurrentUserId}",
                    targetUserId,
                    projectId,
                    targetMembership.Role,
                    newRole,
                    currentUserId
                );

                return MapToResponse(targetMembership);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error changing role for user {TargetUserId} in project {ProjectId}", targetUserId, projectId);

                throw;
            }
        }

        public async Task RemoveMemberAsync(Guid currentUserId, Guid projectId, Guid targetUserId)
        {
            try
            {
                var allowed = await _authorizationService.HasPermissionAsync(currentUserId, projectId, ProjectPermission.ManageMembers);

                if (!allowed) throw new UnauthorizedAccessException("You do not have permission to manage project members.");

                var targetMembership = await _memberRepository.GetMembershipAsync(targetUserId, projectId);

                if (targetMembership is null) throw new KeyNotFoundException("The specified user is not a member of this project.");

                var currentMembership = await _memberRepository.GetMembershipAsync(currentUserId, projectId);

                if (currentMembership is null) throw new UnauthorizedAccessException("You are not a member of this project.");

                if (targetMembership.Role == ProjectRole.Owner && currentMembership.Role != ProjectRole.Owner)
                {
                    throw new UnauthorizedAccessException("Only the project owner can remove an owner.");
                }

                // Prevent removing the last Owner.
                if (targetMembership.Role == ProjectRole.Owner)
                {
                    var members =await _memberRepository.GetProjectMembersAsync(projectId);
                    var ownerCount = members.Count(x => x.Role == ProjectRole.Owner);

                    if (ownerCount <= 1)
                    {
                        throw new InvalidOperationException("A project must have at least one owner.");
                    }
                }

                await _memberRepository.RemoveAsync(targetMembership);
                await _memberRepository.SaveChangesAsync();

                _logger.LogInformation("User {TargetUserId} removed from project {ProjectId} by user {CurrentUserId}",
                    targetUserId,
                    projectId,
                    currentUserId
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removing user {TargetUserId} from project {ProjectId}", targetUserId, projectId);

                throw;
            }
        }

        private static ProjectMemberResponse MapToResponse(ProjectMember member)
        {
            var user = member.User ?? throw new InvalidOperationException("Project member user information was not loaded.");

            return new ProjectMemberResponse
            {
                UserId = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                MiddleName = user.MiddleName,
                LastName = user.LastName,
                DisplayName = user.DisplayName,
                Role = member.Role.ToString(),
                CreatedAt = member.CreatedAt,
                UpdatedAt = member.UpdatedAt
            };
        }
    }
}
