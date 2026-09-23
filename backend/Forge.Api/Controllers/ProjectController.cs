using Forge.Application.Common.Extensions;
using Forge.Application.DTOs.Projects;
using Forge.Application.Interfaces.Projects;
using Forge.Application.Services.Projects;
using Forge.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace Forge.Api.Controllers
{
    [ApiController]
    [Route("api/v1/projects")]
    [Authorize]

    public class ProjectController : ControllerBase
    {
        private readonly IProjectService _projectService;

        public ProjectController(IProjectService projectService)
        {
            _projectService = projectService;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateProjectRequest request)
        {
            var userId = User.GetUserId();

            var result = await _projectService.CreateAsync(userId, request);

            return CreatedAtAction(nameof(GetById), new { projectId = result.Id }, result);
        }

        [HttpGet("{projectId:guid}")]
        public async Task<IActionResult> GetById(Guid projectId)
        {
            var userId = User.GetUserId();

            var result = await _projectService.GetByIdAsync(userId, projectId);

            return Ok(result);
        }

        [HttpPut("{projectId:guid}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(Guid projectId, UpdateProjectRequest request)
        {
            var userId = User.GetUserId();

            var result = await _projectService.UpdateAsync(userId, projectId, request);

            return Ok(result);
        }

        [HttpDelete("{projectId:guid}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid projectId)
        {
            var userId = User.GetUserId();

            await _projectService.DeleteAsync(userId, projectId);

            return NoContent();
        }

        [HttpGet]
        public async Task<IActionResult> GetMyProjects()
        {
            var userId = User.GetUserId();

            var result = await _projectService.GetMyProjectsAsync(userId);

            return Ok(result);
        }
    }
}
