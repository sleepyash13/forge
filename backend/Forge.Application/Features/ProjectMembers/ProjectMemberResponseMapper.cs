using Forge.Application.DTOs.Projects;
using Forge.Domain.Entities;

namespace Forge.Application.Features.ProjectMembers;

internal static class ProjectMemberResponseMapper
{
    public static ProjectMemberResponse ToResponse(ProjectMember member)
    {
        var user = member.User
            ?? throw new InvalidOperationException(
                "Project member user information was not loaded.");

        return new ProjectMemberResponse
        {
            UserId = user.Id,
            Email = user.Email,
            FirstName = user.FirstName,
            MiddleName = user.MiddleName,
            LastName = user.LastName,
            DisplayName = user.DisplayName,
            Role = member.Role.ToString(),
            CreatedAt = member.CreatedAt,
            UpdatedAt = member.UpdatedAt
        };
    }
}
