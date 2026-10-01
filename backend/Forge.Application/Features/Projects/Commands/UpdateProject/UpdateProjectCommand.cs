using Forge.Application.DTOs.Projects;
using MediatR;

namespace Forge.Application.Features.Projects.Commands.UpdateProject;

public sealed record UpdateProjectCommand(
    Guid UserId,
    Guid ProjectId,
    UpdateProjectRequest Request) : IRequest<ProjectResponse>;
