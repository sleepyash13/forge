using Forge.Application.Common.Extensions;
using Forge.Application.DTOs.Projects.Repository;
using Forge.Application.Interfaces.Projects.Repository;
using Forge.Application.Interfaces.Projects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Forge.Api.Controllers
{
    [Authorize]
    [Route("api/v1/projects/{projectId:guid}/repository")]
    [ApiController]

    public class ProjectRepositoryConfigurationController : ControllerBase
    {
        private readonly IProjectRepositoryConfigurationService _repositoryService;
        public ProjectRepositoryConfigurationController(IProjectRepositoryConfigurationService repositoryService)
        {
            _repositoryService = repositoryService;
        }

        [HttpGet]
        public async Task<IActionResult> Get(Guid projectId)
        {
            var userId = User.GetUserId();
            var repository = await _repositoryService.GetAsync(userId, projectId);

            if (repository == null)
            {
                return NotFound(new { success = false, message = "Repository configuration not found for this project." });
            }

            return Ok(repository);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Guid projectId, [FromBody] CreateRepositoryRequest request)
        {
            var userId = User.GetUserId();
            var repository = await _repositoryService.CreateAsync(userId, projectId, request);

            return CreatedAtAction(nameof(Get), new { projectId = projectId }, repository);
        }

        [HttpPut]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(Guid projectId, [FromBody] UpdateRepositoryRequest request)
        {
            var userId = User.GetUserId();
            var repository = await _repositoryService.UpdateAsync(userId, projectId, request);

            return Ok(repository);
        }

        [HttpDelete]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid projectId)
        {
            var userId = User.GetUserId();

            await _repositoryService.DeleteAsync(userId, projectId);

            return Ok(new
            {
                success = true,
                message = "Repository configuration deleted successfully."
            });
        }
    }
}
