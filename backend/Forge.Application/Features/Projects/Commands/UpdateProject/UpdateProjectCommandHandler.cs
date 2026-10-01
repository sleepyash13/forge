using Forge.Application.DTOs.Projects;
using Forge.Application.Interfaces.Projects;
using Forge.Application.Mappers;
using Forge.Application.Validators;
using Forge.Domain.Enums;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Forge.Application.Features.Projects.Commands.UpdateProject;

public sealed class UpdateProjectCommandHandler
    : IRequestHandler<UpdateProjectCommand, ProjectResponse>
{
    private readonly IProjectRepository _projectRepository;
    private readonly IProjectAuthorizationService _authorizationService;
    private readonly ILogger<UpdateProjectCommandHandler> _logger;

    public UpdateProjectCommandHandler(
        IProjectRepository projectRepository,
        IProjectAuthorizationService authorizationService,
        ILogger<UpdateProjectCommandHandler> logger)
    {
        _projectRepository = projectRepository;
        _authorizationService = authorizationService;
        _logger = logger;
    }

    public async Task<ProjectResponse> Handle(
        UpdateProjectCommand command,
        CancellationToken cancellationToken)
    {
        try
        {
            var authorized = await _authorizationService.HasPermissionAsync(
                command.UserId,
                command.ProjectId,
                ProjectPermission.UpdateProject);

            if (!authorized)
            {
                throw new UnauthorizedAccessException(
                    "You do not have permission to update this project.");
            }

            ProjectValidator.ValidateName(command.Request.Name);

            var project = await _projectRepository.GetByIdAsync(command.ProjectId);
            if (project is null)
            {
                throw new KeyNotFoundException("Project was not found.");
            }

            project.Name = ProjectValidator.NormalizeName(command.Request.Name);
            project.Description = ProjectValidator.NormalizeDescription(command.Request.Description);
            project.UpdatedAt = DateTime.UtcNow;

            await _projectRepository.SaveChangesAsync();

            _logger.LogInformation(
                "Project {ProjectId} updated by user {UserId}",
                command.ProjectId,
                command.UserId);

            return ProjectMapper.ToResponse(project);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Error updating project {ProjectId} for user {UserId}",
                command.ProjectId,
                command.UserId);
            throw;
        }
    }
}
