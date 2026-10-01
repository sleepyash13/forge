using Forge.Application.DTOs.Projects.Repository;
using MediatR;

namespace Forge.Application.Features.RepositoryConfigurations.Commands.CreateRepositoryConfiguration;

public sealed record CreateRepositoryConfigurationCommand(
    Guid UserId,
    Guid ProjectId,
    CreateRepositoryRequest Request) : IRequest<RepositoryResponse>;
