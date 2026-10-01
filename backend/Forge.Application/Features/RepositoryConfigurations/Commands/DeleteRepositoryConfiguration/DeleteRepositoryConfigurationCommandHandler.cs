using Forge.Application.Interfaces.Projects;
using Forge.Application.Interfaces.Projects.Repository;
using Forge.Domain.Enums;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Forge.Application.Features.RepositoryConfigurations.Commands.DeleteRepositoryConfiguration;

public sealed class DeleteRepositoryConfigurationCommandHandler
    : IRequestHandler<DeleteRepositoryConfigurationCommand, Unit>
{
    private readonly IProjectRepositoryConfigurationRepository _repository;
    private readonly IProjectAuthorizationService _authorizationService;
    private readonly ILogger<DeleteRepositoryConfigurationCommandHandler> _logger;

    public DeleteRepositoryConfigurationCommandHandler(
        IProjectRepositoryConfigurationRepository repository,
        IProjectAuthorizationService authorizationService,
        ILogger<DeleteRepositoryConfigurationCommandHandler> logger)
    {
        _repository = repository;
        _authorizationService = authorizationService;
        _logger = logger;
    }

    public async Task<Unit> Handle(
        DeleteRepositoryConfigurationCommand command,
        CancellationToken cancellationToken)
    {
        await _authorizationService.HasPermissionAsync(
            command.UserId,
            command.ProjectId,
            ProjectPermission.ManageSettings);

        try
        {
            var repository = await _repository.GetByProjectIdAsync(command.ProjectId);
            if (repository is null)
            {
                throw new KeyNotFoundException("Repository configuration was not found.");
            }

            await _repository.RemoveAsync(repository);
            await _repository.SaveChangesAsync();

            _logger.LogInformation(
                "Repository configuration {RepositoryId} deleted from project {ProjectId} by user {UserId}",
                repository.Id,
                command.ProjectId,
                command.UserId);

            return Unit.Value;
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Failed to delete repository configuration for project {ProjectId} by user {UserId}",
                command.ProjectId,
                command.UserId);
            throw;
        }
    }
}
