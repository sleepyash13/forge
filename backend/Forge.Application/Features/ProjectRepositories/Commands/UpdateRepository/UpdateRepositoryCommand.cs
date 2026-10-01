using Forge.Application.DTOs.Projects.Repository;
using MediatR;

namespace Forge.Application.Features.ProjectRepositories.Commands.UpdateRepository;

public sealed record UpdateRepositoryCommand(
    Guid UserId,
    Guid ProjectId,
    UpdateRepositoryRequest Request) : IRequest<RepositoryResponse>;
