using Forge.Application.DTOs.Projects.Repository;
using Forge.Application.Interfaces.Projects;
using Forge.Application.Interfaces.Projects.Repository;
using Forge.Application.Validators;
using Forge.Domain.Enums;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Forge.Application.Features.ProjectRepositories.Commands.UpdateRepository;

public sealed class UpdateRepositoryCommandHandler
    : IRequestHandler<UpdateRepositoryCommand, RepositoryResponse>
{
    private readonly IProjectRepositoryConfigurationRepository _repository;
    private readonly IProjectAuthorizationService _authorizationService;
    private readonly ILogger<UpdateRepositoryCommandHandler> _logger;

    public UpdateRepositoryCommandHandler(
        IProjectRepositoryConfigurationRepository repository,
        IProjectAuthorizationService authorizationService,
        ILogger<UpdateRepositoryCommandHandler> logger)
    {
        _repository = repository;
        _authorizationService = authorizationService;
        _logger = logger;
    }

    public async Task<RepositoryResponse> Handle(
        UpdateRepositoryCommand command,
        CancellationToken cancellationToken)
    {
        await _authorizationService.HasPermissionAsync(
            command.UserId,
            command.ProjectId,
            ProjectPermission.ManageSettings);
        ProjectRepositoryConfigurationValidator.Validate(command.Request);

        try
        {
            var repository = await _repository.GetByProjectIdAsync(command.ProjectId);
            if (repository is null)
            {
                throw new KeyNotFoundException("Repository configuration was not found.");
            }

            repository.RepositoryUrl = command.Request.RepositoryUrl.Trim();
            repository.DefaultBranch = command.Request.DefaultBranch.Trim();
            repository.BuildCommand = NormalizeOptional(command.Request.BuildCommand);
            repository.DeployConfiguration = NormalizeOptional(command.Request.DeployConfiguration);
            repository.UpdatedAt = DateTime.UtcNow;

            await _repository.SaveChangesAsync();

            _logger.LogInformation(
                "Repository configuration {RepositoryId} updated for project {ProjectId} by user {UserId}",
                repository.Id,
                command.ProjectId,
                command.UserId);

            return RepositoryResponseMapper.ToResponse(repository);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Failed to update repository configuration for project {ProjectId} by user {UserId}",
                command.ProjectId,
                command.UserId);
            throw;
        }
    }

    private static string? NormalizeOptional(string? value) =>
        string.IsNullOrWhiteSpace(value) ? null : value.Trim();
}
