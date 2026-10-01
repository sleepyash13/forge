using MediatR;

namespace Forge.Application.Features.ProjectMembers.Commands.RemoveProjectMember;

public sealed record RemoveProjectMemberCommand(
    Guid CurrentUserId,
    Guid ProjectId,
    Guid TargetUserId) : IRequest<Unit>;
