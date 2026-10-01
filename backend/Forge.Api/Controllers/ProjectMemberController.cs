using Forge.Application.Common.Extensions;
using Forge.Application.DTOs.Projects;
using Forge.Application.Features.ProjectMembers.Commands.AddProjectMember;
using Forge.Application.Features.ProjectMembers.Commands.RemoveProjectMember;
using Forge.Application.Features.ProjectMembers.Commands.UpdateProjectMemberRole;
using Forge.Application.Features.ProjectMembers.Queries.GetProjectMembers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Forge.Api.Controllers
{
    [ApiController]
    [Route("api/v1/projects/{projectId:guid}/members")]
    [Authorize]
    public class ProjectMemberController : ControllerBase
    {
        private readonly ISender _sender;

        public ProjectMemberController(ISender sender)
        {
            _sender = sender;
        }

        [HttpGet]
        public async Task<IActionResult> GetMembers(
            Guid projectId,
            CancellationToken cancellationToken)
        {
            var userId = User.GetUserId();
            var result = await _sender.Send(
                new GetProjectMembersQuery(userId, projectId),
                cancellationToken);

            return Ok(result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddMember(
            Guid projectId,
            AddProjectMemberRequest request,
            CancellationToken cancellationToken)
        {
            var userId = User.GetUserId();
            var result = await _sender.Send(
                new AddProjectMemberCommand(userId, projectId, request),
                cancellationToken);

            return Ok(result);
        }

        [HttpPut("{targetUserId:guid}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateRole(
            Guid projectId,
            Guid targetUserId,
            UpdateProjectMemberRoleRequest request,
            CancellationToken cancellationToken)
        {
            var userId = User.GetUserId();
            var result = await _sender.Send(
                new UpdateProjectMemberRoleCommand(
                    userId,
                    projectId,
                    targetUserId,
                    request),
                cancellationToken);

            return Ok(result);
        }

        [HttpDelete("{targetUserId:guid}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveMember(
            Guid projectId,
            Guid targetUserId,
            CancellationToken cancellationToken)
        {
            var userId = User.GetUserId();
            await _sender.Send(
                new RemoveProjectMemberCommand(userId, projectId, targetUserId),
                cancellationToken);

            return NoContent();
        }
    }
}
