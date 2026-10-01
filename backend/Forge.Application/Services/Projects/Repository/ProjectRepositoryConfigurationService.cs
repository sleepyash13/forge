using Forge.Application.DTOs.Projects.Repository;
using Forge.Application.Interfaces.Projects.Repository;
using Forge.Application.Interfaces.Projects;
using Forge.Application.Validators;
using Forge.Domain.Entities;
using Microsoft.Extensions.Logging;
using Forge.Domain.Enums;

namespace Forge.Application.Services.Projects.Repository
{
    public class ProjectRepositoryConfigurationService: IProjectRepositoryConfigurationService
    {
        private readonly IProjectRepositoryConfigurationRepository _repositoryRepository;
        private readonly IProjectAuthorizationService _authorizationService;
        private readonly ILogger < ProjectRepositoryConfigurationService > _logger;
        public ProjectRepositoryConfigurationService(IProjectRepositoryConfigurationRepository repositoryRepository, 
            IProjectAuthorizationService authorizationService, 
            ILogger<ProjectRepositoryConfigurationService> logger)
        {
            _repositoryRepository = repositoryRepository;
            _authorizationService = authorizationService;
            _logger = logger;
        }

        public async Task<RepositoryResponse?> GetAsync(Guid userId, Guid projectId)
        {
            await _authorizationService.HasPermissionAsync(userId, projectId, ProjectPermission.ViewProject);

            try
            {
                var repository = await _repositoryRepository.GetByProjectIdAsync(projectId);
                if (repository == null)
                {
                    return null;
                }
                return MapToResponse(repository);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to retrieve repository configuration for project {ProjectId} by user {UserId}", projectId, userId);
                throw;
            }
        }

        public async Task<RepositoryResponse> CreateAsync(Guid userId, Guid projectId, CreateRepositoryRequest request)
        {
            await _authorizationService.HasPermissionAsync(userId, projectId, ProjectPermission.ManageSettings);
                ProjectRepositoryConfigurationValidator.Validate(request);

            try
            {
                var existingRepository = await _repositoryRepository.GetByProjectIdAsync(projectId);
                if (existingRepository != null)
                {
                    throw new InvalidOperationException("This project already has a repository configuration.");
                }

                var repository = new ProjectRepositoryConfiguration
                {
                    Id = Guid.NewGuid(),
                    ProjectId = projectId,
                    RepositoryUrl = request.RepositoryUrl.Trim(),
                    DefaultBranch = request.DefaultBranch.Trim(),
                    BuildCommand = string.IsNullOrWhiteSpace(request.BuildCommand) ? null : request.BuildCommand.Trim(),
                    DeployConfiguration = string.IsNullOrWhiteSpace(request.DeployConfiguration) ? null : request.DeployConfiguration.Trim(),
                    CreatedAt = DateTime.UtcNow
                };
                await _repositoryRepository.AddAsync(repository);
                await _repositoryRepository.SaveChangesAsync();

                _logger.LogInformation("Repository configuration created for project {ProjectId} by user {UserId}. Repository {RepositoryId}", 
                    projectId, userId, repository.Id);

                return MapToResponse(repository);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create repository configuration for project {ProjectId} by user {UserId}", 
                    projectId, userId);
                throw;
            }
        }

        public async Task<RepositoryResponse> UpdateAsync(Guid userId, Guid projectId, UpdateRepositoryRequest request)
        {
            await _authorizationService.HasPermissionAsync(userId, projectId, ProjectPermission.ManageSettings);
                ProjectRepositoryConfigurationValidator.Validate(request);
            try
            {
                var repository = await _repositoryRepository.GetByProjectIdAsync(projectId);
                if (repository == null)
                {
                    throw new KeyNotFoundException("Repository configuration was not found.");
                }
                repository.RepositoryUrl = request.RepositoryUrl.Trim();
                repository.DefaultBranch = request.DefaultBranch.Trim();
                repository.BuildCommand = string.IsNullOrWhiteSpace(request.BuildCommand) ? null : request.BuildCommand.Trim();
                repository.DeployConfiguration = string.IsNullOrWhiteSpace(request.DeployConfiguration) ? null : request.DeployConfiguration.Trim();
                repository.UpdatedAt = DateTime.UtcNow;

                await _repositoryRepository.SaveChangesAsync();

                _logger.LogInformation("Repository configuration {RepositoryId} updated for project {ProjectId} by user {UserId}", 
                    repository.Id, projectId, userId);
                return MapToResponse(repository);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update repository configuration for project {ProjectId} by user {UserId}", 
                    projectId, userId);
                throw;
            }
        }

        public async Task DeleteAsync(Guid userId, Guid projectId)
        {
            await _authorizationService.HasPermissionAsync(userId, projectId, ProjectPermission.ManageSettings);
            try
            {
                var repository = await _repositoryRepository.GetByProjectIdAsync(projectId);
                if (repository == null)
                {
                    throw new KeyNotFoundException("Repository configuration was not found.");
                }
                await _repositoryRepository.RemoveAsync(repository);
                await _repositoryRepository.SaveChangesAsync();

                _logger.LogInformation("Repository configuration {RepositoryId} deleted from project {ProjectId} by user {UserId}", 
                    repository.Id, projectId, userId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to delete repository configuration for project {ProjectId} by user {UserId}", 
                    projectId, userId);
                throw;
            }
        }

        private static RepositoryResponse MapToResponse(ProjectRepositoryConfiguration repository)
        {
            return new RepositoryResponse
            {
                Id = repository.Id,
                ProjectId = repository.ProjectId,
                RepositoryUrl = repository.RepositoryUrl,
                DefaultBranch = repository.DefaultBranch,
                BuildCommand = repository.BuildCommand,
                DeployConfiguration = repository.DeployConfiguration,
                CreatedAt = repository.CreatedAt,
                UpdatedAt = repository.UpdatedAt
            };
        }
    }
}