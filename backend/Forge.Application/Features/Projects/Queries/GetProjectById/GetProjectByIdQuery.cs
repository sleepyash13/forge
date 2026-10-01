using Forge.Application.DTOs.Projects;
using MediatR;

namespace Forge.Application.Features.Projects.Queries.GetProjectById;

public sealed record GetProjectByIdQuery(Guid UserId, Guid ProjectId)
    : IRequest<ProjectResponse>;
