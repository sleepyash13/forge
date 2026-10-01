using Forge.Application.DTOs.Projects.Repository;
using MediatR;

namespace Forge.Application.Features.RepositoryConfigurations.Queries.GetRepositoryConfiguration;

public sealed record GetRepositoryConfigurationQuery(Guid UserId, Guid ProjectId)
    : IRequest<RepositoryResponse?>;
