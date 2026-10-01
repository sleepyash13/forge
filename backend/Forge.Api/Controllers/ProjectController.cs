using Forge.Application.Common.Extensions;
using Forge.Application.DTOs.Projects;
using Forge.Application.Features.Projects.Commands.ArchiveProject;
using Forge.Application.Features.Projects.Commands.CreateProject;
using Forge.Application.Features.Projects.Commands.DeleteProject;
using Forge.Application.Features.Projects.Commands.UpdateProject;
using Forge.Application.Features.Projects.Queries.GetMyProjects;
using Forge.Application.Features.Projects.Queries.GetProjectById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Forge.Api.Controllers
{
    [ApiController]
    [Route("api/v1/projects")]
    [Authorize]
    public class ProjectController : ControllerBase
    {
        private readonly ISender _sender;

        public ProjectController(ISender sender)
        {
            _sender = sender;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            CreateProjectRequest request,
            CancellationToken cancellationToken)
        {
            var userId = User.GetUserId();
            var result = await _sender.Send(
                new CreateProjectCommand(userId, request),
                cancellationToken);

            return CreatedAtAction(nameof(GetById), new { projectId = result.Id }, result);
        }

        [HttpGet("{projectId:guid}")]
        public async Task<IActionResult> GetById(
            Guid projectId,
            CancellationToken cancellationToken)
        {
            var userId = User.GetUserId();
            var result = await _sender.Send(
                new GetProjectByIdQuery(userId, projectId),
                cancellationToken);

            return Ok(result);
        }

        [HttpPut("{projectId:guid}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(
            Guid projectId,
            UpdateProjectRequest request,
            CancellationToken cancellationToken)
        {
            var userId = User.GetUserId();
            var result = await _sender.Send(
                new UpdateProjectCommand(userId, projectId, request),
                cancellationToken);

            return Ok(result);
        }

        [HttpDelete("{projectId:guid}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(
            Guid projectId,
            CancellationToken cancellationToken)
        {
            var userId = User.GetUserId();
            await _sender.Send(
                new DeleteProjectCommand(userId, projectId),
                cancellationToken);

            return NoContent();
        }

        [HttpGet]
        public async Task<IActionResult> GetMyProjects(CancellationToken cancellationToken)
        {
            var userId = User.GetUserId();
            var result = await _sender.Send(
                new GetMyProjectsQuery(userId),
                cancellationToken);

            return Ok(result);
        }

        [HttpPost("{projectId:guid}/archive")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Archive(
            Guid projectId,
            CancellationToken cancellationToken)
        {
            var userId = User.GetUserId();
            await _sender.Send(
                new ArchiveProjectCommand(userId, projectId),
                cancellationToken);

            return Ok(new
            {
                success = true,
                message = "Project archived successfully."
            });
        }
    }
}
