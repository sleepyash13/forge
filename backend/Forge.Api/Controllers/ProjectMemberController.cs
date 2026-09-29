using Forge.Application.Common.Extensions;
using Forge.Application.DTOs.Projects;
using Forge.Application.Interfaces.Projects;
using Forge.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Forge.Api.Controllers
{
    [ApiController]
    [Route("api/v1/projects/{projectId:guid}/members")]
    [Authorize]
    public class ProjectMemberController : ControllerBase
    {
        private readonly IProjectMemberService _memberService;
        private readonly ILogger<ProjectMemberController> _logger;

        public ProjectMemberController(IProjectMemberService memberService, ILogger<ProjectMemberController> logger)
        { 
            _memberService = memberService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetMembers(Guid projectId)
        {
            var userId = User.GetUserId();
            var result = await _memberService.GetMembersAsync(userId, projectId);

            return Ok(result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddMember(Guid projectId, AddProjectMemberRequest request)
        {
            var userId = User.GetUserId();
            var result = await _memberService.AddMemberAsync(userId, projectId, request);

            return Ok(result);
        }

        [HttpPut("{targetUserId:guid}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateRole(Guid projectId, Guid targetUserId, UpdateProjectMemberRoleRequest request)
        {
            var userId = User.GetUserId();
            var result = await _memberService.UpdateRoleAsync(userId, projectId, targetUserId, request);

            return Ok(result);
        }

        [HttpDelete("{targetUserId:guid}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveMember(Guid projectId, Guid targetUserId)
        {
            var userId = User.GetUserId();
            await _memberService.RemoveMemberAsync(userId, projectId, targetUserId);

            return NoContent();
        }
    }
}
