using Forge.Application.DTOs.Projects.Repository;
using Forge.Application.Interfaces.Projects;
using Forge.Application.Interfaces.Projects.Repository;
using Forge.Domain.Enums;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Forge.Application.Features.ProjectRepositories.Queries.GetRepository;

public sealed class GetRepositoryQueryHandler
    : IRequestHandler<GetRepositoryQuery, RepositoryResponse?>
{
    private readonly IProjectRepositoryConfigurationRepository _repository;
    private readonly IProjectAuthorizationService _authorizationService;
    private readonly ILogger<GetRepositoryQueryHandler> _logger;

    public GetRepositoryQueryHandler(
        IProjectRepositoryConfigurationRepository repository,
        IProjectAuthorizationService authorizationService,
        ILogger<GetRepositoryQueryHandler> logger)
    {
        _repository = repository;
        _authorizationService = authorizationService;
        _logger = logger;
    }

    public async Task<RepositoryResponse?> Handle(
        GetRepositoryQuery query,
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
                : RepositoryResponseMapper.ToResponse(repository);
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
