using Forge.Application.DTOs.Projects;
using Forge.Application.Interfaces.Projects;
using Forge.Domain.Enums;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Forge.Application.Features.ProjectMembers.Commands.UpdateProjectMemberRole;

public sealed class UpdateProjectMemberRoleCommandHandler
    : IRequestHandler<UpdateProjectMemberRoleCommand, ProjectMemberResponse>
{
    private readonly IProjectMemberRepository _memberRepository;
    private readonly IProjectAuthorizationService _authorizationService;
    private readonly ILogger<UpdateProjectMemberRoleCommandHandler> _logger;

    public UpdateProjectMemberRoleCommandHandler(
        IProjectMemberRepository memberRepository,
        IProjectAuthorizationService authorizationService,
        ILogger<UpdateProjectMemberRoleCommandHandler> logger)
    {
        _memberRepository = memberRepository;
        _authorizationService = authorizationService;
        _logger = logger;
    }

    public async Task<ProjectMemberResponse> Handle(
        UpdateProjectMemberRoleCommand command,
        CancellationToken cancellationToken)
    {
        try
        {
            var allowed = await _authorizationService.HasPermissionAsync(
                command.CurrentUserId,
                command.ProjectId,
                ProjectPermission.ManageMembers);

            if (!allowed)
            {
                throw new UnauthorizedAccessException(
                    "You do not have permission to manage project members.");
            }

            if (!Enum.TryParse<ProjectRole>(command.Request.Role, true, out var newRole))
            {
                throw new ArgumentException("Invalid project role.");
            }

            var targetMembership = await _memberRepository.GetMembershipAsync(
                command.TargetUserId,
                command.ProjectId);
            if (targetMembership is null)
            {
                throw new KeyNotFoundException(
                    "The specified user is not a member of this project.");
            }

            var currentMembership = await _memberRepository.GetMembershipAsync(
                command.CurrentUserId,
                command.ProjectId);
            if (currentMembership is null)
            {
                throw new UnauthorizedAccessException(
                    "You are not a member of this project.");
            }

            if (newRole == ProjectRole.Owner && currentMembership.Role != ProjectRole.Owner)
            {
                throw new UnauthorizedAccessException(
                    "Only the project owner can assign the Owner role.");
            }

            if (targetMembership.Role == ProjectRole.Owner &&
                currentMembership.Role != ProjectRole.Owner)
            {
                throw new UnauthorizedAccessException(
                    "Only the project owner can modify the owner role.");
            }

            if (targetMembership.Role == ProjectRole.Owner && newRole != ProjectRole.Owner)
            {
                var members = await _memberRepository.GetProjectMembersAsync(command.ProjectId);
                var ownerCount = members.Count(member => member.Role == ProjectRole.Owner);

                if (ownerCount <= 1)
                {
                    throw new InvalidOperationException(
                        "A project must have at least one owner.");
                }
            }

            targetMembership.Role = newRole;
            targetMembership.UpdatedAt = DateTime.UtcNow;

            await _memberRepository.SaveChangesAsync();

            _logger.LogInformation(
                "Role changed for user {TargetUserId} in project {ProjectId} from {OldRole} to {NewRole} by user {CurrentUserId}",
                command.TargetUserId,
                command.ProjectId,
                targetMembership.Role,
                newRole,
                command.CurrentUserId);

            return ProjectMemberResponseMapper.ToResponse(targetMembership);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Error changing role for user {TargetUserId} in project {ProjectId}",
                command.TargetUserId,
                command.ProjectId);
            throw;
        }
    }
}
