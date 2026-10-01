using Forge.Application.DTOs.Projects;
using Forge.Application.Interfaces.Projects;
using Forge.Application.Mappers;
using Forge.Domain.Enums;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Forge.Application.Features.Projects.Queries.GetProjectById;

public sealed class GetProjectByIdQueryHandler
    : IRequestHandler<GetProjectByIdQuery, ProjectResponse>
{
    private readonly IProjectRepository _projectRepository;
    private readonly IProjectAuthorizationService _authorizationService;
    private readonly ILogger<GetProjectByIdQueryHandler> _logger;

    public GetProjectByIdQueryHandler(
        IProjectRepository projectRepository,
        IProjectAuthorizationService authorizationService,
        ILogger<GetProjectByIdQueryHandler> logger)
    {
        _projectRepository = projectRepository;
        _authorizationService = authorizationService;
        _logger = logger;
    }

    public async Task<ProjectResponse> Handle(
        GetProjectByIdQuery query,
        CancellationToken cancellationToken)
    {
        try
        {
            var authorized = await _authorizationService.HasPermissionAsync(
                query.UserId,
                query.ProjectId,
                ProjectPermission.ViewProject);

            if (!authorized)
            {
                throw new UnauthorizedAccessException(
                    "You do not have permission to view this project.");
            }

            var project = await _projectRepository.GetByIdAsync(query.ProjectId);
            if (project is null)
            {
                throw new KeyNotFoundException("Project was not found.");
            }

            return ProjectMapper.ToResponse(project);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Error retrieving project {ProjectId} for user {UserId}",
                query.ProjectId,
                query.UserId);
            throw;
        }
    }
}
