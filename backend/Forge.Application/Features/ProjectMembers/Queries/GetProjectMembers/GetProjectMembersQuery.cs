using Forge.Application.DTOs.Projects;
using MediatR;

namespace Forge.Application.Features.ProjectMembers.Queries.GetProjectMembers;

public sealed record GetProjectMembersQuery(Guid CurrentUserId, Guid ProjectId)
    : IRequest<List<ProjectMemberResponse>>;
