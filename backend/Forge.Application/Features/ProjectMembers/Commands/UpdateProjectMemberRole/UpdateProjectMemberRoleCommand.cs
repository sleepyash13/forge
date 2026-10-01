using Forge.Application.DTOs.Projects;
using MediatR;

namespace Forge.Application.Features.ProjectMembers.Commands.UpdateProjectMemberRole;

public sealed record UpdateProjectMemberRoleCommand(
    Guid CurrentUserId,
    Guid ProjectId,
    Guid TargetUserId,
    UpdateProjectMemberRoleRequest Request) : IRequest<ProjectMemberResponse>;
