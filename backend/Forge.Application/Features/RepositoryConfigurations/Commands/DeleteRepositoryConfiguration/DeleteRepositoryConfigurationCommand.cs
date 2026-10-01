using MediatR;

namespace Forge.Application.Features.RepositoryConfigurations.Commands.DeleteRepositoryConfiguration;

public sealed record DeleteRepositoryConfigurationCommand(Guid UserId, Guid ProjectId)
    : IRequest<Unit>;
