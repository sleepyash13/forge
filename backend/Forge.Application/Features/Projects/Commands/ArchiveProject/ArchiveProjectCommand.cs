using MediatR;

namespace Forge.Application.Features.Projects.Commands.ArchiveProject;

public sealed record ArchiveProjectCommand(Guid UserId, Guid ProjectId) : IRequest<Unit>;
