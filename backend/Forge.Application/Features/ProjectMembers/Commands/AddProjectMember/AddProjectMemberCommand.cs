using Forge.Application.DTOs.Projects;
using MediatR;

namespace Forge.Application.Features.ProjectMembers.Commands.AddProjectMember;

public sealed record AddProjectMemberCommand(
    Guid CurrentUserId,
    Guid ProjectId,
    AddProjectMemberRequest Request) : IRequest<ProjectMemberResponse>;
