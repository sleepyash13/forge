using Forge.Application.DTOs.Projects;
using Forge.Application.Interfaces.Projects;
using Forge.Application.Mappers;
using Forge.Application.Validators;
using Forge.Domain.Entities;
using Forge.Domain.Enums;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Forge.Application.Features.Projects.Commands.CreateProject;

public sealed class CreateProjectCommandHandler
    : IRequestHandler<CreateProjectCommand, ProjectResponse>
{
    private readonly IProjectRepository _projectRepository;
    private readonly IProjectMemberRepository _memberRepository;
    private readonly ILogger<CreateProjectCommandHandler> _logger;

    public CreateProjectCommandHandler(
        IProjectRepository projectRepository,
        IProjectMemberRepository memberRepository,
        ILogger<CreateProjectCommandHandler> logger)
    {
        _projectRepository = projectRepository;
        _memberRepository = memberRepository;
        _logger = logger;
    }

    public async Task<ProjectResponse> Handle(
        CreateProjectCommand command,
        CancellationToken cancellationToken)
    {
        try
        {
            ProjectValidator.ValidateName(command.Request.Name);

            var project = new Project
            {
                Id = Guid.NewGuid(),
                Name = ProjectValidator.NormalizeName(command.Request.Name),
                Description = ProjectValidator.NormalizeDescription(command.Request.Description),
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            };

            await _projectRepository.AddAsync(project);

            var ownerMembership = new ProjectMember
            {
                Id = Guid.NewGuid(),
                ProjectId = project.Id,
                UserId = command.UserId,
                Role = ProjectRole.Owner,
                CreatedAt = DateTime.UtcNow
            };

            await _memberRepository.AddAsync(ownerMembership);
            await _projectRepository.SaveChangesAsync();

            _logger.LogInformation(
                "Project {ProjectId} created by user {UserId}",
                project.Id,
                command.UserId);

            return ProjectMapper.ToResponse(project);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating project for user {UserId}", command.UserId);
            throw;
        }
    }
}
