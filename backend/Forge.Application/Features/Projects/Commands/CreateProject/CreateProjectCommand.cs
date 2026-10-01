using Forge.Application.DTOs.Projects;
using MediatR;

namespace Forge.Application.Features.Projects.Commands.CreateProject;

public sealed record CreateProjectCommand(Guid UserId, CreateProjectRequest Request)
    : IRequest<ProjectResponse>;
