using MediatR;

namespace Forge.Application.Features.Projects.Commands.DeleteProject;

public sealed record DeleteProjectCommand(Guid UserId, Guid ProjectId) : IRequest<Unit>;
