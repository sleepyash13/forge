using Forge.Application.DTOs.Projects;
using Forge.Application.Interfaces.Projects;
using Forge.Application.Mappers;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Forge.Application.Features.Projects.Queries.GetMyProjects;

public sealed class GetMyProjectsQueryHandler
    : IRequestHandler<GetMyProjectsQuery, List<ProjectResponse>>
{
    private readonly IProjectRepository _projectRepository;
    private readonly ILogger<GetMyProjectsQueryHandler> _logger;

    public GetMyProjectsQueryHandler(
        IProjectRepository projectRepository,
        ILogger<GetMyProjectsQueryHandler> logger)
    {
        _projectRepository = projectRepository;
        _logger = logger;
    }

    public async Task<List<ProjectResponse>> Handle(
        GetMyProjectsQuery query,
        CancellationToken cancellationToken)
    {
        try
        {
            var projects = await _projectRepository.GetByUserIdAsync(query.UserId);
            return projects.Select(ProjectMapper.ToResponse).ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving projects for user {UserId}", query.UserId);
            throw;
        }
    }
}
