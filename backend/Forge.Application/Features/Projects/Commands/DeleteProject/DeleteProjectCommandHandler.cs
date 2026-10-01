using Forge.Application.Interfaces.Projects;
using Forge.Domain.Enums;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Forge.Application.Features.Projects.Commands.DeleteProject;

public sealed class DeleteProjectCommandHandler
    : IRequestHandler<DeleteProjectCommand, Unit>
{
    private readonly IProjectRepository _projectRepository;
    private readonly IProjectAuthorizationService _authorizationService;
    private readonly ILogger<DeleteProjectCommandHandler> _logger;

    public DeleteProjectCommandHandler(
        IProjectRepository projectRepository,
        IProjectAuthorizationService authorizationService,
        ILogger<DeleteProjectCommandHandler> logger)
    {
        _projectRepository = projectRepository;
        _authorizationService = authorizationService;
        _logger = logger;
    }

    public async Task<Unit> Handle(
        DeleteProjectCommand command,
        CancellationToken cancellationToken)
    {
        try
        {
            var authorized = await _authorizationService.HasPermissionAsync(
                command.UserId,
                command.ProjectId,
                ProjectPermission.DeleteProject);

            if (!authorized)
            {
                throw new UnauthorizedAccessException(
                    "You do not have permission to delete this project.");
            }

            var project = await _projectRepository.GetByIdAsync(command.ProjectId);
            if (project is null)
            {
                throw new KeyNotFoundException("Project was not found.");
            }

            await _projectRepository.DeleteAsync(project);
            await _projectRepository.SaveChangesAsync();

            _logger.LogInformation(
                "Project {ProjectId} deleted by user {UserId}",
                command.ProjectId,
                command.UserId);

            return Unit.Value;
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Error deleting project {ProjectId} for user {UserId}",
                command.ProjectId,
                command.UserId);
            throw;
        }
    }
}
