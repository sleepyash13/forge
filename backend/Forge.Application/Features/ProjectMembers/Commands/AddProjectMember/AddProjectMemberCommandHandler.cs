using Forge.Application.DTOs.Projects;
using Forge.Application.Interfaces.Projects;
using Forge.Application.Interfaces.Users;
using Forge.Domain.Entities;
using Forge.Domain.Enums;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Forge.Application.Features.ProjectMembers.Commands.AddProjectMember;

public sealed class AddProjectMemberCommandHandler
    : IRequestHandler<AddProjectMemberCommand, ProjectMemberResponse>
{
    private readonly IProjectMemberRepository _memberRepository;
    private readonly IUserRepository _userRepository;
    private readonly IProjectAuthorizationService _authorizationService;
    private readonly ILogger<AddProjectMemberCommandHandler> _logger;

    public AddProjectMemberCommandHandler(
        IProjectMemberRepository memberRepository,
        IUserRepository userRepository,
        IProjectAuthorizationService authorizationService,
        ILogger<AddProjectMemberCommandHandler> logger)
    {
        _memberRepository = memberRepository;
        _userRepository = userRepository;
        _authorizationService = authorizationService;
        _logger = logger;
    }

    public async Task<ProjectMemberResponse> Handle(
        AddProjectMemberCommand command,
        CancellationToken cancellationToken)
    {
        try
        {
            var allowed = await _authorizationService.HasPermissionAsync(
                command.CurrentUserId,
                command.ProjectId,
                ProjectPermission.ManageMembers);

            if (!allowed)
            {
                throw new UnauthorizedAccessException(
                    "You do not have permission to manage project members.");
            }

            if (string.IsNullOrWhiteSpace(command.Request.Email))
            {
                throw new ArgumentException("Email is required.");
            }

            if (!Enum.TryParse<ProjectRole>(command.Request.Role, true, out var role))
            {
                throw new ArgumentException("Invalid project role.");
            }

            var normalizedEmail = command.Request.Email.Trim().ToLowerInvariant();
            var user = await _userRepository.GetByEmailAsync(normalizedEmail);
            if (user is null)
            {
                throw new KeyNotFoundException(
                    "A user with the specified email was not found.");
            }

            var existing = await _memberRepository.GetMembershipAsync(
                user.Id,
                command.ProjectId);
            if (existing is not null)
            {
                throw new InvalidOperationException(
                    "The user is already a member of this project.");
            }

            if (role == ProjectRole.Owner)
            {
                var currentMembership = await _memberRepository.GetMembershipAsync(
                    command.CurrentUserId,
                    command.ProjectId);

                if (currentMembership?.Role != ProjectRole.Owner)
                {
                    throw new UnauthorizedAccessException(
                        "Only the project owner can assign the Owner role.");
                }
            }

            var member = new ProjectMember
            {
                Id = Guid.NewGuid(),
                ProjectId = command.ProjectId,
                UserId = user.Id,
                Role = role,
                CreatedAt = DateTime.UtcNow
            };

            await _memberRepository.AddAsync(member);
            await _memberRepository.SaveChangesAsync();

            _logger.LogInformation(
                "User {TargetUserId} added to project {ProjectId} with role {Role} by user {CurrentUserId}",
                user.Id,
                command.ProjectId,
                role,
                command.CurrentUserId);

            member.User = user;
            return ProjectMemberResponseMapper.ToResponse(member);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Error adding member to project {ProjectId} by user {UserId}",
                command.ProjectId,
                command.CurrentUserId);
            throw;
        }
    }
}
