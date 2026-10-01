using Forge.Application.Interfaces.Projects;
using Forge.Domain.Enums;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Forge.Application.Features.ProjectMembers.Commands.RemoveProjectMember;

public sealed class RemoveProjectMemberCommandHandler
    : IRequestHandler<RemoveProjectMemberCommand, Unit>
{
    private readonly IProjectMemberRepository _memberRepository;
    private readonly IProjectAuthorizationService _authorizationService;
    private readonly ILogger<RemoveProjectMemberCommandHandler> _logger;

    public RemoveProjectMemberCommandHandler(
        IProjectMemberRepository memberRepository,
        IProjectAuthorizationService authorizationService,
        ILogger<RemoveProjectMemberCommandHandler> logger)
    {
        _memberRepository = memberRepository;
        _authorizationService = authorizationService;
        _logger = logger;
    }

    public async Task<Unit> Handle(
        RemoveProjectMemberCommand command,
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

            if (targetMembership.Role == ProjectRole.Owner &&
                currentMembership.Role != ProjectRole.Owner)
            {
                throw new UnauthorizedAccessException(
                    "Only the project owner can remove an owner.");
            }

            if (targetMembership.Role == ProjectRole.Owner)
            {
                var members = await _memberRepository.GetProjectMembersAsync(command.ProjectId);
                var ownerCount = members.Count(member => member.Role == ProjectRole.Owner);

                if (ownerCount <= 1)
                {
                    throw new InvalidOperationException(
                        "A project must have at least one owner.");
                }
            }

            await _memberRepository.RemoveAsync(targetMembership);
            await _memberRepository.SaveChangesAsync();

            _logger.LogInformation(
                "User {TargetUserId} removed from project {ProjectId} by user {CurrentUserId}",
                command.TargetUserId,
                command.ProjectId,
                command.CurrentUserId);

            return Unit.Value;
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Error removing user {TargetUserId} from project {ProjectId}",
                command.TargetUserId,
                command.ProjectId);
            throw;
        }
    }
}
