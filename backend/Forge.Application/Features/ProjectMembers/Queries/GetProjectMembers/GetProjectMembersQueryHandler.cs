using Forge.Application.DTOs.Projects;
using Forge.Application.Interfaces.Projects;
using Forge.Domain.Enums;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Forge.Application.Features.ProjectMembers.Queries.GetProjectMembers;

public sealed class GetProjectMembersQueryHandler
    : IRequestHandler<GetProjectMembersQuery, List<ProjectMemberResponse>>
{
    private readonly IProjectMemberRepository _memberRepository;
    private readonly IProjectAuthorizationService _authorizationService;
    private readonly ILogger<GetProjectMembersQueryHandler> _logger;

    public GetProjectMembersQueryHandler(
        IProjectMemberRepository memberRepository,
        IProjectAuthorizationService authorizationService,
        ILogger<GetProjectMembersQueryHandler> logger)
    {
        _memberRepository = memberRepository;
        _authorizationService = authorizationService;
        _logger = logger;
    }

    public async Task<List<ProjectMemberResponse>> Handle(
        GetProjectMembersQuery query,
        CancellationToken cancellationToken)
    {
        try
        {
            var allowed = await _authorizationService.HasPermissionAsync(
                query.CurrentUserId,
                query.ProjectId,
                ProjectPermission.ViewProject);

            if (!allowed)
            {
                throw new UnauthorizedAccessException(
                    "You do not have permission to view this project.");
            }

            var members = await _memberRepository.GetProjectMembersAsync(query.ProjectId);
            return members.Select(ProjectMemberResponseMapper.ToResponse).ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Error retrieving members for project {ProjectId} by user {UserId}",
                query.ProjectId,
                query.CurrentUserId);
            throw;
        }
    }
}
