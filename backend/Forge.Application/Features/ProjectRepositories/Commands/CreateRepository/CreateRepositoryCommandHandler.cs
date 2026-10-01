using Forge.Application.DTOs.Projects.Repository;
using Forge.Application.Interfaces.Projects;
using Forge.Application.Interfaces.Projects.Repository;
using Forge.Application.Validators;
using Forge.Domain.Entities;
using Forge.Domain.Enums;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Forge.Application.Features.ProjectRepositories.Commands.CreateRepository;

public sealed class CreateRepositoryCommandHandler
    : IRequestHandler<CreateRepositoryCommand, RepositoryResponse>
{
    private readonly IProjectRepositoryConfigurationRepository _repository;
    private readonly IProjectAuthorizationService _authorizationService;
    private readonly ILogger<CreateRepositoryCommandHandler> _logger;

    public CreateRepositoryCommandHandler(
        IProjectRepositoryConfigurationRepository repository,
        IProjectAuthorizationService authorizationService,
        ILogger<CreateRepositoryCommandHandler> logger)
    {
        _repository = repository;
        _authorizationService = authorizationService;
        _logger = logger;
    }

    public async Task<RepositoryResponse> Handle(
        CreateRepositoryCommand command,
        CancellationToken cancellationToken)
    {
        await _authorizationService.HasPermissionAsync(
            command.UserId,
            command.ProjectId,
            ProjectPermission.ManageSettings);
        ProjectRepositoryConfigurationValidator.Validate(command.Request);

        try
        {
            var existingRepository = await _repository.GetByProjectIdAsync(command.ProjectId);
            if (existingRepository is not null)
            {
                throw new InvalidOperationException(
                    "This project already has a repository configuration.");
            }

            var repository = new ProjectRepositoryConfiguration
            {
                Id = Guid.NewGuid(),
                ProjectId = command.ProjectId,
                RepositoryUrl = command.Request.RepositoryUrl.Trim(),
                DefaultBranch = command.Request.DefaultBranch.Trim(),
                BuildCommand = NormalizeOptional(command.Request.BuildCommand),
                DeployConfiguration = NormalizeOptional(command.Request.DeployConfiguration),
                CreatedAt = DateTime.UtcNow
            };

            await _repository.AddAsync(repository);
            await _repository.SaveChangesAsync();

            _logger.LogInformation(
                "Repository configuration created for project {ProjectId} by user {UserId}. Repository {RepositoryId}",
                command.ProjectId,
                command.UserId,
                repository.Id);

            return RepositoryResponseMapper.ToResponse(repository);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Failed to create repository configuration for project {ProjectId} by user {UserId}",
                command.ProjectId,
                command.UserId);
            throw;
        }
    }

    private static string? NormalizeOptional(string? value) =>
        string.IsNullOrWhiteSpace(value) ? null : value.Trim();
}
