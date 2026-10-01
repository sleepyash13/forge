using MediatR;

namespace Forge.Application.Features.ProjectRepositories.Commands.DeleteRepository;

public sealed record DeleteRepositoryCommand(Guid UserId, Guid ProjectId) : IRequest<Unit>;
