using Forge.Application.Interfaces.Projects;
using Forge.Application.Interfaces.Projects.Repository;
using Forge.Domain.Enums;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Forge.Application.Features.ProjectRepositories.Commands.DeleteRepository;

public sealed class DeleteRepositoryCommandHandler
    : IRequestHandler<DeleteRepositoryCommand, Unit>
{
    private readonly IProjectRepositoryConfigurationRepository _repository;
    private readonly IProjectAuthorizationService _authorizationService;
    private readonly ILogger<DeleteRepositoryCommandHandler> _logger;

    public DeleteRepositoryCommandHandler(
        IProjectRepositoryConfigurationRepository repository,
        IProjectAuthorizationService authorizationService,
        ILogger<DeleteRepositoryCommandHandler> logger)
    {
        _repository = repository;
        _authorizationService = authorizationService;
        _logger = logger;
    }

    public async Task<Unit> Handle(
        DeleteRepositoryCommand command,
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
