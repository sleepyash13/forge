using Forge.Application.Common.Extensions;
using Forge.Application.DTOs.Projects.Repository;
using Forge.Application.Features.ProjectRepositories.Commands.CreateRepository;
using Forge.Application.Features.ProjectRepositories.Commands.DeleteRepository;
using Forge.Application.Features.ProjectRepositories.Commands.UpdateRepository;
using Forge.Application.Features.ProjectRepositories.Queries.GetRepository;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Forge.Api.Controllers
{
    [Authorize]
    [Route("api/v1/projects/{projectId:guid}/repository")]
    [ApiController]

    public class ProjectRepositoryConfigurationController : ControllerBase
    {
        private readonly ISender _sender;

        public ProjectRepositoryConfigurationController(ISender sender)
        {
            _sender = sender;
        }

        [HttpGet]
        public async Task<IActionResult> Get(Guid projectId, CancellationToken cancellationToken)
        {
            var userId = User.GetUserId();
            var repository = await _sender.Send(
                new GetRepositoryQuery(userId, projectId),
                cancellationToken);

            if (repository == null)
            {
                return NotFound(new { success = false, message = "Repository configuration not found for this project." });
            }

            return Ok(repository);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            Guid projectId,
            [FromBody] CreateRepositoryRequest request,
            CancellationToken cancellationToken)
        {
            var userId = User.GetUserId();
            var repository = await _sender.Send(
                new CreateRepositoryCommand(userId, projectId, request),
                cancellationToken);

            return CreatedAtAction(nameof(Get), new { projectId }, repository);
        }

        [HttpPut]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(
            Guid projectId,
            [FromBody] UpdateRepositoryRequest request,
            CancellationToken cancellationToken)
        {
            var userId = User.GetUserId();
            var repository = await _sender.Send(
                new UpdateRepositoryCommand(userId, projectId, request),
                cancellationToken);

            return Ok(repository);
        }

        [HttpDelete]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid projectId, CancellationToken cancellationToken)
        {
            var userId = User.GetUserId();

            await _sender.Send(
                new DeleteRepositoryCommand(userId, projectId),
                cancellationToken);

            return Ok(new
            {
                success = true,
                message = "Repository configuration deleted successfully."
            });
        }
    }
}
