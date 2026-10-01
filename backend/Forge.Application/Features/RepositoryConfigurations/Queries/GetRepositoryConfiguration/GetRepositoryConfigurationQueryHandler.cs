using Forge.Application.DTOs.Projects.Repository;
using Forge.Application.Interfaces.Projects;
using Forge.Application.Interfaces.Projects.Repository;
using Forge.Domain.Enums;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Forge.Application.Features.RepositoryConfigurations.Queries.GetRepositoryConfiguration;

public sealed class GetRepositoryConfigurationQueryHandler
    : IRequestHandler<GetRepositoryConfigurationQuery, RepositoryResponse?>
{
    private readonly IProjectRepositoryConfigurationRepository _repository;
    private readonly IProjectAuthorizationService _authorizationService;
    private readonly ILogger<GetRepositoryConfigurationQueryHandler> _logger;

    public GetRepositoryConfigurationQueryHandler(
        IProjectRepositoryConfigurationRepository repository,
        IProjectAuthorizationService authorizationService,
        ILogger<GetRepositoryConfigurationQueryHandler> logger)
    {
        _repository = repository;
        _authorizationService = authorizationService;
        _logger = logger;
    }

    public async Task<RepositoryResponse?> Handle(
        GetRepositoryConfigurationQuery query,
        CancellationToken cancellationToken)
    {
        await _authorizationService.HasPermissionAsync(
            query.UserId,
            query.ProjectId,
            ProjectPermission.ViewProject);

        try
        {
            var repository = await _repository.GetByProjectIdAsync(query.ProjectId);
            return repository is null
                ? null
                : RepositoryConfigurationResponseMapper.ToResponse(repository);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Failed to retrieve repository configuration for project {ProjectId} by user {UserId}",
                query.ProjectId,
                query.UserId);
            throw;
        }
    }
}
