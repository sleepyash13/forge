using Forge.Application.DTOs.Projects.Repository;
using MediatR;

namespace Forge.Application.Features.RepositoryConfigurations.Commands.UpdateRepositoryConfiguration;

public sealed record UpdateRepositoryConfigurationCommand(
    Guid UserId,
    Guid ProjectId,
    UpdateRepositoryRequest Request) : IRequest<RepositoryResponse>;
