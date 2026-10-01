using Forge.Application.Interfaces.Projects;
using Forge.Domain.Enums;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Forge.Application.Features.Projects.Commands.ArchiveProject;

public sealed class ArchiveProjectCommandHandler
    : IRequestHandler<ArchiveProjectCommand, Unit>
{
    private readonly IProjectRepository _projectRepository;
    private readonly IProjectAuthorizationService _authorizationService;
    private readonly ILogger<ArchiveProjectCommandHandler> _logger;

    public ArchiveProjectCommandHandler(
        IProjectRepository projectRepository,
        IProjectAuthorizationService authorizationService,
        ILogger<ArchiveProjectCommandHandler> logger)
    {
        _projectRepository = projectRepository;
        _authorizationService = authorizationService;
        _logger = logger;
    }

    public async Task<Unit> Handle(
        ArchiveProjectCommand command,
        CancellationToken cancellationToken)
    {
        try
        {
            var allowed = await _authorizationService.HasPermissionAsync(
                command.UserId,
                command.ProjectId,
                ProjectPermission.ArchiveProject);

            if (!allowed)
            {
                throw new UnauthorizedAccessException(
                    "You do not have permission to archive this project.");
            }

            var project = await _projectRepository.GetByIdAsync(command.ProjectId);
            if (project is null)
            {
                throw new KeyNotFoundException("Project not found.");
            }

            if (!project.IsActive)
            {
                throw new InvalidOperationException("Project is already archived.");
            }

            project.IsActive = false;
            project.UpdatedAt = DateTime.UtcNow;

            await _projectRepository.SaveChangesAsync();

            _logger.LogInformation(
                "Project {ProjectId} archived by user {UserId}",
                command.ProjectId,
                command.UserId);

            return Unit.Value;
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Error archiving project {ProjectId} by user {UserId}",
                command.ProjectId,
                command.UserId);
            throw;
        }
    }
}
